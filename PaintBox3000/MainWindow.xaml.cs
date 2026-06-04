using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaintBox3000
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public enum ToolMode { Ellipse, Rectangle, Line, Freehand }
	public partial class MainWindow : Window
	{
		private Cursor cursor;
		private ToolMode? activeTool;
		private Drawables? activeShape;
		private SolidColorBrush? activeFill;
		private SolidColorBrush? activeStroke;

		private Stack<UIElement> _history = new();
		private Stack<UIElement>? _undoHistory = new();

		public MainWindow()
		{
			InitializeComponent();
			cursor = this.Cursor;
			activeShape = null;
		}
		private void UpdateSB(Label label, ToolMode? tool) => label.Content = tool.ToString();
		private void UpdateSB(Border border, SolidColorBrush brush)
		{
			border.Background = brush;
			((TextBlock)border.Child).Foreground = brush;
		}

		private void OnPaintLine(object sender, RoutedEventArgs e)
		{
			activeTool = ToolMode.Line;
			UpdateSB(LblSBTool, activeTool); //
		}
		private void OnPaintEllipse(object sender, RoutedEventArgs e)
		{
			activeTool = ToolMode.Ellipse;
			UpdateSB(LblSBTool, activeTool);
		}
		private void OnPaintRectangle(object sender, RoutedEventArgs e)
		{
			activeTool = ToolMode.Rectangle;
			UpdateSB(LblSBTool, activeTool);
		}
		private void OnPaintFreehand(object sender, RoutedEventArgs e)
		{
			activeTool = ToolMode.Freehand;
			UpdateSB(LblSBTool, activeTool);
		}
		private void OnStrokeColorChanged(object sender, RoutedEventArgs e)
		{
			activeStroke = (SolidColorBrush)((Button)sender).Background;
			UpdateSB(LblSBStrokeColor, activeStroke);
		}

		private void OnFillColorChanged(object sender, RoutedEventArgs e)
		{
			activeFill = (SolidColorBrush?)((Button)sender).Background;
			UpdateSB(LblSBFillColor, activeFill);
		}


		private void OnPressed(object sender, MouseButtonEventArgs e)
		{
			if (activeTool == null || activeStroke == null) return;
			this.Cursor = Cursors.Cross;
			activeShape = activeTool switch
			{
				ToolMode.Line => new DrawableLine(activeStroke),
				ToolMode.Ellipse => new DrawableEllipse(activeStroke, activeFill),
				ToolMode.Rectangle => new DrawableRectangle(activeStroke, activeFill),
				ToolMode.Freehand => new DrawableFreehand(activeStroke),
				_ => throw new NotImplementedException()
			};
			activeShape.SetStart(e.GetPosition(Canvas));

			if (activeShape.Visual == null) return;
			Canvas.Children.Add(activeShape.Visual);
			_history.Push(activeShape.Visual);
		}
		private void OnMoved(object sender, MouseEventArgs e)
		{
			activeShape?.SetSize(e.GetPosition(Canvas));
		}

		private void OnReleased(object sender, MouseButtonEventArgs e)
		{
			this.Cursor = cursor;
			activeShape = null;
		}

		private void OnClickClear(object sender, RoutedEventArgs e)
		{
			Canvas.Children.Clear();
		}

		private void OnClickUndo(object sender, RoutedEventArgs e)
		{
			if (_history.Count == 0) return;
			var stackItem = _history.Pop();
			_undoHistory.Push(stackItem);
			Canvas.Children.Remove(stackItem);
			
		}

		private void OnClickRedo(object sender, RoutedEventArgs e)
		{
			if (_undoHistory.Count == 0) return;
				var stackItem = _undoHistory.Pop();
				_history.Push(stackItem);
				Canvas.Children.Add(stackItem);
		}


	}
}