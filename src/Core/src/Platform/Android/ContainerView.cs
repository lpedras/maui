﻿using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.HotReload;
using AView = Android.Views.View;

namespace Microsoft.Maui
{
	public class ContainerView : LinearLayout, IReloadHandler
	{
		private AView? _mainView;

		protected ContainerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public ContainerView(IMauiContext context) : base(context.Context)
		{
			_context = context;
		}

		public AView? MainView
		{
			get => _mainView;
			set
			{
				if (_mainView != null)
				{
					RemoveView(_mainView);
				}

				_mainView = value;

				if (_mainView != null)
				{
					_mainView.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
					AddView(_mainView);
				}
			}
		}
		IFrameworkElement? _view;
		readonly IMauiContext? _context;

		public IFrameworkElement? CurrentView
		{
			get => _view;
			set => SetView(value);
		}
		void SetView(IFrameworkElement? view, bool forceRefresh = false)
		{
			if (view == _view && !forceRefresh)
				return;
			_view = view;

			if (_view is IHotReloadableView ihr)
			{
				ihr.ReloadHandler = this;
				MauiHotReloadHelper.AddActiveView(ihr);
			}

			MainView = null;
			if (_view != null)
			{
				_ = _context ?? throw new ArgumentNullException(nameof(_context));
				MainView = _view.ToNative(_context);
			}
		}
		public void Reload() => SetView(CurrentView, true);


	}
}
