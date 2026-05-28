using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Numerics;

namespace PaintBox3000
{
	public class DrawableRectangle : DrawableShape
	{
		private readonly Rectangle _rectangle;
		public override Shape? Visual => _rectangle;

		public DrawableRectangle(Canvas canvas, Brush stroke, Brush? fill) : base(canvas, stroke) 
		{
			_rectangle = new();
			_rectangle.Fill = fill;
			_rectangle.Width = 0;
			_rectangle.Height = 0;
		}
		public override void OnPressed(double x1, double y1)
		{
			this.x1 = x1;
			this.y1 = y1;
		}
		public override void OnMoved(double x2, double y2)
		{
			Canvas.SetTop(_rectangle, y2 > y1 ? y1 : y2);
			Canvas.SetLeft(_rectangle, x2 > x1 ? x1 : x2);
			_rectangle.Width = Math.Abs(x2 - x1);
			_rectangle.Height = Math.Abs(y2 - y1);

		}

	}
}
