
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.IO;
using LedMatrixCSharp.View.Layout;

namespace LedMatrixCSharp.View.Views
{
    public class ImageSequence: Panel
    {
        private class ImageSequenceConfig
        {
            public int Speed { get; set; }
            public int Count { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        Timer timer;
        Image image;

        List<string> images = new List<string>();
        ImageSequenceConfig CurrentImageSequenceConfig;

        public int CurrentIndex { get; private set; }

        public ImageSequence(string folderPath)
        {
            CurrentImageSequenceConfig = GetConfig(Path.Join(folderPath, "animation.config"));
            Width = CurrentImageSequenceConfig.Width;
            Height = CurrentImageSequenceConfig.Height;

            GetImages(folderPath);

            timer = new Timer();
            timer.Interval = 1000/ CurrentImageSequenceConfig.Speed;
            timer.Elapsed += ChangeImage;
            timer.Start();
        }

        private void ChangeImage(object sender, ElapsedEventArgs e)
        {
            CurrentIndex++;
            if (CurrentIndex == CurrentImageSequenceConfig.Count) CurrentIndex = 0;
            image = new Image(images[CurrentIndex]);
            image.Update();
            Concat(image);
        }

        public override void Update()
        {
            base.Update();
        }

        private void GetImages(string path)
        {
            for (int i = 0; i < CurrentImageSequenceConfig.Count; i++)
            {
                if (File.Exists(Path.Join(path, i + ".png")))
                {
                    images.Add(Path.Join(path, i + ".png"));
                }
            }
        }

        private ImageSequenceConfig GetConfig(string path)
        {
            var config = new ImageSequenceConfig();
            var file = File.OpenText(path);

            while (!file.EndOfStream)
            {
                var line = file.ReadLine();
                if (line.Trim().StartsWith("#")) continue;
                if (line.Trim().ToLower().StartsWith("speed"))
                {
                    var value = line.Trim().Split('=')[1];
                    config.Speed = int.Parse(value);
                }
                if (line.Trim().ToLower().StartsWith("count"))
                {
                    var value = line.Trim().Split('=')[1];
                    config.Count = int.Parse(value);
                }
                if (line.Trim().ToLower().StartsWith("width"))
                {
                    var value = line.Trim().Split('=')[1];
                    config.Width = int.Parse(value);
                }
                if (line.Trim().ToLower().StartsWith("height"))
                {
                    var value = line.Trim().Split('=')[1];
                    config.Height = int.Parse(value);
                }
            }

            return config;
        }
    }
}
