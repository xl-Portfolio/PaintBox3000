using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintBox3000.Drawables
{
    internal class DrawableEllipse : AbstractDrawable
    {
        public DrawableEllipse(Brush stroke, double strokeThickness, Brush? fill)
            : base(new Ellipse(), stroke, strokeThickness)
        {
            Visual.Fill = fill;
            Visual.Width = 0;
            Visual.Height = 0;
        }
    }
}