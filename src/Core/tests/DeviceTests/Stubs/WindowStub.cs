namespace Microsoft.Maui.DeviceTests.Stubs
{
	public class WindowStub : FrameworkElementStub, IWindow
	{
		public IView View { get; set; }
	}
}