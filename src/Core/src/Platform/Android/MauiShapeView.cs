using Android.Content;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native;

namespace Microsoft.Maui
{
	public class MauiShapeView : NativeGraphicsView
	{
		public MauiShapeView(Context? context, IDrawable? drawable = null) : base(context, drawable)
		{

		}
	}
}