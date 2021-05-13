namespace Microsoft.Maui.Handlers
{
	public partial class LayoutHandler : ViewHandler<ILayout, object>
	{
		public void Add(IView view) { }

		public void Remove(IView view) { }

		protected override object CreateNativeView() =>
			new();
	}
}
