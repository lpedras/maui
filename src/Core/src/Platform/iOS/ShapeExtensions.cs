using Microsoft.Maui.Graphics;
using CoreGraphics;
using Microsoft.Maui.Graphics.Native;

namespace Microsoft.Maui
{
	public static class ShapeExtensions
	{
		public static CGPath ToNative(this PathF pathF)
		{
			return pathF.AsCGPath();
		}
	}
}