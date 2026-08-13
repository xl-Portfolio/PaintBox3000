using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintBox3000
{
    class DrawableFreehand : Drawables
    {
		private readonly Polyline _polyline;
		public override Shape? Visual => _polyline;
		public DrawableFreehand(Brush stroke) : base(stroke)
		{
			_polyline = new();
			ApplyStrokeToVisual();
		}
	}
}
