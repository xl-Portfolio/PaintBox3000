using System.Windows;

namespace PaintBox3000.Core
{
    /// <summary>
    /// provides undo / redo functionality by keeping track of drawn elements
    /// </summary>
    internal class CanvasHistoryManager
    {
        private readonly Stack<UIElement> _history = new();
        private readonly Stack<UIElement> _undoHistory = new();

        public void Push(UIElement element)
        {
            _history.Push(element);
        }

        public UIElement? Undo()
        {
            if (_history.Count == 0) return null;

            UIElement element = _history.Pop();
            _undoHistory.Push(element);
            return element;
        }

        public UIElement? Redo()
        {
            if (_undoHistory.Count == 0) return null;

            UIElement element = _undoHistory.Pop();
            _history.Push(element);
            return element;
        }

        public void ClearHistory()
        {
            _history.Clear();
        }
    }
}
