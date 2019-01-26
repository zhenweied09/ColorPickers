using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;

namespace ColorPickerDemo
{
    public static class Extensions
    {
        public static SKPoint ToPixelSKPoint(this Point pt, SKCanvasView canvasView)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }

        public static bool IsInsideCircle(this SKPoint location, SKPoint center, float radius)
        {
            if (radius < 0) return false;

            var distance = Math.Sqrt(Math.Pow((location.X - center.X), 2f) +
                                        Math.Pow((location.Y - center.Y), 2f));
            return distance < radius;
        }
    }
}
