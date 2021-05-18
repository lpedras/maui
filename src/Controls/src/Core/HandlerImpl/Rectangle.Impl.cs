using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	public class Rectangle : IRectangle
	{
		public Rectangle()
		{

		}

		public Rectangle(CornerRadius cornerRadius) : this()
		{
			CornerRadius = cornerRadius;
		}

		public CornerRadius CornerRadius { get; set; }
	}
}
