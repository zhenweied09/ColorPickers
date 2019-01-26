﻿using Xamarin.Forms;

namespace ColorPickerDemo.TouchTracking
{

    public class TouchEffect : RoutingEffect
    {
        public event TouchActionEventHandler TouchAction;

        public TouchEffect() : base("ColorPickerDemo.TouchEffect") { }

        public bool Capture { set; get; }

        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {

            TouchAction?.Invoke(element, args);
        }
    }
}