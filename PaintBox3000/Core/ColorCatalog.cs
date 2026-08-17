using System.Reflection;
using System.Windows.Media;

namespace PaintBox3000.Core
{
    /// <summary>
    /// provides colors and their cached properties
    /// </summary>
    internal class ColorCatalog
    {
        public PropertyInfo[] SortedColors { get; } = [.. typeof(Colors).GetProperties()
        .OrderByDescending(p =>
        {
            Color c = (Color)p.GetValue(null, null)!;
            return c.R + c.G + c.B;
        })];
        public SolidColorBrush GetFirstColor() => ToBrush(SortedColors[0]); 
        public SolidColorBrush GetLastColor() => ToBrush(SortedColors[^1]);
        public PropertyInfo? GetPropertyInfo(Color color) =>
            SortedColors.FirstOrDefault(p => (Color)p.GetValue(null, null)! == color);

        public SolidColorBrush ToBrush(PropertyInfo pi) => new((Color)pi.GetValue(null, null)!);
    }
}

