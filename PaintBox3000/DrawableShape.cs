using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintBox3000
{
	public abstract class DrawableShape : IDrawable
	{
		protected Brush? stroke;
		protected double strokeThickness = 3;
		protected double x1 = 0d, x2 = 0d, y1 = 0d, y2 = 0d;
		public abstract Shape? Visual { get; }

		protected DrawableShape(Brush stroke)
		{
			this.stroke = stroke;
			this.strokeThickness = 3;
		}
		protected void ApplyStrokeToVisual()
		{
			if (Visual != null)
			{
				Visual.Stroke = stroke;
				Visual.StrokeThickness = strokeThickness;
			}
		}
		public virtual void OnPressed(double x1, double y1)
		{
			this.x1 = x1;
			this.y1 = y1;
		}
		public virtual void OnMoved(double x2, double y2)
		{
			Canvas.SetTop(Visual, y2 > y1 ? y1 : y2);
			Canvas.SetLeft(Visual, x2 > x1 ? x1 : x2);
			Visual.Width = Math.Abs(x2 - x1);
			Visual.Height = Math.Abs(y2 - y1);
		}

	}
}