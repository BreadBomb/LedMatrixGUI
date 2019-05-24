using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using Unosquare.PiGpio.NativeMethods;
using Unosquare.PiGpio.NativeEnums;
using Unosquare.PiGpio.NativeTypes;

namespace LedMatrixCSharp.Utils
{
    public class Scroller
    {
        public int Position { get; set; }
        public SystemGpio UpPin { get; set; }
        public SystemGpio DownPin { get; set; }
        public UserGpio LastPin { get; set; }
    }

    public class Controls
    {
        private static Controls controls;
        
        private static Dictionary<string, SystemGpio> Buttons = new Dictionary<string, SystemGpio>();
        private static Dictionary<string, Scroller> Scrollers = new Dictionary<string, Scroller>();

        public static Controls Instance
        {
            get
            {
                if (controls == null)
                {
                    controls = new Controls();
                }
                return controls;
            }
        }

        public Controls()
        {
            Setup.GpioInitialise();

            var rot = SystemGpio.Bcm20;
            var gruen = SystemGpio.Bcm13;
            var blau = SystemGpio.Bcm06;
            IO.GpioSetMode(rot, PinMode.Output);
            IO.GpioSetMode(gruen, PinMode.Output);
            IO.GpioSetMode(blau, PinMode.Output);

            IO.GpioWrite(rot, true);
            IO.GpioWrite(gruen, false);
            IO.GpioWrite(blau, true);
        }

        public void AddButton(string name, SystemGpio pin)
        {
            IO.GpioSetMode(pin, PinMode.Input);
            IO.GpioSetPullUpDown(pin, GpioPullMode.Down);
            Buttons.Add(name, pin);
        }
        
        public void AddScroller(string name, SystemGpio up, SystemGpio down)
        {
            IO.GpioSetMode(up, PinMode.Input);
            IO.GpioSetMode(down, PinMode.Input);

            var scroller = new Scroller()
            {
                UpPin = up,
                DownPin = down
            };

            Scrollers.Add(name, scroller);

            //Console.WriteLine("Registered Scroller " + name);
        }

        public void OnButtonClick(string name, PiGpioAlertDelegate action)
        {
            var userGpio = (UserGpio)Buttons[name];

            IO.GpioSetWatchdog(userGpio, 10);
            IO.GpioSetAlertFunc(userGpio, (gpio, level, time) => {
                if (level == LevelChange.LowToHigh)
                {
                    action.Invoke(gpio, level, time);
                }
            });
        }

        public void OnScrollerScrolled(string name, Action<int, Scroller> action)
        {
            if (!Scrollers.ContainsKey(name)) return;
            var scroller = Scrollers[name];

            var userGpioUp = (UserGpio)scroller.UpPin;
            var userGpioDown = (UserGpio)scroller.DownPin;

            IO.GpioSetWatchdog(userGpioUp, 10);
            IO.GpioSetWatchdog(userGpioDown, 10);

            Action<UserGpio ,bool> pinChanged = (gpio, status) =>
            {
                if (gpio != scroller.LastPin)
                {
                    scroller.LastPin = gpio;

                    if (gpio == userGpioUp && !status)
                    {
                        if (!IO.GpioRead(scroller.DownPin))
                        {
                            scroller.Position++;
                            action.Invoke(0, scroller);
                        }
                    }
                    else if (gpio == userGpioDown && status)
                    {
                        if (IO.GpioRead(scroller.UpPin))
                        {
                            scroller.Position--;
                            action.Invoke(1, scroller);
                        }
                    }
                }
            };

            IO.GpioSetAlertFunc(userGpioUp, (gpio, level, time) => {
                if (level == LevelChange.LowToHigh)
                {
                    pinChanged.Invoke(gpio, IO.GpioRead((SystemGpio)gpio));
                }
            });
            IO.GpioSetAlertFunc(userGpioDown, (gpio, level, time) =>
            {
                if (level == LevelChange.LowToHigh)
                {
                    pinChanged.Invoke(gpio, IO.GpioRead((SystemGpio)gpio));
                }
            });
        }
    }
}