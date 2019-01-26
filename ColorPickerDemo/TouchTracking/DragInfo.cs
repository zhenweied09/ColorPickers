using Xamarin.Forms;

namespace ColorPickerDemo.TouchTracking
{

    internal class DragInfo
    {

        public DragInfo(long id, Point pressPoint)
        {
            Id = id;
            PressPoint = pressPoint;
        }

        public long Id { private set; get; }
        public Point PressPoint { private set; get; }
    }
}