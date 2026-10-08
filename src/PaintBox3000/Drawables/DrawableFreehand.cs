using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

using PaintBox3000.Enums;
using PaintBox3000.Helpers;

namespace PaintBox3000.Drawables
{
    internal class DrawableFreehand : AbstractDrawable
    {
        private readonly Polyline _polyline;

        public DrawableFreehand(Brush stroke, double strokeThickness, BrushTip tip)
            : base(new Polyline(), stroke, strokeThickness)
        {
            _polyline = (Polyline)Visual;
            if (Visual != null)
            {
                BrushTipHelper.Apply(Visual, tip);
            }
        }

        public override Point BottomRight
        {
            get
            {
                if (_polyline.Points.Count == 0)
                {
                    return new Point(0, 0);
                }

                return new Point(
                    _polyline.Points.Max(pt => pt.X),
                    _polyline.Points.Max(pt => pt.Y));
            }
        }

        public override void SetStart(Point p)
        {
            _polyline.Points.Clear();
            _polyline.Points.Add(p);
        }

        public override void SetSize(Point p)
        {
            _polyline.Points.Add(p);
        }
    }
}