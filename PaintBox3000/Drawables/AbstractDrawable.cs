using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintBox3000.Drawables
{
    public abstract class AbstractDrawable : IDrawable
    {
        private Brush? _stroke;
        private double _strokeThickness;
        private Point PointStart { get; set; }
        public abstract Shape? Visual { get; }

        protected AbstractDrawable(Brush stroke, double strokeThickness)
        {
            _stroke = stroke;
            _strokeThickness = strokeThickness;
        }
        protected void ApplyStrokeToVisual()
        {
            if (Visual != null)
            {
                Visual.Stroke = _stroke;
                Visual.StrokeThickness = _strokeThickness;
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