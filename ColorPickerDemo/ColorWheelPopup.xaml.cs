using System;
using ColorPickerDemo.TouchTracking;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ColorPickerDemo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorWheelPopup : PopupPage
    {

        private const int _shrinkage = 50;
        private SKColor[] _colors = new SKColor[8];

        private readonly SKPaint _touchCircleOutline = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.White.ToSKColor(),
            StrokeWidth = 5,
            IsAntialias = true
        };

        private readonly SKPaint _touchCircleFill = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        private readonly SKPaint _circlePalette = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        private readonly SKPaint _rectanglePalette = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        private SKPoint _touchLocation;
        private float _touchCircleRadius = 15;
        private SKColor _selectedColor = Color.Transparent.ToSKColor();

        private SKPoint _center;
        private float _radius;

        private bool _colorChanged;
        public event EventHandler<ColorChangedEventArgs> ColorChanged;

        public ColorWheelPopup()
        {
            InitializeComponent();

            for (int i = 0; i < _colors.Length; i++)
            {
                _colors[i] = SKColor.FromHsl(i * 360f / 7, 100, 50);
            }
        }

        async void OnClose(object sender, EventArgs e)
        {
            if (this.Parent.FindByName("WheelPickerButton") is Button btn)
            {
                btn.IsEnabled = true;
            }

            await Navigation.PopPopupAsync();
        }

        async void OnSelected(object sender, EventArgs e)
        {
            if (_colorChanged)
            {
                ColorChanged?.Invoke(this, new ColorChangedEventArgs(_selectedColor.ToFormsColor()));
                _colorChanged = false;
            }

            if (this.Parent.FindByName("WheelPickerButton") is Button btn)
            {
                btn.IsEnabled = true;
            }

            await Navigation.PopPopupAsync();
        }

        void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {

            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear();

            _center = new SKPoint(info.Rect.MidX, info.Rect.MidY);
            _radius = (Math.Min(info.Width, info.Height) - _shrinkage) / 2;
            _circlePalette.Shader = SKShader.CreateSweepGradient(_center, _colors, null);

            canvas.DrawCircle(_center, _radius, _circlePalette);

            var rectLeft = info.Rect.MidX - _radius;
            var rectTop = 0;
            var rectRight = rectLeft + _radius * 2;
            var rectBottom = rectTop + _shrinkage;
            var rect = new SKRect(rectLeft, rectTop, rectRight, rectBottom);

            var rectPaletteLeft = new SKPoint(rectLeft, _shrinkage / 2);
            var rectPaletteRight = new SKPoint(rectLeft + _radius * 2, _shrinkage / 2);

            _rectanglePalette.Shader = SKShader.CreateLinearGradient(rectPaletteLeft, rectPaletteRight, _colors, null, SKShaderTileMode.Clamp);

            canvas.DrawRect(rect, _rectanglePalette);

            //insure touch circle in the center of color wheel
            if (_touchLocation == SKPoint.Empty)
            {
                _touchLocation = _center;
                _colorChanged = true;
            }

            if (_colorChanged)
            {
                using (var bmp = new SKBitmap(info))
                {
                    IntPtr dstpixels = bmp.GetPixels();

                    var succeed = surface.ReadPixels(info, dstpixels, info.RowBytes, (int)_touchLocation.X, (int)_touchLocation.Y);
                    if (succeed)
                    {
                        _selectedColor = bmp.GetPixel(0, 0);
                        _touchCircleFill.Color = _selectedColor;
                    }
                }
            }

            canvas.DrawCircle(_touchLocation, _touchCircleRadius, _touchCircleOutline);
            canvas.DrawCircle(_touchLocation, _touchCircleRadius, _touchCircleFill);
        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            var skPoint = args.Location.ToPixelSKPoint(canvasView);
            if (skPoint.IsInsideCircle(_center, _radius))
            {
                _touchLocation = skPoint;

                if (args.Type == TouchActionType.Entered ||
                    args.Type == TouchActionType.Pressed ||
                    args.Type == TouchActionType.Moved ||
                    args.Type == TouchActionType.Released)
                {
                    _colorChanged = true;
                    canvasView.InvalidateSurface();
                }
            }
            else
            {
                _colorChanged = false;
            }
        }

    }
}