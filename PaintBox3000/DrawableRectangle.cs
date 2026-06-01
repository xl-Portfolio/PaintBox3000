using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Numerics;

namespace PaintBox3000
{
	public class DrawableRectangle : Drawables
	{
		private readonly Rectangle _rectangle;
		public override Shape? Visual => _rectangle;

		public DrawableRectangle(Brush stroke, Brush? fill) : base(stroke)
		{
			_rectangle = new();
			_rectangle.Fill = fill;
			_rectangle.Width = 0;
			_rectangle.Height = 0;
			ApplyStrokeToVisual();
		}

	}
}