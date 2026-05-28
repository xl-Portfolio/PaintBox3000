using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Numerics;

namespace PaintBox3000
{
	public class DrawableEllipse : DrawableShape
	{
		private readonly Ellipse _ellipse;
		public override Shape? Visual => _ellipse;

		public DrawableEllipse(Canvas canvas, Brush stroke, Brush? fill) : base(canvas, stroke) 
		{
			_ellipse = new();
			_ellipse.Fill = fill;
			_ellipse.Width = 0;
			_ellipse.Height = 0;
		}
		public override void OnPressed(double x1, double y1)
		{
			this.x1 = x1;
			this.y1 = y1;
		}
		public override void OnMoved(double x2, double y2)
		{
			Canvas.SetTop(_ellipse, y2 > y1 ? y1 : y2);
			Canvas.SetLeft(_ellipse, x2 > x1 ? x1 : x2);
			_ellipse.Width = Math.Abs(x2 - x1);
			_ellipse.Height = Math.Abs(y2 - y1);

		}

	}
}
