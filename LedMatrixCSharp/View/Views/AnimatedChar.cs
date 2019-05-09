using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Threading;
using BrainbeanApps.ValueAnimation;
using LedMatrixCSharp.Utils;
using SixLabors.Fonts;

namespace LedMatrixCSharp.View.Views
{
    public class AnimatedChar : View
    {
        private Stopwatch animationStopwatch;
        private ValueAnimator<int> animation;
        private BDFFont font;
        private char c;
        private char new_c;
        private Timer animationTimer;

        public AnimatedChar(char c, BDFFont font)
        {
            this.c = c;
            this.font = font;
            animationStopwatch = new Stopwatch();
            Width = font.getCharacterWidth(0);
            Height = font.Height;
            animation = new ValueAnimator<int>()
            {
                InitialValue = 0,
                DeltaValue = Height,
                Animation = ValueAnimations.Linear<int>(),
                Duration = 1.0f,
            };
            animation.CompletedEvent += (sender, args) =>
            {
                this.c = new_c;  
                animationStopwatch.Stop();
                animationStopwatch.Reset();
                animationTimer.Dispose();
                animation.Reset();
            };
            InitialDraw();
        }

        public void ChangeChar(char c)
        {
            if (this.new_c == c) return;
            this.new_c = c;
            animationStopwatch.Start();
            animationTimer = new Timer(UpdateAnimation, null, 0, 60);
        }

        public void UpdateAnimation(object state)
        {
            float deltaTime = animationStopwatch.ElapsedMilliseconds / 1000.0f;
            animation.Process(deltaTime);
            RedrawLabel();
        }

        private void InitialDraw()
        {
            font.DrawGlyph(this, 0, 0, CanvasColor.WHITE, c);
        }

        public void RedrawLabel()
        {
            if (animation.CurrentValue != null)
            {
                Clear();
                int value = animation.CurrentValue.Value;
                font.DrawGlyph(this, 0, 0 - value, CanvasColor.WHITE, c);
                font.DrawGlyph(this, 0, Height - value, CanvasColor.WHITE, new_c);
            }
        }
    }
}