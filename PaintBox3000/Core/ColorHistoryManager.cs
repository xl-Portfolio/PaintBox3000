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

        public void AddStroke(SolidColorBrush brush) => Add(StrokeHistory, brush);
        public void AddFill(SolidColorBrush brush) => Add(FillHistory, brush);  
        private static void Add(ObservableCollection<SolidColorBrush> history, SolidColorBrush brush)
        {
            int index = history
                .Select((item, index) => new { item, index })
                .FirstOrDefault(x => x.item.Color == brush.Color)?.index ?? -1;

            if (index >= 0)
                history.Move(index, 0);
            else
                history.Insert(0, brush);

            if (history.Count > 10)
                history.RemoveAt(history.Count - 1);
        }
    }
}
