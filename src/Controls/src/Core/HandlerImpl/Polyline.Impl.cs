#nullable enable
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	public class Polyline : IPolyline
	{
		public Polyline()
		{

		}

		public Polyline(PointCollection? points)
		{
			Points = points;
		}

		public PointCollection? Points { get; set; }
	}
}