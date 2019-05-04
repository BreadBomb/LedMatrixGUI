using LedMatrixCSharp.Utils;
using Raspberry.IO.GeneralPurpose;
using System;

namespace LedMatrixCSharp.View.Layout
{
    public class ScrollLayout: Panel
    {
        private View child;
        private int yOffset;
        
        public string ScrollerName { get; set; }
        
        public View Child
        {
            get { return child; }
            set
            {
                child = value;
                if (Width >= 32)
                {
                    child.Width = 31;
                }

                Width = 32;
                Height = child.Height;
            }
        }

        //State of Pin08
        private static bool levA = false;
        //State of Pin7
        private static bool levB = false;
        //The name of the last GPIO pin to fire a PinStatusChanged event
        private static string lastGpio = String.Empty;

        public ScrollLayout(string scroller)
        {
            this.ScrollerName = scroller;

            //Declare our pins (connector 24 and 26 / processor 08 and 7) as INPUT pins, and apply pull-up resistors
            var pin1 = ConnectorPin.P1Pin35.Input().PullDown();
            var pin2 = ConnectorPin.P1Pin37.Input().PullDown();

            var ledBlue = ConnectorPin.P1Pin31.Output();
            var ledGreen = ConnectorPin.P1Pin33.Output();
            var ledRed = ConnectorPin.P1Pin38.Output();
            
            var ledConnection = new GpioConnection(ledRed, ledBlue, ledGreen);

            ledBlue.Enabled = true;
            ledGreen.Enabled = true;                 
            
            var button = ConnectorPin.P1Pin32.Input().PullDown();
            
            var buttonConnection = new GpioConnection(button);

            buttonConnection.PinStatusChanged += (sender, args) =>
            {
                if (args.Enabled)
                {
                    Console.WriteLine("BUTTON Above");
                }
            };
                        
            //Create the settings for the connection
            var settings = new GpioConnectionSettings();

            //Interval between pin checks. This is *really* important - higher values lead to missed values/borking. Lower 
            //values are apparently possible, but may have 'severe' performance impact. Further testing needed.
            settings.PollInterval = TimeSpan.FromMilliseconds(1);

            //Create a new GpioConnection with the settings per above, and including pin1 (24) and pin2 (26).
            var connection = new GpioConnection(settings, pin1, pin2);
            //Create a new GpioConnection with the settings per above, and including pin1 (24) and pin2 (26).

            //Integer storing the number of detents turned - clockwise turns should increase this and vice versa.
            var encoderPos = 0;

            //Add an event handler to the connection. If either pin1 or pin2's value changes this will fire.
            connection.PinStatusChanged += (sender, eventArgs) =>
            {
                //If pin 24 / Pin08 / pin1 has changed value...
                if (eventArgs.Configuration.Pin == ProcessorPin.Pin19)
                {
                    //Set levA to this pin's value
                    levA = eventArgs.Enabled;
                }
                //If any other pin (i.e. pin 26 / Pin7 / pin2) has changed value...
                else
                {
                    //Set levB to this pin's value
                    levB = eventArgs.Enabled;
                }

                //If the pin whose value changed is different to the *last* pin whose value changed...
                if (eventArgs.Configuration.Pin.ToString() != lastGpio)
                {
                    //Update the last changed pin
                    lastGpio = eventArgs.Configuration.Pin.ToString();

                    //If pin 24 / Pin08 / pin1's value changed and its value is now 0...
                    if ((eventArgs.Configuration.Pin == ProcessorPin.Pin19) && (!eventArgs.Enabled))
                    {
                        //If levB = 0
                        if (!levB && encoderPos < Height - 32)
                        {
                            //Encoder has turned 1 detent clockwise. Update the counter:
                            encoderPos++;
                            yOffset--;
                            Console.WriteLine("DOWN");
                        }
                    }
                    //Else if pin 26 / Pin7 / pin2's value changed and its value is now 1...
                    else if ((eventArgs.Configuration.Pin == ProcessorPin.Pin26) && (eventArgs.Enabled))
                    {
                        //If levA = 1
                        if (levA && encoderPos > 0)
                        {
                            //Encoder has turned 1 detent anti-clockwise. Update the counter:
                            encoderPos--;
                            yOffset++;
                            Console.WriteLine("UP");
                        }
                    }
                }
            };

        }
        
        public override void Update()
        {
            Clear();
            SetPixel(31, 0, CanvasColor.YELLOW);
            SetPixel(31, 31, CanvasColor.YELLOW);
            for (int i = 0; i < (30d / Height)*30d+1; i++)
            {
                SetPixel(31, i+1+(int)Math.Round(((double)-yOffset / (double)(Height)) * 32, 0), CanvasColor.WHITE);
            }

            if (Child != null)
            {
                Child.Y  = yOffset;
            }

            Child?.Update();
            
            Concat(Child);

            base.Update();
        }
    }
}