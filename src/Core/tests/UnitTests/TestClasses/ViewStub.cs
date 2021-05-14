using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Primitives;

namespace Microsoft.Maui.UnitTests
{
	public class ViewStub : IView
	{
		public Thickness Margin { get; set; }

		public IList<IGestureRecognizer> GestureRecognizers { get; set; }

		public IList<IGestureRecognizer> CompositeGestureRecognizers { get; set; }

		public bool IsEnabled { get; set; } = true;

		public Color BackgroundColor { get; set; }

		public Rectangle Frame { get; set; }

		public double Width { get; set; } = 20;

		public double Height { get; set; } = 20;

		public IViewHandler Handler { get; set; }

		public IFrameworkElement Parent { get; set; }

		public Size DesiredSize { get; set; } = new Size(20, 20);

		public string AutomationId { get; set; }

		public FlowDirection FlowDirection { get; set; }

		public LayoutAlignment HorizontalLayoutAlignment { get; set; }

		public LayoutAlignment VerticalLayoutAlignment { get; set; }

		public Semantics Semantics { get; set; } = new Semantics();


		public Size Arrange(Rectangle bounds)
		{
			Frame = bounds;
			DesiredSize = bounds.Size;

			return DesiredSize;
		}

		public IList<IGestureView> GetChildElements(Point point) =>
			null;

		public void InvalidateArrange()
		{
		}

		public void InvalidateMeasure()
		{
		}

		public Size Measure(double widthConstraint, double heightConstraint)
		{
			return new Size(widthConstraint, heightConstraint);
		}
	}
}
