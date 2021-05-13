using System;

namespace Microsoft.Maui.Handlers.Benchmarks
{
	public class WindowStub : FrameworkElementStub, IWindow
	{
		public IView View { get; set; }
	}
}