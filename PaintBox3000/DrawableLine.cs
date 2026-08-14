using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintBox3000
{
	internal class DrawableLine : Drawables
	{
		private readonly Line _line;
		public override Shape? Visual => _line;
		public DrawableLine(Brush stroke, double strokeThickness) : base(stroke, strokeThickness)
		{
			_line = new();
			ApplyStrokeToVisual();

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
