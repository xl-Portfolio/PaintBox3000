using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

using PaintBox3000.Enums;
using PaintBox3000.Helpers;

namespace PaintBox3000.Drawables
{
    internal class DrawableLine : AbstractDrawable
    {
        private readonly Line _line;

        public DrawableLine(Brush stroke, double strokeThickness, BrushTip tip)
            : base(new Line(), stroke, strokeThickness)
        {
            _line = (Line)Visual;
            if (Visual != null)
            {
                BrushTipHelper.Apply(Visual, tip);
            }
        }

        public override Point BottomRight => new(
            Math.Max(_line.X1, _line.X2),
            Math.Max(_line.Y1, _line.Y2));

        public override void SetStart(Point p)
        {
            _line.X1 = p.X;
            _line.X2 = p.X;
            _line.Y1 = p.Y;
            _line.Y2 = p.Y;
        }

        public override void SetSize(Point p)
        {
            _line.X2 = p.X;
            _line.Y2 = p.Y;
        }
    }
}