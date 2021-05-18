using System;

namespace Microsoft.Maui.Controls
{
	public interface IFlyoutPageController
	{
		bool CanChangeIsPresented { get; set; }

		Graphics.Rectangle DetailBounds { get; set; }

		Graphics.Rectangle FlyoutBounds { get; set; }

		bool ShouldShowSplitMode { get; }

		void UpdateFlyoutLayoutBehavior();

		event EventHandler<BackButtonPressedEventArgs> BackButtonPressed;
	}

	[Obsolete("IMasterDetailPageController is obsolete as of version 5.0.0. Please use IFlyoutPageController instead.")]
	public interface IMasterDetailPageController
	{
		bool CanChangeIsPresented { get; set; }

		Graphics.Rectangle DetailBounds { get; set; }

		Graphics.Rectangle MasterBounds { get; set; }

		bool ShouldShowSplitMode { get; }

		void UpdateMasterBehavior();

		event EventHandler<BackButtonPressedEventArgs> BackButtonPressed;
	}
}
