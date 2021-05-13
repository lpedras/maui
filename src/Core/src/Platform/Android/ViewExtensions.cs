using AndroidX.Core.View;
using AView = Android.Views.View;

namespace Microsoft.Maui
{
	public static class ViewExtensions
	{
		const int DefaultAutomationTagId = -1;

		public static int AutomationTagId { get; set; } = DefaultAutomationTagId;

		public static void UpdateIsEnabled(this AView nativeView, IFrameworkElement view)
		{
			if (nativeView != null)
				nativeView.Enabled = view.IsEnabled;
		}

		public static void UpdateBackgroundColor(this AView nativeView, IFrameworkElement view)
		{
			var backgroundColor = view.BackgroundColor;
			if (backgroundColor != null)
				nativeView?.SetBackgroundColor(backgroundColor.ToNative());
		}

		public static bool GetClipToOutline(this AView view)
		{
			return view.ClipToOutline;
		}

		public static void SetClipToOutline(this AView view, bool value)
		{
			view.ClipToOutline = value;
		}

		public static void UpdateAutomationId(this AView nativeView, IFrameworkElement view)
		{
			if (AutomationTagId == DefaultAutomationTagId)
			{
				AutomationTagId = Resource.Id.automation_tag_id;
			}

			nativeView.SetTag(AutomationTagId, view.AutomationId);
		}

		public static void UpdateSemantics(this AView nativeView, IFrameworkElement view)
		{
			var semantics = view.Semantics;
			if (semantics == null)
				return;

			nativeView.ContentDescription = semantics.Description;
			ViewCompat.SetAccessibilityHeading(nativeView, semantics.IsHeading);
		}

		public static void InvalidateMeasure(this AView nativeView, IFrameworkElement view)
		{
			nativeView.RequestLayout();
		}

		public static void UpdateWidth(this AView nativeView, IFrameworkElement view)
		{
			// GetDesiredSize will take the specified Width into account during the layout
			if (!nativeView.IsInLayout)
			{
				nativeView.RequestLayout();
			}
		}

		public static void UpdateHeight(this AView nativeView, IFrameworkElement view)
		{
			// GetDesiredSize will take the specified Height into account during the layout
			if (!nativeView.IsInLayout)
			{
				nativeView.RequestLayout();
			}
		}
	}
}