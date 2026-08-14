using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Numerics;

namespace PaintBox3000
{
	public class DrawableEllipse : Drawables
	{
		private readonly Ellipse _ellipse;
		public override Shape? Visual => _ellipse;

		public DrawableEllipse(Brush stroke, double strokeThickness, Brush? fill) : base(stroke, strokeThickness)
		{
			_ellipse = new()
			{
				Fill = fill,
				Width = 0,
				Height = 0
			};
			ApplyStrokeToVisual();
		}

	}
}
