using PaintBox3000.Enums;
using System.Windows.Media;

namespace PaintBox3000.Drawables
{
    public static class DrawableFactory
    {
        public static AbstractDrawable Create(ToolMode tool, Brush stroke, double strokeThickness, Brush? fill, BrushTip tip) => tool switch
        {
            ToolMode.Line => new DrawableLine(stroke, strokeThickness, tip),
            ToolMode.Ellipse => new DrawableEllipse(stroke, strokeThickness, fill),
            ToolMode.Rectangle => new DrawableRectangle(stroke, strokeThickness, fill),
            ToolMode.Freehand => new DrawableFreehand(stroke, strokeThickness, tip),
            _ => throw new NotImplementedException()
        };
    }
}
