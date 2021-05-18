using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native;

namespace Microsoft.Maui
{
	public static class ShapeExtensions
	{
		public static Android.Graphics.Path ToNative(this PathF pathF)
		{
			return pathF.AsAndroidPath();
		}
	}
}