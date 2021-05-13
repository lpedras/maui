#nullable enable
namespace Microsoft.Maui.HotReload
{
	public interface IHotReloadableView : IReplaceableView, IFrameworkElement
	{
		IReloadHandler ReloadHandler { get; set; }
		void TransferState(IView newView);
		void Reload();
	}
}