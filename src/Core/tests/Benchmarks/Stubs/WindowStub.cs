namespace Microsoft.Maui.Handlers.Benchmarks
{
	public class WindowStub : FrameworkElementStub, IWindow
	{
		public IFrameworkElement View { get; set; }
	}
}