using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using PaintBox3000.Helpers;
using PaintBox3000.Enums;

namespace PaintBox3000.Drawables
{
    class DrawableFreehand : AbstractDrawable
    {
        private readonly Polyline _polyline;
        public override Shape? Visual => _polyline;
        public DrawableFreehand(Brush stroke, double strokeThickness, BrushTip tip) : base(stroke, strokeThickness)
        {
            _polyline = new();
            ApplyStrokeToVisual();
            if (Visual != null) BrushTipHelper.Apply(Visual, tip);
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

        public override Point BottomRight
        {
            get
            {
                if (_polyline.Points.Count == 0) return new Point(0, 0);
                return new Point(
                    _polyline.Points.Max(pt => pt.X),
                    _polyline.Points.Max(pt => pt.Y)
                );
            }
        }
    }
}
