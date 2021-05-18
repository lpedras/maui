using System.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.Specialized;
using Microsoft.Maui.Controls.Platform;

#if WINDOWS
using WFillRule = Microsoft.UI.Xaml.Media.FillRule;
using WPolygon = Microsoft.UI.Xaml.Shapes.Polygon;

namespace Microsoft.Maui.Controls.Compatibility.Platform.UWP
#else
using Microsoft.Maui.Controls.Compatibility.Platform.WPF.Extensions;
using WFillRule = System.Windows.Media.FillRule;
using WPolygon = System.Windows.Shapes.Polygon;

namespace Microsoft.Maui.Controls.Compatibility.Platform.WPF
#endif
{
	public class PolygonRenderer : ShapeRenderer<Shapes.Polygon, WPolygon>
	{
		PointCollection _points;

		protected override void OnElementChanged(ElementChangedEventArgs<Shapes.Polygon> args)
		{
			if (Control == null && args.NewElement != null)
			{
				SetNativeControl(new WPolygon());
			}

			base.OnElementChanged(args);

			if (args.NewElement != null)
			{
				var points = args.NewElement.Points;
				points.CollectionChanged += OnCollectionChanged;

				UpdatePoints();
				UpdateFillRule();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(sender, args);

			if (args.PropertyName == Shapes.Polygon.PointsProperty.PropertyName)
				UpdatePoints();
			else if (args.PropertyName == Shapes.Polygon.FillRuleProperty.PropertyName)
				UpdateFillRule();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				if (_points != null)
				{
					_points.CollectionChanged -= OnCollectionChanged;
					_points = null;
				}
			}
		}

		void UpdatePoints()
		{
			if (_points != null)
				_points.CollectionChanged -= OnCollectionChanged;

			_points = Element.Points;

			_points.CollectionChanged += OnCollectionChanged;

			Control.Points = _points.ToWindows();
		}

		void UpdateFillRule()
		{
			Control.FillRule = Element.FillRule == FillRule.EvenOdd ?
				WFillRule.EvenOdd :
				WFillRule.Nonzero;
		}

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdatePoints();
		}
	}
}