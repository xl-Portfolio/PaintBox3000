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

		public DrawableEllipse(Brush stroke, Brush? fill) : base(stroke)
		{
			_ellipse = new();
			_ellipse.Fill = fill;
			_ellipse.Width = 0;
			_ellipse.Height = 0;
			ApplyStrokeToVisual();
		}

	}
}
