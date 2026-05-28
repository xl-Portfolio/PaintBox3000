using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintBox3000
{
	public abstract class DrawableShape : IDrawable
	{
		protected Canvas canvas;
		protected Brush? stroke;
		protected double strokeThickness;
		protected double x1 = 0d, x2 = 0d, y1 = 0d, y2 = 0d;
		public abstract Shape? Visual { get; }
		//protected double strokeThickness;

		protected DrawableShape(Canvas canvas, Brush stroke)
		{
			this.canvas = canvas;
			this.stroke = stroke;
			this.strokeThickness = 3;
		}
		public abstract void OnPressed(double x1, double y1);
		public abstract void OnMoved(double x2, double y2);

		//public abstract void OnReleased();
	}
}