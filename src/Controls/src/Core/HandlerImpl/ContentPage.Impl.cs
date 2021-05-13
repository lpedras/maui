using Microsoft.Maui.Graphics;
using Microsoft.Maui.HotReload;

namespace Microsoft.Maui.Controls
{
	public partial class ContentPage : IPage, IHotReloadableView
	{
		// TODO ezhart That there's a layout alignment here tells us this hierarchy needs work :) 
		public Primitives.LayoutAlignment HorizontalLayoutAlignment => Primitives.LayoutAlignment.Fill;

		IView IPage.Content => Content;

		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			if (Content is IFrameworkElement frameworkElement)
			{
				frameworkElement.Measure(widthConstraint, heightConstraint);
			}

			return new Size(widthConstraint, heightConstraint);
		}

		protected override Size ArrangeOverride(Rectangle bounds)
		{
			// Update the Bounds (Frame) for this page
			Layout(bounds);

			if (Content is IFrameworkElement element)
			{
				element.Arrange(bounds);
				element.Handler?.NativeArrange(element.Frame);
			}

			return Frame.Size;
		}

		protected override void InvalidateMeasureOverride()
		{
			base.InvalidateMeasureOverride();

			if (Content is IFrameworkElement frameworkElement)
			{
				frameworkElement.InvalidateMeasure();
			}
		}

		#region HotReload

		IFrameworkElement IReplaceableView.ReplacedView => MauiHotReloadHelper.GetReplacedView(this) ?? this;

		IReloadHandler IHotReloadableView.ReloadHandler { get; set; }

		void IHotReloadableView.TransferState(IView newView)
		{
			//TODO: Let you hot reload the the ViewModel
			//TODO: Lets do a real state transfer
			if (newView is View v)
				v.BindingContext = BindingContext;
		}

		void IHotReloadableView.Reload()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				(this as IView)?.CheckHandlers();
				var reloadHandler = ((IHotReloadableView)this).ReloadHandler;
				reloadHandler?.Reload();
				//TODO: if reload handler is null, Do a manual reload?
			});
		}
		#endregion
	}
}