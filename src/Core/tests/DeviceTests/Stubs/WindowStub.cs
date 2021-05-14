namespace Microsoft.Maui.DeviceTests.Stubs
{
	public class WindowStub : FrameworkElementStub, IWindow
	{
		public IFrameworkElement View { get; set; }
	}
}