#nullable enable
using Microsoft.Maui.Graphics;
namespace Microsoft.Maui
{
	public interface IViewHandler
	{
		void SetMauiContext(IMauiContext mauiContext);
		void SetVirtualView(IFrameworkElement view);
		void UpdateValue(string property);
		void DisconnectHandler();
		object? NativeView { get; }
		IFrameworkElement? VirtualView { get; }
		IMauiContext? MauiContext { get; }
		bool HasContainer { get; set; }
		Size GetDesiredSize(double widthConstraint, double heightConstraint);
		void NativeArrange(Rectangle frame);
	}
}
