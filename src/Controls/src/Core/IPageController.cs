using System.Collections.ObjectModel;

namespace Microsoft.Maui.Controls
{
	public interface IPageController : IVisualElementController
	{
		Graphics.Rectangle ContainerArea { get; set; }

		bool IgnoresContainerArea { get; set; }

		ObservableCollection<Element> InternalChildren { get; }

		void SendAppearing();

		void SendDisappearing();
	}
}