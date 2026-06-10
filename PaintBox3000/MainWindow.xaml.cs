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
using System.Reflection;

namespace PaintBox3000
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public enum ToolMode { Ellipse, Rectangle, Line, Freehand }
	public partial class MainWindow : Window
	{
		private Cursor cursor;
		private ToolMode activeTool;
		private Drawables? activeShape;
		private SolidColorBrush activeFill;
		private SolidColorBrush activeStroke;

		private Stack<UIElement> _history = new();
		private Stack<UIElement>? _undoHistory = new();

		public MainWindow()
		{
			InitializeComponent();
			cursor = this.Cursor;
			InitializeSideBar();
			activeStroke = ToBrush((PropertyInfo)strokeColorList.SelectedItem);
			activeFill = ToBrush((PropertyInfo)fillColorList.SelectedItem);
			BtnLine.IsChecked = true;
			BtnLine.RaiseEvent(new RoutedEventArgs(RadioButton.ClickEvent));
		}
		private static void UpdateStatBar(Label label, ToolMode? tool) => label.Content = tool.ToString().ToLower();
		private static void UpdateStatBar(Label label, PropertyInfo pi) => label.Content = pi.Name.ToLower();
		private static SolidColorBrush ToBrush(PropertyInfo pi) => new((Color)pi.GetValue(null, null)!);

		private void InitializeSideBar()
		{
			PropertyInfo[] propertyInfosColor = [.. typeof(Colors).GetProperties()
				.OrderByDescending((currentColor) =>
				{
					Color c = (Color)currentColor.GetValue(null, null);
					return c.R + c.G + c.B;
				})];
			fillColorList.ItemsSource = propertyInfosColor;
			fillColorList.SelectedIndex = 0;
			strokeColorList.ItemsSource = propertyInfosColor;
			strokeColorList.SelectedIndex = propertyInfosColor.Length - 1;
		}
		private void OpenSideBar(ToolMode tool)
		{
			MainGrid.ColumnDefinitions[3].Width = new GridLength(250);
			//SideBar.Visibility = Visibility.Visible;
			SideBarHeader.Content = $"{tool.ToString().ToLower()} settings";
		}
		
		private void OnPaintLine(object sender, RoutedEventArgs e)
		{
			activeTool = ToolMode.Line;
			UpdateStatBar(LblSBTool, activeTool);
		}
		private void OnPaintEllipse(object sender, RoutedEventArgs e)
		{
			activeTool = ToolMode.Ellipse;
			UpdateStatBar(LblSBTool, activeTool);
		}
		private void OnPaintRectangle(object sender, RoutedEventArgs e)
		{
			activeTool = ToolMode.Rectangle;
			UpdateStatBar(LblSBTool, activeTool);
		}
		private void OnPaintFreehand(object sender, RoutedEventArgs e)
		{
			activeTool = ToolMode.Freehand;
			UpdateStatBar(LblSBTool, activeTool);
		}

		private void OnPressed(object sender, MouseButtonEventArgs e)
		{
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

		private void OnCloseSidebar(object sender, RoutedEventArgs e)
		{
			//SideBar.Visibility = Visibility.Collapsed;
			MainGrid.ColumnDefinitions[3].Width = new GridLength(0);
		}

		private void OnBrushSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			//if (xxx == null) return;
			return;
		}

		private void OnStrokeColorChanged(object sender, SelectionChangedEventArgs e)
		{
			if (strokeColorList.SelectedItem is not PropertyInfo pi) return;
			activeStroke = ToBrush(pi);
			UpdateStatBar(LblSBStrokeColor, pi);
		}
		private void OnFillColorChanged(object sender, SelectionChangedEventArgs e)
		{
			if (fillColorList.SelectedItem is not PropertyInfo pi) return;
			activeFill = ToBrush(pi);
			UpdateStatBar(LblSBFillColor, pi);
		}
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			Canvas.MinWidth = Canvas.ActualWidth;
			Canvas.MinHeight = Canvas.ActualHeight;
		}

		//private void OnSetColor(object sender, RoutedEventArgs e)
		//{
		//	OpenSideBar(activeTool);
		//}
	}
}