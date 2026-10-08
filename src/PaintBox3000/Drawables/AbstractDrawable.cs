using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintBox3000.Drawables
{
    public abstract class AbstractDrawable : IDrawable
    {
        protected AbstractDrawable(Shape visual, Brush stroke, double strokeThickness)
        {
            Visual = visual;
            Visual.Stroke = stroke;
            Visual.StrokeThickness = strokeThickness;
        }

        public Shape Visual { get; }

        public virtual Point BottomRight => new(
            Canvas.GetLeft(Visual) + (Visual?.Width ?? 0),
            Canvas.GetTop(Visual) + (Visual?.Height ?? 0));

        private Point PointStart { get; set; }

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
    }
}