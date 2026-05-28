using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintBox3000
{
	internal class DrawableLine : DrawableShape
	{
		private readonly Line _line;
		public DrawableLine(Canvas canvas, Brush stroke) : base(canvas, stroke)
		{
			_line = new();
		}

		public override Shape? Visual => _line;

		public override void OnPressed(double x1, double y1)
		{
			_line.X1 = x1;
			_line.Y1 = y1;
		}

		public override void OnMoved(double x2, double y2)
		{
			_line.X2 = x2;
			_line.Y2 = y2;
		}


	}
}
