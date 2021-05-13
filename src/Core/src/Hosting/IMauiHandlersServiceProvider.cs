#nullable enable
using System;

namespace Microsoft.Maui
{
	public interface IMauiHandlersServiceProvider : IMauiServiceProvider
	{
		Type? GetHandlerType(Type view);

		IViewHandler? GetHandler(Type type);

		IViewHandler? GetHandler<T>() where T : IFrameworkElement;
	}
}