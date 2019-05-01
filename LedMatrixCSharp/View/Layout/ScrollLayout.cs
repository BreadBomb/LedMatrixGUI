using LedMatrixCSharp.Utils;
using Raspberry.IO.GeneralPurpose;
using System;

namespace LedMatrixCSharp.View.Layout
{
    public class ScrollLayout: View
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
                this.Dimensions = new Dimensions(Child.Dimensions.Width + 1, Child.Dimensions.Height);
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
            var pin1 = ConnectorPin.P1Pin36.Input().PullUp();
            var pin2 = ConnectorPin.P1Pin38.Input().PullUp();

            //Create the settings for the connection
            var settings = new GpioConnectionSettings();

            //Interval between pin checks. This is *really* important - higher values lead to missed values/borking. Lower 
            //values are apparently possible, but may have 'severe' performance impact. Further testing needed.
            settings.PollInterval = TimeSpan.FromMilliseconds(1);

            //Create a new GpioConnection with the settings per above, and including pin1 (24) and pin2 (26).
            var connection = new GpioConnection(settings, pin1, pin2);
            //Create a new GpioConnection with the settings per above, and including pin1 (24) and pin2 (26).

            Console.WriteLine(connection);

            //Integer storing the number of detents turned - clockwise turns should increase this and vice versa.
            var encoderPos = 0;

            //Add an event handler to the connection. If either pin1 or pin2's value changes this will fire.
            connection.PinStatusChanged += (sender, eventArgs) =>
            {
                //If pin 24 / Pin08 / pin1 has changed value...
                if (eventArgs.Configuration.Pin == ProcessorPin.Pin16)
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
                    if ((eventArgs.Configuration.Pin == ProcessorPin.Pin16) && (!eventArgs.Enabled))
                    {
                        //If levB = 0
                        if (!levB && encoderPos < Child.Dimensions.Height-32)
                        {
                            //Encoder has turned 1 detent clockwise. Update the counter:
                            encoderPos++;
                            yOffset--;
                        }
                    }
                    //Else if pin 26 / Pin7 / pin2's value changed and its value is now 1...
                    else if ((eventArgs.Configuration.Pin == ProcessorPin.Pin20) && (eventArgs.Enabled))
                    {
                        //If levA = 1
                        if (levA && encoderPos > 0)
                        {
                            //Encoder has turned 1 detent anti-clockwise. Update the counter:
                            encoderPos--;
                            yOffset++;
                        }
                    }
                }
            };

        }
        
        public override void Draw()
        {
            if (Child.Dimensions.Height != Dimensions.Height)
            {
                this.Dimensions = new Dimensions(Child.Dimensions.Width + 1, Child.Dimensions.Height);
            }
            Canvas.Clear();
            Canvas.SetPixel(31, 0, CanvasColor.GRAY);
            Canvas.SetPixel(31, 31, CanvasColor.GRAY);
            for (int i = 0; i < (30d / (Dimensions.Height+32))*30d; i++)
            {
                Canvas.SetPixel(31, i+1+(int)Math.Round(((double)-yOffset / (double)(Child.Dimensions.Height)) * 32, 0), CanvasColor.WHITE);
            }

            Child?.Draw();
            if (Child != null) 
                Child.Position = new CanvasPosition(Child.Position.X, yOffset);

            base.Draw();
        }
    }
}