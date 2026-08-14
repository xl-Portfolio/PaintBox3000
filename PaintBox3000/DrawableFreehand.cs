using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace PaintBox3000
{
    class DrawableFreehand : Drawables
    {
		private readonly Polyline _polyline;
		public override Shape? Visual => _polyline;
		public DrawableFreehand(Brush stroke, double strokeThickness) : base(stroke, strokeThickness)
		{
			_polyline = new();
			ApplyStrokeToVisual();
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
