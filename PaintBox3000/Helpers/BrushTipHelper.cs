using System.Windows.Media;
using System.Windows.Shapes;
using PaintBox3000.Enums;

namespace PaintBox3000.Helpers
{
    internal static class BrushTipHelper
    {
        public static void Apply(Shape shape, BrushTip tip)
        {
            bool round = tip == BrushTip.Round;
            shape.StrokeLineJoin = round ? PenLineJoin.Round : PenLineJoin.Miter;
            shape.StrokeStartLineCap = round ? PenLineCap.Round : PenLineCap.Square;
            shape.StrokeEndLineCap = round ? PenLineCap.Round : PenLineCap.Square;
        }
    }
}
