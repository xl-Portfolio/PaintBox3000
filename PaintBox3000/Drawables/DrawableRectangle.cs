using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintBox3000.Drawables
{
    public class DrawableRectangle : AbstractDrawable
    {
        private readonly Rectangle _rectangle;
        public override Shape? Visual => _rectangle;

        public DrawableRectangle(Brush stroke, double strokeThickness, Brush? fill) : base(stroke, strokeThickness)
        {
            _rectangle = new();
            _rectangle.Fill = fill;
            _rectangle.Width = 0;
            _rectangle.Height = 0;
            ApplyStrokeToVisual();
        }

    }
}