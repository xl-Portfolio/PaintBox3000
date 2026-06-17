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
			SideBar.Visibility = Visibility.Visible;
			SideBarHeader.Content = $"{tool.ToString().ToLower()} settings";
		}
		
		private void OnPaintLine(object sender, RoutedEventArgs e)
		{
			BtnLine.IsChecked = true;
			activeTool = ToolMode.Line;
			OpenSideBar(activeTool);
			UpdateStatBar(LblSBTool, activeTool);
		}
		private void OnPaintEllipse(object sender, RoutedEventArgs e)
		{
			BtnEllipse.IsChecked = true;
			activeTool = ToolMode.Ellipse;
			OpenSideBar(activeTool);
			UpdateStatBar(LblSBTool, activeTool);
		}
		private void OnPaintRectangle(object sender, RoutedEventArgs e)
		{
			BtnRectangle.IsChecked = true;
			activeTool = ToolMode.Rectangle;
			OpenSideBar(activeTool);
			UpdateStatBar(LblSBTool, activeTool);
		}
		private void OnPaintFreehand(object sender, RoutedEventArgs e)
		{
			BtnFreehand.IsChecked = true;
			activeTool = ToolMode.Freehand;
			OpenSideBar(activeTool);
			UpdateStatBar(LblSBTool, activeTool);
		}

		private void OnPressed(object sender, MouseButtonEventArgs e)
		{
			actualCanvas.CaptureMouse();
			this.Cursor = Cursors.Cross;
			activeShape = activeTool switch
			{
				ToolMode.Line => new DrawableLine(activeStroke),
				ToolMode.Ellipse => new DrawableEllipse(activeStroke, activeFill),
				ToolMode.Rectangle => new DrawableRectangle(activeStroke, activeFill),
				ToolMode.Freehand => new DrawableFreehand(activeStroke),
				_ => throw new NotImplementedException()
			};
			activeShape.SetStart(e.GetPosition(actualCanvas));

			if (activeShape.Visual == null) return;
			actualCanvas.Children.Add(activeShape.Visual);
		}
		private void OnMoved(object sender, MouseEventArgs e)
		{
			activeShape?.SetSize(e.GetPosition(actualCanvas));
		}

		private void OnReleased(object sender, MouseButtonEventArgs e)
		{
			actualCanvas.ReleaseMouseCapture();
			this.Cursor = cursor;
			if (activeShape?.Visual != null)
			{
				_history.Push(activeShape.Visual);
				Point br = activeShape.BottomRight;
				if (br.X > actualCanvas.MinWidth) actualCanvas.MinWidth = br.X;
				if (br.Y > actualCanvas.MinHeight) actualCanvas.MinHeight = br.Y;
			}

			activeShape = null;
		}

		private void OnClickClear(object sender, RoutedEventArgs e)
		{
			actualCanvas.Children.Clear();
		}
		private void OnClickUndo(object sender, RoutedEventArgs e)
		{
			if (_history.Count == 0) return;
			var stackItem = _history.Pop();
			_undoHistory.Push(stackItem);
			actualCanvas.Children.Remove(stackItem);
			
		}
		private void OnClickRedo(object sender, RoutedEventArgs e)
		{
			if (_undoHistory.Count == 0) return;
				var stackItem = _undoHistory.Pop();
				_history.Push(stackItem);
				actualCanvas.Children.Add(stackItem);
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
			actualCanvas.MinWidth = actualCanvas.ActualWidth;
			actualCanvas.MinHeight = actualCanvas.ActualHeight;
		}
		private void OnCloseSidebar(object sender, RoutedEventArgs e)
		{
			SideBar.Visibility = Visibility.Collapsed;
		}
		private void OnSideBarVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (SideBar.Visibility == Visibility.Visible)
			{
				MainGrid.ColumnDefinitions[3].Width = new GridLength(250);
				Splitter.IsEnabled = true;
			}
			if (SideBar.Visibility == Visibility.Collapsed)
			{
				MainGrid.ColumnDefinitions[3].Width = new GridLength(0);
				Splitter.IsEnabled = false;
			}
			
		}

        //private void OnSetColor(object sender, RoutedEventArgs e)
        //{
        //	OpenSideBar(activeTool);
        //}
    }
}