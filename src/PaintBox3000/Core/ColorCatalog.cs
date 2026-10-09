using System.Reflection;
using System.Windows.Media;

namespace PaintBox3000.Core
{
    /// <summary>
    /// provides colors and their cached properties.
    /// </summary>
    internal class ColorCatalog
    {
        public PropertyInfo[] SortedColors { get; } = [.. typeof(Colors).GetProperties()
        .OrderByDescending(p =>
        {
            Color c = ToColor(p);
            return c.R + c.G + c.B;
        })];

        public static SolidColorBrush ToBrush(PropertyInfo p) => new(ToColor(p));

        public PropertyInfo? GetPropertyInfo(Color color) =>
            SortedColors.FirstOrDefault(p => ToColor(p) == color);

        private static Color ToColor(PropertyInfo p)
        {
            return (Color)p.GetValue(null, null)!;
        }
    }
}