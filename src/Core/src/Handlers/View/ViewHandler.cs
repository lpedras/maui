#if __IOS__
using NativeView = UIKit.UIView;
#elif __MACOS__
using NativeView = AppKit.NSView;
#elif MONOANDROID
using NativeView = Android.Views.View;
#elif NETSTANDARD
using NativeView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial class ViewHandler
	{
		public static PropertyMapper<IView> ViewMapper = new PropertyMapper<IView>
		{
			[nameof(IView.AutomationId)] = MapAutomationId,
			[nameof(IView.BackgroundColor)] = MapBackgroundColor,
			[nameof(IView.Frame)] = MapFrame,
			[nameof(IView.IsEnabled)] = MapIsEnabled,
			[nameof(IView.Shadow)] = MapShadow
		};

		public static void MapAutomationId(IViewHandler handler, IView view)
		{
			(handler.NativeView as NativeView)?.UpdateAutomationId(view);
		}

		public static void MapBackgroundColor(IViewHandler handler, IView view)
		{
			(handler.NativeView as NativeView)?.UpdateBackgroundColor(view);
		}

		public static void MapFrame(IViewHandler handler, IView view)
		{
			handler.SetFrame(view.Frame);
		}

		public static void MapIsEnabled(IViewHandler handler, IView view)
		{
			(handler.NativeView as NativeView)?.UpdateIsEnabled(view);
		}

		public static void MapShadow(IViewHandler handler, IView view)
		{
			(handler.NativeView as NativeView)?.UpdateShadow(view);
		}
	}
}