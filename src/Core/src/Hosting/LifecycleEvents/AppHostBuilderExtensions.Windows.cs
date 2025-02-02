using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using System;

namespace Microsoft.Maui.LifecycleEvents
{ 
	public static partial class AppHostBuilderExtensions
	{
		internal static IAppHostBuilder ConfigureCrossPlatformLifecycleEvents(this IAppHostBuilder builder) =>
			builder.ConfigureLifecycleEvents(events => events.AddWindows(OnConfigureLifeCycle));

		static void OnConfigureLifeCycle(IWindowsLifecycleBuilder windows)
		{
			windows
				.OnWindowCreated(window =>
				{
					window.GetWindow()?.Created();
				})
				.OnResumed(window =>
				{
					window.GetWindow()?.Resumed();
				})
				.OnActivated((window, args) =>
				{
					switch (args.WindowActivationState)
					{
						case UI.Xaml.WindowActivationState.CodeActivated:
						case UI.Xaml.WindowActivationState.PointerActivated:
							window.GetWindow()?.Activated();
							break;
						case UI.Xaml.WindowActivationState.Deactivated:
							window.GetWindow()?.Deactivated();
							break;
					}
				})
				.OnVisibilityChanged((window, args) =>
				{
					if (!args.Visible)
						window.GetWindow()?.Stopped();
				})
				.OnClosed((window, args) =>
				{
					window.GetWindow()?.Destroying();
				});
		}
	}
}
