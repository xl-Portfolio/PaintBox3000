using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintBox3000
{
	public abstract class Drawables : IDrawable
	{
		private Brush? stroke;
		private double strokeThickness = 3;
		private Point PointStart { get; set; }
		public abstract Shape? Visual { get; }

		protected Drawables(Brush stroke)
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
		public virtual void SetStart(Point p)
		{
			PointStart = p;
		}
		public virtual void SetSize(Point p)
		{
			Canvas.SetTop(Visual, Math.Min(PointStart.Y, p.Y));
			Canvas.SetLeft(Visual, Math.Min(PointStart.X, p.X));
			Visual.Width = Math.Abs(p.X - PointStart.X);
			Visual.Height = Math.Abs(p.Y - PointStart.Y);
		}
		public virtual Point BottomRight => new(
			Canvas.GetLeft(Visual) + (Visual?.Width ?? 0),
			Canvas.GetTop(Visual) + (Visual?.Height ?? 0)
		);
	}
}