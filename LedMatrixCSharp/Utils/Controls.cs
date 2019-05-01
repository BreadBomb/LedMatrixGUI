using Raspberry.IO.GeneralPurpose;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;

namespace LedMatrixCSharp.Utils
{
    public class Controls
    {
        private static Controls controls;
        
        private static Dictionary<string, InputPinConfiguration> Buttons = new Dictionary<string, InputPinConfiguration>();
        private static Dictionary<string, InputPinConfiguration[]> Scrollers = new Dictionary<string, InputPinConfiguration[]>();

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

        public Controls(){
        }

        public static void AddButton(string name, ConnectorPin pin)
        {
            var button = pin.Input();
            button.Resistor = PinResistor.PullUp;
            Buttons.Add(name, button);
        }
        
        public static void AddScroller(string name, string up, string down)
        {
            //Enum.TryParse(up, out ConnectorPin pinUp);
            //Enum.TryParse(up, out ConnectorPin pinDown);
            
            
            //var scrollUp = pinUp.Input();
            //var scrollDown = pinDown.Input();
            
            //Scrollers.Add(name, new InputPinConfiguration[] {scrollUp, scrollDown});

            //Console.WriteLine("Registered Scroller " + name);
        }

        public Action<bool> OnButtonClick(string name)
        {
            return Buttons[name].StatusChangedAction;
        }

        public static Action<bool>[] OnScrollerScrolled(string name)
        {
            return new Action<bool>[] { Scrollers[name][0].StatusChangedAction, Scrollers[name][1].StatusChangedAction };
        }
    }
}