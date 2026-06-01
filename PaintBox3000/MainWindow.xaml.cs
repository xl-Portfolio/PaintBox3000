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
		private Brush? activeFill;
		private Brush? activeStroke;

		private Stack<UIElement> _history = new();

		public MainWindow()
		{
			InitializeComponent();
			cursor = this.Cursor;
			//activeType = null;
			////activeStroke = null;
			//activeFill = null;
		}
		private void OnPaintLine(object sender, RoutedEventArgs e)
		{
			activeTool = ToolMode.Line;
		}
		private void OnPaintEllipse(object sender, RoutedEventArgs e)
		{
			activeTool = ToolMode.Ellipse;
		}
		private void OnPaintRectangle(object sender, RoutedEventArgs e)
		{
			activeTool = ToolMode.Rectangle;
		}
		private void OnPaintFreehand(object sender, RoutedEventArgs e)
		{
			activeTool = ToolMode.Freehand;
		}
		private void OnStrokeColorChanged(object sender, RoutedEventArgs e)
		{
			activeStroke = ((Button)sender).Background;
		}

		private void OnFillColorChanged(object sender, RoutedEventArgs e)
		{
			activeFill = ((Button)sender).Background;
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
			if (_history.Count < 0) return;
			Canvas.Children.Remove(_history.Pop());
		}

		private void OnClickRedo(object sender, RoutedEventArgs e)
		{
			return;
		}


	}
}