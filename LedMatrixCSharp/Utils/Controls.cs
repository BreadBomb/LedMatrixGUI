using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.PiGpio;
using Unosquare.WiringPi;

namespace LedMatrixCSharp.Utils
{
    public class Scroller
    {
        public int Position { get; set; }
        public IGpioPin UpPin { get; set; }
        public IGpioPin DownPin { get; set; }
        public int LastPin { get; set; }
    }

    public class Controls
    {
        private static Controls controls;
        
        private static Dictionary<string, IGpioPin> Buttons = new Dictionary<string, IGpioPin>();
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
            Pi.Init<BootstrapWiringPi>();

            var rot = Pi.Gpio[20];
            rot.PinMode = GpioPinDriveMode.Output;
            var blau = Pi.Gpio[6];
            blau.PinMode = GpioPinDriveMode.Output;
            var gruen = Pi.Gpio[13];
            gruen.PinMode = GpioPinDriveMode.Output;
            rot.Value = true;
            blau.Value = false;
            gruen.Value = true;
        }

        public void AddButton(string name, P1 pin)
        {
            var button = Pi.Gpio[pin];
            button.PinMode = GpioPinDriveMode.Input;
            button.InputPullMode = GpioPinResistorPullMode.PullDown;
            Buttons.Add(name, button);
        }
        
        public void AddScroller(string name, P1 up, P1 down)
        {
            var upPin = Pi.Gpio[up];
            upPin.PinMode = GpioPinDriveMode.Input;
            var downPin = Pi.Gpio[down];
            downPin.PinMode = GpioPinDriveMode.Input;

            var scroller = new Scroller()
            {
                UpPin = upPin,
                DownPin = downPin
            };

            Scrollers.Add(name, scroller);

            //Console.WriteLine("Registered Scroller " + name);
        }

        public void OnButtonClick(string name, Action action)
        {
            Buttons[name].RegisterInterruptCallback(EdgeDetection.RisingEdge, action);
        }

        public void OnScrollerScrolled(string name, Action<int, Scroller> action)
        {
            if (!Scrollers.ContainsKey(name)) return;
            var scroller = Scrollers[name];

            Action<int ,bool> pinChanged = (gpio, status) =>
            {
                if (gpio != scroller.LastPin)
                {
                    scroller.LastPin = gpio;

                    if (gpio == scroller.UpPin.BcmPinNumber && !status)
                    {
                        if (!scroller.DownPin.Value)
                        {
                            scroller.Position++;
                            action.Invoke(0, scroller);
                        }
                    }
                    else if (gpio == scroller.DownPin.BcmPinNumber && status)
                    {
                        if (scroller.UpPin.Value)
                        {
                            scroller.Position--;
                            action.Invoke(1, scroller);
                        }
                    }
                }
            };

            scroller.UpPin.RegisterInterruptCallback(EdgeDetection.FallingEdge, () => pinChanged.Invoke(scroller.UpPin.BcmPinNumber, scroller.UpPin.Value));
            scroller.DownPin.RegisterInterruptCallback(EdgeDetection.FallingEdge, () => pinChanged.Invoke(scroller.DownPin.BcmPinNumber, scroller.DownPin.Value));
        }
    }
}