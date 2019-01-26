using System;
using SkiaSharp;
using Xamarin.Forms;

namespace ColorPickerDemo
{
    public class ColorPick
    {

        public ColorPick(string hex)
        {
            Color = Color.FromHex(hex);
        }

        public Color Color { get; set; }
        public int Radius { get; set; }
        public SKPoint Position { get; set; }

        public bool IsTouched(SKPoint pt)
        {

            return (Math.Pow(pt.X - Position.X, 2) / Math.Pow(Radius, 2) +
                    Math.Pow(pt.Y - Position.Y, 2) / Math.Pow(Radius, 2)) < 1;
        }
    }
}