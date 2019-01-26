using System;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace ColorPickerDemo
{
    public partial class MainPage : ContentPage
    {
        readonly ColorPickerPopup _circlePickerPopup;
        readonly ColorWheelPopup _wheelPickerPopup;

        public MainPage()
        {
            InitializeComponent();

            _circlePickerPopup = new ColorPickerPopup();
            _circlePickerPopup.ColorChanged += CircleColorPickerOnColorChanged;

            _wheelPickerPopup = new ColorWheelPopup();
            _wheelPickerPopup.ColorChanged += WheelColorPickerOnColorChanged;
        }

         async void OnCirclePickerClicked(object sender, EventArgs e)
        {
            CirclePickerButton.IsEnabled = false;
            await Navigation.PushPopupAsync(_circlePickerPopup);
        }

        async void OnWheelPickerClicked(object sender, EventArgs e)
        {
            WheelPickerButton.IsEnabled = false;
            await Navigation.PushPopupAsync(_wheelPickerPopup);
        }

        void CircleColorPickerOnColorChanged(object sender, ColorChangedEventArgs args)
        {
            BoxView.Color = args.Color;
            _circlePickerPopup.IsEnabled = true;
        }

        void WheelColorPickerOnColorChanged(object sender, ColorChangedEventArgs args)
        {
            BoxView.Color = args.Color;
            _wheelPickerPopup.IsEnabled = true;
        }
    }
}
