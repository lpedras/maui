using System;
using Android.Views;
using Microsoft.Maui.Graphics;
using IVisualElementRenderer = Microsoft.Maui.Controls.Compatibility.Platform.Android.IVisualElementRenderer;
using ViewHandler = Microsoft.Maui.Handlers.ViewHandler<Microsoft.Maui.IView, Android.Views.View>;
using VisualElementChangedEventArgs = Microsoft.Maui.Controls.Compatibility.Platform.Android.VisualElementChangedEventArgs;

namespace Microsoft.Maui.Controls.Compatibility
{
	public class RendererToHandlerShim : ViewHandler
	{
		internal IVisualElementRenderer VisualElementRenderer { get; private set; }

		public static IViewHandler CreateShim(object renderer)
		{
			if (renderer is IViewHandler handler)
				return handler;

			if (renderer is IVisualElementRenderer ivr)
				return new RendererToHandlerShim(ivr);

			return new RendererToHandlerShim();
		}

		public RendererToHandlerShim() : base(ViewHandler.ViewMapper)
		{
		}

		public RendererToHandlerShim(IVisualElementRenderer visualElementRenderer) : this()
		{
			if (visualElementRenderer != null)
				SetupRenderer(visualElementRenderer);
		}

		public void SetupRenderer(IVisualElementRenderer visualElementRenderer)
		{
			VisualElementRenderer = visualElementRenderer;
			VisualElementRenderer.ElementChanged += OnElementChanged;

			if (VisualElementRenderer.Element is IView view)
			{
				view.Handler = this;
				SetVirtualView(view);
			}
			else if (VisualElementRenderer.Element != null)
				throw new Exception($"{VisualElementRenderer.Element} must implement: {nameof(Microsoft.Maui.IView)}");
		}

		void OnElementChanged(object sender, VisualElementChangedEventArgs e)
		{
			if (e.OldElement is IView view)
				view.Handler = null;

			if (e.NewElement is IView newView)
			{
				newView.Handler = this;
				this.SetVirtualView(newView);
			}
			else if (e.NewElement != null)
				throw new Exception($"{e.NewElement} must implement: {nameof(Microsoft.Maui.IView)}");
		}

		protected override global::Android.Views.View CreateNativeView()
		{
			return VisualElementRenderer.View;
		}

		protected override void ConnectHandler(global::Android.Views.View nativeView)
		{
			base.ConnectHandler(nativeView);
			VirtualView.Handler = this;
		}

		protected override void DisconnectHandler(global::Android.Views.View nativeView)
		{
			Platform.Android.AppCompat.Platform.SetRenderer(
				VisualElementRenderer.Element,
				null);

			VisualElementRenderer.SetElement(null);

			base.DisconnectHandler(nativeView);
			VirtualView.Handler = null;
		}

		public override void SetVirtualView(IView view)
		{
			if (VisualElementRenderer == null && Context != null)
			{
				var renderer = Internals.Registrar.Registered.GetHandlerForObject<IVisualElementRenderer>(view, Context)
										   ?? new Platform.Android.AppCompat.Platform.DefaultRenderer(Context);

				SetupRenderer(renderer);
			}

			if (VisualElementRenderer.Element != view)
			{
				VisualElementRenderer.SetElement((VisualElement)view);
			}
			else
			{
				base.SetVirtualView(view);
			}

			Platform.Android.AppCompat.Platform.SetRenderer(
				VisualElementRenderer.Element,
				VisualElementRenderer);
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			return Platform.Android.AppCompat.Platform.GetNativeSize(
				VisualElementRenderer, widthConstraint, heightConstraint);
		}

		public override void NativeArrange(Rectangle frame)
		{
			// This is a hack to force the shimmed control to actually do layout; without this, some controls won't actually
			// call OnLayout after SetFrame if their sizes haven't changed (e.g., ScrollView)
			// Luckily, measuring with MeasureSpecMode.Exactly is pretty fast, since it just returns the value you give it.
			NativeView?.Measure(MeasureSpecMode.Exactly.MakeMeasureSpec((int)frame.Width),
				MeasureSpecMode.Exactly.MakeMeasureSpec((int)frame.Height));

			base.NativeArrange(frame);
		}
	}
}
