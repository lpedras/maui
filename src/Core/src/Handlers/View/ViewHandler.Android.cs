﻿using Android.Widget;
using AndroidX.Core.View;
using AndroidX.Core.View.Accessibility;
using NativeView = Android.Views.View;

namespace Microsoft.Maui.Handlers
{
	public partial class ViewHandler
	{
		MauiAccessibilityDelegate? AccessibilityDelegate { get; set; }

		MauiTouchListener TouchListener { get; } = new MauiTouchListener();

		partial void ConnectingHandler(NativeView? nativeView)
		{
			TouchListener.GestureManager = GestureManager;
			nativeView?.SetOnTouchListener(TouchListener);
		}

		partial void DisconnectingHandler(NativeView? nativeView)
		{
			TouchListener.GestureManager = null;
			nativeView?.SetOnTouchListener(null);

			if (nativeView.IsAlive() && AccessibilityDelegate != null)
			{
				AccessibilityDelegate.Handler = null;
				ViewCompat.SetAccessibilityDelegate(nativeView, null);
				AccessibilityDelegate = null;
			}
		}

		static partial void MappingSemantics(IViewHandler handler, IFrameworkElement view)
		{
			if (view.Semantics != null &&
				handler is ViewHandler viewHandler &&
				viewHandler.AccessibilityDelegate == null &&
				ViewCompat.GetAccessibilityDelegate(handler.NativeView as NativeView) == null)
			{
				if (!string.IsNullOrEmpty(view.Semantics.Hint))
				{
					viewHandler.AccessibilityDelegate = new MauiAccessibilityDelegate() { Handler = viewHandler };
					ViewCompat.SetAccessibilityDelegate(handler.NativeView as NativeView, viewHandler.AccessibilityDelegate);
				}
			}
		}

		public void OnInitializeAccessibilityNodeInfo(NativeView? host, AccessibilityNodeInfoCompat? info)
		{
			var semantics = ((IViewHandler)this).VirtualView?.Semantics;
			if (semantics == null)
				return;

			if (info == null)
				return;

			if (!string.IsNullOrEmpty(semantics.Hint))
			{
				info.HintText = semantics.Hint;

				if (host is EditText)
					info.ShowingHintText = false;
			}
		}

		class MauiAccessibilityDelegate : AccessibilityDelegateCompat
		{
			public ViewHandler? Handler { get; set; }

			public MauiAccessibilityDelegate()
			{
			}

			public override void OnInitializeAccessibilityNodeInfo(NativeView? host, AccessibilityNodeInfoCompat? info)
			{
				base.OnInitializeAccessibilityNodeInfo(host, info);
				Handler?.OnInitializeAccessibilityNodeInfo(host, info);
			}
		}

		class MauiTouchListener : Java.Lang.Object, NativeView.IOnTouchListener
		{
			public GestureManager? GestureManager { get; set; }

			public bool OnTouch(NativeView? v, Android.Views.MotionEvent? e) =>
				GestureManager?.OnTouchEvent(e) ?? false;
		}
	}
}