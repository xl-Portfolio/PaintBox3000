using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using PaintBox3000.Helpers;
using PaintBox3000.Enums;

namespace PaintBox3000.Drawables
{
    internal class DrawableLine : AbstractDrawable
    {
        private readonly Line _line;
        public override Shape? Visual => _line;

        public DrawableLine(Brush stroke, double strokeThickness, BrushTip tip) : base(stroke, strokeThickness)
        {
            _line = new();
            ApplyStrokeToVisual();
            if (Visual != null) BrushTipHelper.Apply(Visual, tip);
        }
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
        public override Point BottomRight => new(
            Math.Max(_line.X1, _line.X2),
            Math.Max(_line.Y1, _line.Y2)
        );

    }
}
