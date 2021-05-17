#nullable enable
using Microsoft.Maui.Graphics;
using System;
#if __IOS__ || MACCATALYST
using NativeView = UIKit.UIView;
#elif MONOANDROID
using NativeView = Android.Views.View;
#elif WINDOWS
using NativeView = Microsoft.UI.Xaml.FrameworkElement;
#elif NETSTANDARD
using NativeView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public abstract partial class ViewHandler : IViewHandler
	{
		bool _hasContainer;

		public static PropertyMapper<IFrameworkElement> ViewMapper = new PropertyMapper<IFrameworkElement>
		{
			[nameof(IFrameworkElement.AutomationId)] = MapAutomationId,
			[nameof(IFrameworkElement.Visibility)] = MapVisibility,
			[nameof(IFrameworkElement.Background)] = MapBackground,
			[nameof(IFrameworkElement.Width)] = MapWidth,
			[nameof(IFrameworkElement.Height)] = MapHeight,
			[nameof(IFrameworkElement.IsEnabled)] = MapIsEnabled,
			[nameof(IFrameworkElement.Semantics)] = MapSemantics,
			Actions =
			{
				[nameof(IFrameworkElement.InvalidateMeasure)] = MapInvalidateMeasure
			}
		};

		internal ViewHandler()
		{

		}

		public bool HasContainer
		{
			get => _hasContainer;
			set
			{
				if (_hasContainer == value)
					return;

				_hasContainer = value;

				if (value)
					SetupContainer();
				else
					RemoveContainer();
			}
		}

		protected abstract void SetupContainer();

		protected abstract void RemoveContainer();

		public IMauiContext? MauiContext { get; private set; }

		public IServiceProvider? Services => MauiContext?.Services;

		public object? NativeView { get; private protected set; }

		public IFrameworkElement? VirtualView { get; private protected set; }

		public void SetMauiContext(IMauiContext mauiContext) => MauiContext = mauiContext;

		public abstract void SetVirtualView(IFrameworkElement view);

		public abstract void UpdateValue(string property);

		void IViewHandler.DisconnectHandler() =>
			DisconnectHandler(((NativeView?)NativeView));

		public abstract Size GetDesiredSize(double widthConstraint, double heightConstraint);

		public abstract void NativeArrange(Rectangle frame);

		internal GestureManager? GestureManager { get; private set; }

		partial void ConnectingHandler(NativeView? nativeView);

		private protected void ConnectHandler(NativeView? nativeView)
		{
			if (VirtualView is IGestureController)
			{
				GestureManager = new GestureManager();
				GestureManager.SetViewHandler(this);
			}

			ConnectingHandler(nativeView);
		}

		partial void DisconnectingHandler(NativeView? nativeView);

		private protected void DisconnectHandler(NativeView? nativeView)
		{
			DisconnectingHandler(nativeView);
			
			if (GestureManager != null)
			{
				GestureManager.Dispose();
				GestureManager = null;
			}

			if (VirtualView != null)
				VirtualView.Handler = null;

			VirtualView = null;
		}

		public static void MapWidth(IViewHandler handler, IFrameworkElement view)
		{
			((NativeView?)handler.NativeView)?.UpdateWidth(view);
		}

		public static void MapHeight(IViewHandler handler, IFrameworkElement view)
		{
			((NativeView?)handler.NativeView)?.UpdateHeight(view);
		}

		public static void MapIsEnabled(IViewHandler handler, IFrameworkElement view)
		{
			((NativeView?)handler.NativeView)?.UpdateIsEnabled(view);
		}

		public static void MapVisibility(IViewHandler handler, IFrameworkElement view)
		{
			((NativeView?)handler.NativeView)?.UpdateVisibility(view);
		}

		public static void MapBackground(IViewHandler handler, IFrameworkElement view)
		{
			((NativeView?)handler.NativeView)?.UpdateBackground(view);
		}

		public static void MapAutomationId(IViewHandler handler, IFrameworkElement view)
		{
			((NativeView?)handler.NativeView)?.UpdateAutomationId(view);
		}

		static partial void MappingSemantics(IViewHandler handler, IFrameworkElement view);

		public static void MapSemantics(IViewHandler handler, IFrameworkElement view)
		{
			MappingSemantics(handler, view);
			((NativeView?)handler.NativeView)?.UpdateSemantics(view);
		}

		public static void MapInvalidateMeasure(IViewHandler handler, IFrameworkElement view)
		{
			((NativeView?)handler.NativeView)?.InvalidateMeasure(view);
		}
	}
}