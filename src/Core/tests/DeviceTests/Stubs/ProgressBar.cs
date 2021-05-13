using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.DeviceTests.Stubs
{
	public class ProgressBarStub : ViewStub, IProgress
	{
		public double Progress { get; set; }

		public Color ProgressColor { get; set; }
	}
}