using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using LedMatrixCSharp.View.Layout;
using RaspberrySharp.IO.GeneralPurpose;

namespace LedMatrixCSharp.Utils
{
    public class Scroller
    {
        public int Position { get; set; }
        public ProcessorPin UpPin { get; set; }
        public ProcessorPin DownPin { get; set; }
        public ProcessorPin LastPin { get; set; }
    }

    public class Controls
    {
        private static Controls controls;
        
        private static Dictionary<string, GpioConnection> Buttons = new Dictionary<string, GpioConnection>();
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
            var rot = ProcessorPin.Gpio38.Output();
            var gruen = ProcessorPin.Gpio31.Output();
            var blau = ProcessorPin.Gpio33.Output();

            gruen.Enable();
            blau.Disable();
            rot.Enable();
        }

        public void AddButton(string name, ProcessorPin pin)
        {
            var p = pin.Input();
            p.Resistor = PinResistor.PullDown;
            
            var gpioConnection = new GpioConnection(p);
            
            Buttons.Add(name, gpioConnection);
        }
        
        public void AddScroller(string name, ProcessorPin up, ProcessorPin down)
        {            
            var scroller = new Scroller()
            {
                UpPin = up,
                DownPin = down
            };

            Scrollers.Add(name, scroller);

            //Console.WriteLine("Registered Scroller " + name);
        }

        public void OnButtonClick(string name, EventHandler<PinStatusEventArgs> eventHandler)
        {
            var gpio = Buttons[name];
            
            gpio.Open();
            gpio.PinStatusChanged += eventHandler;          
        }

        public void ButtonUnsubscribe(string name, EventHandler<PinStatusEventArgs> eventHandler)
        {
            var gpio = Buttons[name];

            gpio.PinStatusChanged -= eventHandler;
        }

        public void OnScrollerScrolled(string name, Action<int, Scroller> action)
        {
            if (!Scrollers.ContainsKey(name)) return;
            var scroller = Scrollers[name];

            var gpioUp = scroller.UpPin.Input();
            var gpioDown = scroller.DownPin.Input();

            Action<ProcessorPin ,bool> pinChanged = (gpio, status) =>
            {
                if (gpio != scroller.LastPin)
                {
                    scroller.LastPin = gpio;

                    if (gpio == gpioUp.Pin && !status)
                    {
                            scroller.Position++;
                            action.Invoke(0, scroller);
                    }
                    else if (gpio == gpioDown.Pin && status)
                    {
                            scroller.Position--;
                            action.Invoke(1, scroller);
                    }
                }
            };
            
            gpioUp.StatusChangedAction = (b) => pinChanged.Invoke(gpioUp.Pin, b);
            gpioDown.StatusChangedAction = (b) => pinChanged.Invoke(gpioDown.Pin, b);
        }
    }
}