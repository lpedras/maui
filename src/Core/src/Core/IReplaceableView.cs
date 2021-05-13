using System;

namespace Microsoft.Maui
{
	public interface IReplaceableView
	{
		IFrameworkElement ReplacedView { get; }
	}
}
