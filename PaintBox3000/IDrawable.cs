using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace PaintBox3000
{
	internal interface IDrawable
	{
		Shape? Visual { get; }
		void SetStart(Point p);
		void SetSize(Point p);
	}
}