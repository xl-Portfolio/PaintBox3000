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
	public enum ShapeType { Ellipse, Rectangle, Line }
	public partial class MainWindow : Window
	{
		private Cursor cursor;
		private ShapeType? activeType;
		private DrawableShape? activeShape;
		private Brush? activeFill;
		private Brush? activeStroke;

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
			activeType = ShapeType.Line;
		}
		private void OnPaintEllipse(object sender, RoutedEventArgs e)
		{
			activeType = ShapeType.Ellipse;
		}
		private void OnPaintRectangle(object sender, RoutedEventArgs e)
		{
			activeType = ShapeType.Rectangle;
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
			if (activeType == null || activeStroke == null) return;
			this.Cursor = Cursors.Cross;
			activeShape = activeType switch
			{
				ShapeType.Line => new DrawableLine(activeStroke),
				ShapeType.Ellipse => new DrawableEllipse(activeStroke, activeFill),
				ShapeType.Rectangle => new DrawableRectangle(activeStroke, activeFill),
				_ => throw new NotImplementedException()
			};
			activeShape.OnPressed(e.GetPosition(Canvas).X, e.GetPosition(Canvas).Y);
			Canvas.Children.Add(activeShape.Visual);
		}
		private void OnMoved(object sender, MouseEventArgs e)
		{
			activeShape?.OnMoved(e.GetPosition(Canvas).X, e.GetPosition(Canvas).Y);
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
			return;
		}

		private void OnClickRedo(object sender, RoutedEventArgs e)
		{
			return;
		}
	}
}