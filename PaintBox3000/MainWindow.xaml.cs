using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Reflection;
using Microsoft.Win32;
using PaintBox3000.Enums;
using PaintBox3000.Drawables;
using PaintBox3000.Core;

namespace PaintBox3000
{
    /// <summary>
    /// orchestrates UI and functionality
    /// </summary>
    public partial class MainWindow : Window
	{
		private Cursor _cursor;
		private ToolMode _activeTool;
		private AbstractDrawable? _activeShape;
		private SolidColorBrush _activeFill;
		private SolidColorBrush _activeStroke;
		private double _activeBrushSize;
		private BrushTip _activeBrushTip;

        private readonly ImageFileService _imageFileService = new();
        private readonly CanvasHistoryManager _historyManager = new();
        private readonly ColorCatalog _colorCatalog = new();
        private readonly ColorHistoryManager _colorHistoryManager = new();

        public MainWindow()
		{
			InitializeComponent();
			_cursor = this.Cursor;
			InitializeSideBar();
            InitializeDefaultChoices();
            InitializeColorHistoryBox();
        }
		private static void UpdateStatBar(Label label, ToolMode tool) => label.Content = tool.ToString().ToLowerInvariant();
		private static void UpdateStatBar(Label label, PropertyInfo pi) => label.Content = pi.Name.ToLowerInvariant();
        private static void UpdateStatBar(Label label, double brushSize) => label.Content = $"{(int)brushSize}pt";
        
        private void InitializeDefaultChoices()
        {
            _activeFill = _colorCatalog.GetFirstColor();
            _activeStroke = _colorCatalog.GetLastColor();

            BtnLine.RaiseEvent(new RoutedEventArgs(RadioButton.ClickEvent));
        }
        private void InitializeColorHistoryBox()
        {
            strokeHistoryCombo.ItemsSource = _colorHistoryManager.StrokeHistory;
            fillHistoryCombo.ItemsSource = _colorHistoryManager.FillHistory;
        }
		private void InitializeSideBar()
		{
            PropertyInfo[] colorProperties = _colorCatalog.SortedColors;

            fillColorList.ItemsSource = colorProperties;
			fillColorList.SelectedIndex = 0;

            strokeColorList.ItemsSource = colorProperties;
			strokeColorList.SelectedIndex = colorProperties.Length - 1;

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
            MessageBox.Show("Fehler: Datei konnte nicht gespeichert / geladen werden");
        }
        private void SelectTool(ToolMode tool, RadioButton button)
        {
            button.IsChecked = true;
            _activeTool = tool;
            OpenSideBar(_activeTool);
            UpdateStatBar(LblSBTool, _activeTool);
        }
        private void DisplayImage(string path)
        {
            try
            {
                BitmapImage bmp = _imageFileService.LoadImage(path);

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

                _historyManager.Push(image);
            }
            catch (Exception)
            {
                DisplayErrorMessage();
            }
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            actualCanvas.MinWidth = actualCanvas.ActualWidth;
            actualCanvas.MinHeight = actualCanvas.ActualHeight;

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

            if (files?.Length > 0)
                DisplayImage(files[0]);
            else
                DisplayErrorMessage();
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
                _imageFileService.SaveCanvas(actualCanvas, saveFileDialog.FileName);
            }
            catch (Exception)
            {
                DisplayErrorMessage();
            }
        }
        private void OnPaintLine(object sender, RoutedEventArgs e) => SelectTool(ToolMode.Line, BtnLine);
        private void OnPaintEllipse(object sender, RoutedEventArgs e) => SelectTool(ToolMode.Ellipse, BtnEllipse);
        private void OnPaintRectangle(object sender, RoutedEventArgs e) => SelectTool(ToolMode.Rectangle, BtnRectangle);
        private void OnPaintFreehand(object sender, RoutedEventArgs e) => SelectTool(ToolMode.Freehand, BtnFreehand);
        private void OnPressed(object sender, MouseButtonEventArgs e)
		{
            _activeShape = DrawableFactory.Create(_activeTool, _activeStroke, _activeBrushSize, _activeFill, _activeBrushTip);
            _activeShape.SetStart(e.GetPosition(actualCanvas));

			if (_activeShape.Visual == null) return;
            actualCanvas.CaptureMouse();
            this.Cursor = Cursors.Cross;

            actualCanvas.Children.Add(_activeShape.Visual);
        }
		private void OnMoved(object sender, MouseEventArgs e)
		{
			_activeShape?.SetSize(e.GetPosition(actualCanvas));
		}
		private void OnReleased(object sender, MouseButtonEventArgs e)
		{
			actualCanvas.ReleaseMouseCapture();
			this.Cursor = _cursor;
			if (_activeShape?.Visual != null)
			{
				_historyManager.Push(_activeShape.Visual);
				Point bottomRight = _activeShape.BottomRight;
				if (bottomRight.X > actualCanvas.MinWidth) 
                    actualCanvas.MinWidth = bottomRight.X;
				if (bottomRight.Y > actualCanvas.MinHeight) 
                    actualCanvas.MinHeight = bottomRight.Y;

                _colorHistoryManager.AddStroke(_activeStroke);
                strokeHistoryCombo.SelectedIndex = 0;
                if (_activeTool is ToolMode.Ellipse or ToolMode.Rectangle)
                    _colorHistoryManager.AddFill(_activeFill);
                    fillHistoryCombo.SelectedIndex = 0;
            }
			_activeShape = null;
        }
		private void OnClickClear(object sender, RoutedEventArgs e)
		{
			actualCanvas.Children.Clear();
            _historyManager.ClearHistory();

        }
		private void OnClickUndo(object sender, RoutedEventArgs e)
		{
            UIElement? element = _historyManager.Undo();
            if (element == null) return;

            actualCanvas.Children.Remove(element);

        }
		private void OnClickRedo(object sender, RoutedEventArgs e)
		{
            UIElement? element = _historyManager.Redo();
            if (element == null) return;

            actualCanvas.Children.Add(element);
        }
		private void OnBrushSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
            _activeBrushSize = e.NewValue;
            if (LblStatStrokeThickness == null) return;
            UpdateStatBar(LblStatStrokeThickness, _activeBrushSize);
        }
		private void OnBrushShapeChanged(object sender, RoutedEventArgs e)
		{
            if (sender == radioRound) _activeBrushTip = BrushTip.Round;
            else if (sender == radioSquare) _activeBrushTip = BrushTip.Square;
        }
		private void OnStrokeColorChanged(object sender, SelectionChangedEventArgs e)
		{
			if (strokeColorList.SelectedItem is not PropertyInfo colorProperty) return;
			_activeStroke = _colorCatalog.ToBrush(colorProperty);
            _colorHistoryManager.AddStroke(_activeStroke);
            UpdateStatBar(LblSBStrokeColor, colorProperty);
		}
		private void OnFillColorChanged(object sender, SelectionChangedEventArgs e)
		{
			if (fillColorList.SelectedItem is not PropertyInfo colorProperty) return;
			_activeFill = _colorCatalog.ToBrush(colorProperty);
            _colorHistoryManager.AddFill(_activeFill);
            UpdateStatBar(LblSBFillColor, colorProperty);
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
			else if (SideBar.Visibility == Visibility.Collapsed)
			{
				MainGrid.ColumnDefinitions[3].Width = new GridLength(0);
				Splitter.IsEnabled = false;
			}
			
		}
		private void OnStrokeHistorySelected(object sender, RoutedEventArgs e)
		{
            if (strokeHistoryCombo.SelectedItem is SolidColorBrush brush)
            {
                _activeStroke = brush;
                PropertyInfo? colorProperty = _colorCatalog.GetPropertyInfo(brush.Color);

                if (colorProperty != null)
                    UpdateStatBar(LblSBStrokeColor, colorProperty);
            }
        }
		private void OnFillHistorySelected(object sender, RoutedEventArgs e)
		{
            if (fillHistoryCombo.SelectedItem is SolidColorBrush brush)
            {
                _activeFill = brush;

                PropertyInfo? colorProperty = _colorCatalog.GetPropertyInfo(brush.Color);

                if (colorProperty != null)
                    UpdateStatBar(LblSBFillColor, colorProperty);
            }
        }
    }
}