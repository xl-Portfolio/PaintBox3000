using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintBox3000.Drawables
{
    internal class DrawableRectangle : AbstractDrawable
    {
        public DrawableRectangle(Brush stroke, double strokeThickness, Brush? fill)
            : base(new Rectangle(), stroke, strokeThickness)
        {
            Visual.Fill = fill;
            Visual.Width = 0;
            Visual.Height = 0;
        }
    }
}