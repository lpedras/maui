#nullable enable
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	public class Path : IPath
	{
		public Path()
		{

		}

		public Path(string? data)
		{
			Data = data;
		}

		public string? Data { get; set; }
	}
}
