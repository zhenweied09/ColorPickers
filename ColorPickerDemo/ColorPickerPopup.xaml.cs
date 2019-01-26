using System;
using System.Collections.Generic;
using ColorPickerDemo.TouchTracking;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ColorPickerDemo {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ColorPickerPopup : PopupPage {

	    static readonly List<ColorPick> ColorPicks;
	    static bool _colorPicksInitialized;
	    static ColorPick _pickedColor;

	    const int ColorsPerRow = 5;
	    const int CanvasPadding = 5;
	    bool _colorChanged;

	    readonly SKPaint _clrPickPaint = new SKPaint {
	        Style = SKPaintStyle.Fill,
	        IsAntialias = true
	    };

	    readonly SKPaint _pickedClrPaint = new SKPaint {
	        Style = SKPaintStyle.Stroke,
            StrokeWidth = 5,
            IsAntialias = true,
	    };

        static ColorPickerPopup() {
	        ColorPicks = new List<ColorPick> {

	            new ColorPick("#25c5db"),
	            new ColorPick("#0098a6"),
	            new ColorPick("#0e47a1"),
	            new ColorPick("#1665c1"),
	            new ColorPick("#039be6"),

	            new ColorPick("#64b5f6"),
	            new ColorPick("#ff7000"),
	            new ColorPick("#ff9f00"),
	            new ColorPick("#ffb200"),
	            new ColorPick("#cf9702"),

	            new ColorPick("#8c6e63"),
	            new ColorPick("#6e4c42"),
	            new ColorPick("#d52f31"),
	            new ColorPick("#ff1643"),
	            new ColorPick("#f44236"),

	            new ColorPick("#ec407a"),
	            new ColorPick("#ad1457"),
	            new ColorPick("#6a1b9a"),
	            new ColorPick("#ab48bf"),
	            new ColorPick("#b968c7"),

	            new ColorPick("#00695b"),
	            new ColorPick("#00887a"),
	            new ColorPick("#4cb6ac"),
	            new ColorPick("#307c32"),
	            new ColorPick("#43a047"),

	            new ColorPick("#64dd16"),
	            new ColorPick("#222222"),
	            new ColorPick("#5f7c8c"),
	            new ColorPick("#b1bec6"),
	            new ColorPick("#465a65"),

	            new ColorPick("#607d8d"),
	            new ColorPick("#91a5ae"),
	        };
	    }


        public ColorPickerPopup () {
			InitializeComponent ();
		}

	    public event EventHandler<ColorChangedEventArgs> ColorChanged;


        async void OnClose(object sender, EventArgs e)
        {
            if (this.Parent.FindByName("CirclePickerButton") is Button btn)
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

            if (!_colorPicksInitialized)
            {
                InitializeColorPicks(info.Width);
            }

            // draw the colors
            foreach (var cp in ColorPicks)
            {
                _clrPickPaint.Color = cp.Color.ToSKColor();
                canvas.DrawCircle(cp.Position.X, cp.Position.Y, cp.Radius, _clrPickPaint);
            }

            // check if there is a selected color
            if (_pickedColor == null) { return; }

            // draw the highlight for the picked color
            _pickedClrPaint.Color = _pickedColor.Color.ToSKColor();
            canvas.DrawCircle(_pickedColor.Position.X, _pickedColor.Position.Y, _pickedColor.Radius + 10, _pickedClrPaint);
        }

        static void InitializeColorPicks(int skImageWidth) {

	        var contentWidth = skImageWidth - (CanvasPadding * 2);
	        var colorWidth = contentWidth / ColorsPerRow;

	        var sp = new SKPoint((colorWidth / 2) + CanvasPadding, (colorWidth / 2) + CanvasPadding);
	        var col = 1;
	        var row = 1;
	        var radius = (colorWidth / 2) - 10;

	        foreach (var cp in ColorPicks) {

	            if (col > ColorsPerRow) {
	                col = 1;
	                row += 1;
	            }

	            // calc current position
	            var x = sp.X + (colorWidth * (col - 1));
	            var y = sp.Y + (colorWidth * (row - 1));

	            cp.Radius = radius;
	            cp.Position = new SKPoint(x, y);
	            col += 1;
	        }

	        _colorPicksInitialized = true;
	    }

	    private async void OnTouchEffectAction(object sender, TouchActionEventArgs args) {

            if (args.Type == TouchActionType.Released)
            {
                var pnt = args.Location.ToPixelSKPoint(canvasView);

                // loop through all colors
                foreach (var cp in ColorPicks)
                {
                    // check if selecting a color
                    if (cp.IsTouched(pnt))
                    {
                        _colorChanged = true;
                        _pickedColor = cp;
                        break; // get out of loop
                    }
                }

                canvasView.InvalidateSurface();

                if (_colorChanged)
                {
                    ColorChanged?.Invoke(this, new ColorChangedEventArgs(_pickedColor.Color));
                    _colorChanged = false;
                    await Navigation.PopPopupAsync();
                }
            }
	    }

    }
}