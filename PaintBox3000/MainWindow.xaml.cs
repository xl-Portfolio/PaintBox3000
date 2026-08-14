using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Reflection;
using Microsoft.Win32;
using PaintBox3000.Enums;
using PaintBox3000.Drawables;
using System.Collections.ObjectModel;

namespace PaintBox3000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
		private Cursor cursor;
		private ToolMode activeTool;
		private AbstractDrawable? activeShape;
		private SolidColorBrush activeFill;
		private SolidColorBrush activeStroke;
		private double activeBrushSize;
		private BrushTip activeBrushTip;

		private Stack<UIElement> _history = new();
		private Stack<UIElement>? _undoHistory = new();

        private ObservableCollection<SolidColorBrush> strokeColorHistory = new();
        private ObservableCollection<SolidColorBrush> fillColorHistory = new();

        public MainWindow()
		{
			InitializeComponent();
			cursor = this.Cursor;
			InitializeSideBar();

            BtnLine.RaiseEvent(new RoutedEventArgs(RadioButton.ClickEvent));
		}
		private static void UpdateStatBar(Label label, ToolMode? tool) => label.Content = tool.ToString().ToLower();
		private static void UpdateStatBar(Label label, PropertyInfo pi) => label.Content = pi.Name.ToLower();
        private static void UpdateStatBar(Label label, double brushSize) => label.Content = $"{(int)brushSize}pt";
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

            brushSlider.Value = 3;
            radioRound.IsChecked = true;
        }
		private void OpenSideBar(ToolMode tool)
		{
			SideBar.Visibility = Visibility.Visible;
			SideBarHeader.Content = $"{tool.ToString().ToLower()} settings";
		}
        private void DisplayErrorMessage()
        {
            MessageBox.Show("Fehler: Datei konnte nicht geladen werden");
        }
        private void DisplayImage(string uriString)
        {
            try
            {
                Uri uri = new(uriString); //exception, falls Datenpfad nicht vorhanden

                BitmapImage bmp = new(); //exception, falls kein gültiges Bildformat
                bmp.BeginInit();
                bmp.UriSource = uri;
                bmp.CacheOption = BitmapCacheOption.OnLoad; //lädt und decodiert präventiv
                bmp.EndInit();

                Image image = new()
                {
                    Source = bmp,
                    Width = bmp.PixelWidth,
                    Height = bmp.PixelHeight,
                    Stretch = Stretch.Uniform
                };

                Canvas.SetLeft(image, 0);
                Canvas.SetTop(image, 0);

                actualCanvas.Children.Add(image);

                _history.Push(image);

            }
            catch { DisplayErrorMessage(); }
        }
		public static void AddToColorHistory(ObservableCollection<SolidColorBrush> history, SolidColorBrush brush, ComboBox box)
		{
            if (history.Any(b => b.Color == brush.Color)) return;
            history.Insert(0, brush);
            box.SelectedIndex = 0;
            if (history.Count > 10) history.RemoveAt(history.Count - 1);
        }
        private void OnOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = "Bild öffnen",
                Filter = "Imagefiles|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff"
            };

            if (openFileDialog.ShowDialog() == true)
                DisplayImage(openFileDialog.FileName);
        }
        private void OnDrop(object sender, DragEventArgs e) 
        {
            string[]? files = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (files != null && files.Length > 0) 
            {
                DisplayImage(files[0]);
            }
            else { DisplayErrorMessage(); }

        }
        private void OnClose(object sender, RoutedEventArgs e) => Close();
		private void OnSave(object sender, RoutedEventArgs e)
		{
            SaveFileDialog saveFileDialog = new()
            {
                DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = "Bild speichern",
                Filter = "PNG-Datei|*.png|JPEG-Datei|*.jpg|Bitmap-Datei|*.bmp",
                DefaultExt = ".png",
                FileName = "PaintBox3000"
            };

            if (saveFileDialog.ShowDialog() != true) return;

            try
            {
                int width = (int)actualCanvas.ActualWidth;
                int height = (int)actualCanvas.ActualHeight;

                RenderTargetBitmap renderBitmap = new(width, height, 96, 96, PixelFormats.Pbgra32);
                renderBitmap.Render(actualCanvas);

                BitmapEncoder encoder = System.IO.Path.GetExtension(saveFileDialog.FileName).ToLower() switch
                {
                    ".jpg" or ".jpeg" => new JpegBitmapEncoder(),
                    ".bmp" => new BmpBitmapEncoder(),
                    _ => new PngBitmapEncoder()
                };
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                using System.IO.FileStream stream = new(saveFileDialog.FileName, System.IO.FileMode.Create);
                encoder.Save(stream);
            }
            catch
            {
                DisplayErrorMessage();
            }
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
				ToolMode.Line => new DrawableLine(activeStroke, activeBrushSize, activeBrushTip),
				ToolMode.Ellipse => new DrawableEllipse(activeStroke, activeBrushSize, activeFill),
				ToolMode.Rectangle => new DrawableRectangle(activeStroke, activeBrushSize, activeFill),
				ToolMode.Freehand => new DrawableFreehand(activeStroke, activeBrushSize, activeBrushTip),
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

                AddToColorHistory(strokeColorHistory, activeStroke, strokeHistoryCombo);
                if (activeTool is ToolMode.Ellipse or ToolMode.Rectangle)
                    AddToColorHistory(fillColorHistory, activeFill, fillHistoryCombo);
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
            activeBrushSize = e.NewValue;
            if (LblStatStrokeThickness == null) return;
            UpdateStatBar(LblStatStrokeThickness, activeBrushSize);
        }
		private void OnBrushShapeChanged(object sender, RoutedEventArgs e)
		{
            if (sender == radioRound) activeBrushTip = BrushTip.Round;
            else if (sender == radioSquare) activeBrushTip = BrushTip.Square;
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

            strokeHistoryCombo.ItemsSource = strokeColorHistory;			
			fillHistoryCombo.ItemsSource = fillColorHistory;
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
		private void OnStrokeHistorySelected(object sender, RoutedEventArgs e)
		{
			return;
		}
		private void OnFillHistorySelected(object sender, RoutedEventArgs e)
		{
			return;
		}
    }
}