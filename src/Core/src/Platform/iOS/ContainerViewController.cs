﻿using System;
using Microsoft.Maui.HotReload;
using UIKit;
namespace Microsoft.Maui
{
	public class ContainerViewController : UIViewController, IReloadHandler
	{
		IFrameworkElement? _view;
		UIView? _currentNativeView;

		// The handler needs this view before LoadView is called on the controller
		// So this is used to create the first view that the handler will use
		// without forcing the VC to call LoadView
		UIView? _pendingLoadedView;

		public IFrameworkElement? CurrentView
		{
			get => _view;
			set => SetView(value);
		}

		public UIView? CurrentNativeView
			=> _pendingLoadedView ?? _currentNativeView;

		public IMauiContext? Context { get; set; }

		void SetView(IFrameworkElement? view, bool forceRefresh = false)
		{
			if (view == _view && !forceRefresh)
				return;

			_view = view;

			if (view is IPage page)
				Title = page.Title;

			if (_view is IHotReloadableView ihr)
			{
				ihr.ReloadHandler = this;
				MauiHotReloadHelper.AddActiveView(ihr);
			}
			_currentNativeView?.RemoveFromSuperview();
			_currentNativeView = null;

			if (IsViewLoaded && _view != null)
				LoadNativeView(_view);
		}

		internal UIView LoadFirstView(IFrameworkElement view)
		{
			_pendingLoadedView = CreateNativeView(view);
			return _pendingLoadedView;
		}

		public override void LoadView()
		{
			base.LoadView();
			if (_view != null && Context != null)
				LoadNativeView(_view);
		}

		void LoadNativeView(IFrameworkElement view)
		{
			_currentNativeView = _pendingLoadedView ?? CreateNativeView(view);
			_pendingLoadedView = null;
			View!.AddSubview(_currentNativeView);
			if (view.Background == null)
				View.BackgroundColor = UIColor.SystemBackgroundColor;
		}

		protected virtual UIView CreateNativeView(IFrameworkElement view)
		{
			_ = Context ?? throw new ArgumentNullException(nameof(Context));
			_ = _view ?? throw new ArgumentNullException(nameof(view));

			return _view.ToNative(Context);
		}

		public override void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();

			if (_currentNativeView == null)
				return;

			_currentNativeView.Frame = View!.Bounds;
		}

		public void Reload() =>
			SetView(CurrentView, true);
	}
}