using System.Windows.Controls;
using System.Windows.Shapes;

namespace PaintBox3000
{
	internal interface IDrawable
	{
		Shape? Visual { get; }
		void OnPressed(double x1, double y1);
		void OnMoved(double x2, double y2);
	}
}