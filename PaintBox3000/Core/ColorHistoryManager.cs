using System.Collections.ObjectModel;
using System.Windows.Media;

namespace PaintBox3000.Core
{/// <summary>
/// Keeps track of used colors
/// </summary>
    internal class ColorHistoryManager
    {
        public ObservableCollection<SolidColorBrush> StrokeHistory { get; } = new();
        public ObservableCollection<SolidColorBrush> FillHistory { get; } = new();

        public bool AddStroke(SolidColorBrush brush) => Add(StrokeHistory, brush);
        public bool AddFill(SolidColorBrush brush) => Add(FillHistory, brush);
        private static bool Add(ObservableCollection<SolidColorBrush> history, SolidColorBrush brush)
        {
            if (history.Any(b => b.Color == brush.Color)) return false;

            history.Insert(0, brush);
            if (history.Count > 10) history.RemoveAt(history.Count - 1);
            return true;
        }
    }
}
