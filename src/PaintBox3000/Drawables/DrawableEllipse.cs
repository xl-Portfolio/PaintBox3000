using System.Windows.Media;
using System.Windows.Shapes;


namespace PaintBox3000.Drawables
{
    internal class DrawableEllipse : AbstractDrawable
    {
        private readonly Ellipse _ellipse;

        public DrawableEllipse(Brush stroke, double strokeThickness, Brush? fill)
            : base(stroke, strokeThickness)
        {
            _ellipse = new()
            {
                Fill = fill,
                Width = 0,
                Height = 0,
            };
            ApplyStrokeToVisual();
        }

        public override Shape? Visual => _ellipse;
    }
}