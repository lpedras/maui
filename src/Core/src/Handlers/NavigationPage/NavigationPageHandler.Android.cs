﻿#nullable enable

using System;
using System.Collections.Generic;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Navigation;
using AndroidX.Navigation.Fragment;
using Google.Android.Material.AppBar;
using AView = Android.Views.View;

namespace Microsoft.Maui.Handlers
{
	internal partial class NavigationPageHandler :
		ViewHandler<INavigationView, AView>
	{
		private NavHostFragment? _navHost;
		private FragmentNavigator? _fragmentNavigator;
		private Toolbar? _toolbar;
		private AppBarLayout? _appBar;

		NavHostFragment NavHost
		{
			get => _navHost ?? throw new InvalidOperationException($"NavHost cannot be null");
			set => _navHost = value;
		}

		FragmentNavigator FragmentNavigator
		{
			get => _fragmentNavigator ?? throw new InvalidOperationException($"FragmentNavigator cannot be null");
			set => _fragmentNavigator = value;
		}

		int NativeNavigationStackCount => NavHost?.NavController.BackStack.Size() - 1 ?? 0;
		int NavigationStackCount => VirtualView?.NavigationStack.Count ?? 0;

		internal Toolbar Toolbar
		{
			get => _toolbar ?? throw new InvalidOperationException($"ToolBar cannot be null");
			set => _toolbar = value;
		}

		internal AppBarLayout AppBar
		{
			get => _appBar ?? throw new InvalidOperationException($"AppBar cannot be null");
			set => _appBar = value;
		}

		protected override AView CreateNativeView()
		{
			LayoutInflater? li = LayoutInflater.From(Context);
			_ = li ?? throw new InvalidOperationException($"LayoutInflater cannot be null");

			var view = li.Inflate(Resource.Layout.navigationlayout, null).JavaCast<NavigationLayout>();
			_ = view ?? throw new InvalidOperationException($"Resource.Layout.navigationlayout view not found");

			_toolbar = view.FindViewById<Toolbar>(Resource.Id.maui_toolbar);
			_appBar = view.FindViewById<AppBarLayout>(Resource.Id.appbar);
			return view;
		}

		protected override void ConnectHandler(AView nativeView)
		{
			var fragmentManager = Context.GetFragmentManager();
			_ = fragmentManager ?? throw new InvalidOperationException($"GetFragmentManager returned null");
			_ = VirtualView ?? throw new InvalidOperationException($"VirtualView cannot be null");

			NavHost = (NavHostFragment)
				fragmentManager.FindFragmentById(Resource.Id.nav_host);

			FragmentNavigator =
				(FragmentNavigator)NavHost
					.NavController
					.NavigatorProvider
					.GetNavigator(Java.Lang.Class.FromType(typeof(FragmentNavigator)));


			var navGraphNavigator =
				(NavGraphNavigator)NavHost
					.NavController
					.NavigatorProvider
					.GetNavigator(Java.Lang.Class.FromType(typeof(NavGraphNavigator)));

			base.ConnectHandler(nativeView);

			var inflater = NavHost.NavController.NavInflater;
			NavGraph graph = new NavGraph(navGraphNavigator);

			NavDestination navDestination;
			List<int> destinations = new List<int>();
			foreach (var page in VirtualView.NavigationStack)
			{
				navDestination =
					MauiFragmentNavDestination.
						AddDestination(
							page,
							this,
							graph,
							FragmentNavigator);

				destinations.Add(navDestination.Id);
			}

			graph.StartDestination = destinations[0];

			NavHost.NavController.SetGraph(graph, null);

			for (var i = NativeNavigationStackCount; i < NavigationStackCount; i++)
			{
				var dest = destinations[i];
				NavHost.NavController.Navigate(dest);
			}
		}

		private static void PushAsyncTo(NavigationPageHandler arg1, INavigationView arg2, object? arg3)
		{
			if (arg3 is not MauiNavigationRequestedEventArgs e)
				return;

			var destination =
				MauiFragmentNavDestination.AddDestination(e.Page, arg1, arg1.NavHost.NavController.Graph, arg1.FragmentNavigator);

			arg1.NavHost.NavController.Navigate(destination.Id, null);
		}

		private static void PopAsyncTo(NavigationPageHandler arg1, INavigationView arg2, object? arg3)
		{
			arg1.NavHost.NavController.NavigateUp();
		}

		internal void OnPop()
		{
			VirtualView
				.PopAsync()
				.FireAndForget((e) =>
				{
					//Log.Warning(nameof(NavigationPageHandler), $"{e}");
				});
		}

		//void OnPopped(object? sender, NavigationRequestedEventArgs e)
		//{
		//	NavHost.NavController.NavigateUp();
		//}

		//void UpdatePadding()
		//{
		//}

		//void UpdateBarTextColor()
		//{
		//	UpdateBarBackground();
		//}

		//void UpdateBarBackground()
		//{
		//	var context = Context;
		//	var bar = Toolbar;
		//	//ActionBarDrawerToggle toggle = _drawerToggle;

		//	if (bar == null)
		//		return;

		//	//bool isNavigated = NavigationPageController.StackDepth > 1;
		//	//bar.NavigationIcon = null;
		//	var navPage = VirtualView;

		//	//if (isNavigated)
		//	//{
		//	//	if (NavigationPage.GetHasBackButton(currentPage) && !Context.IsDesignerContext())
		//	//	{
		//	//		if (toggle != null)
		//	//		{
		//	//			toggle.DrawerIndicatorEnabled = false;
		//	//			toggle.SyncState();
		//	//		}

		//	//		var activity = (AppCompatActivity)context.GetActivity();
		//	//		var icon = new DrawerArrowDrawable(activity.SupportActionBar.ThemedContext);
		//	//		icon.Progress = 1;
		//	//		bar.NavigationIcon = icon;

		//	//		var prevPage = Element.Peek(1);
		//	//		var backButtonTitle = NavigationPage.GetBackButtonTitle(prevPage);
		//	//		_defaultNavigationContentDescription = backButtonTitle != null
		//	//			? bar.SetNavigationContentDescription(prevPage, backButtonTitle)
		//	//			: bar.SetNavigationContentDescription(prevPage, _defaultNavigationContentDescription);
		//	//	}
		//	//	else if (toggle != null && _flyoutPage != null)
		//	//	{
		//	//		toggle.DrawerIndicatorEnabled = _flyoutPage.ShouldShowToolbarButton();
		//	//		toggle.SyncState();
		//	//	}
		//	//}
		//	//else
		//	//{
		//	//	if (toggle != null && _flyoutPage != null)
		//	//	{
		//	//		toggle.DrawerIndicatorEnabled = _flyoutPage.ShouldShowToolbarButton();
		//	//		toggle.SyncState();
		//	//	}
		//	//}

		//	var tintColor = navPage.BarBackgroundColor;
		//	Brush barBackground = navPage.BarBackground;

		//	if (barBackground == null && tintColor != null)
		//		barBackground = new SolidColorBrush(tintColor);

		//	if (barBackground != null)
		//		bar.UpdateBackground(barBackground);
		//	//else if (tintColor == null)
		//	//	bar.BackgroundTintMode = null;
		//	//else
		//	//{
		//	//	bar.Background = null;
		//	//	bar.BackgroundTintMode = PorterDuff.Mode.Src;
		//	//	bar.BackgroundTintList = ColorStateList.ValueOf(tintColor.ToNative());
		//	//}

		//	var textColor = navPage.BarTextColor;
		//	if (textColor != null)
		//		bar.SetTitleTextColor(textColor.ToNative().ToArgb());

		//	//var navIconColor = NavigationPage.GetIconColor(currentPage);
		//	//if (navIconColor != null && bar.NavigationIcon != null)
		//	//	DrawableExtensions.SetColorFilter(bar.NavigationIcon, navIconColor, FilterMode.SrcAtop);

		//	bar.Title = navPage.CurrentPage?.Title ?? string.Empty;

		//	//if (_toolbar.NavigationIcon != null && textColor != null)
		//	//{
		//	//	var icon = _toolbar.NavigationIcon as DrawerArrowDrawable;
		//	//	if (icon != null)
		//	//		icon.Color = textColor.ToAndroid().ToArgb();
		//	//}

		//	//UpdateTitleIcon();

		//	//UpdateTitleView();
		//}

		//void UpdateTitleIcon()
		//{
		//}
		//void UpdateTitleView()
		//{
		//}


		//public static void MapPadding(NavigationPageHandler handler, INavigationView view)
		//	=> handler.UpdatePadding();

		//public static void MapBarTextColor(NavigationPageHandler handler, INavigationView view)
		//	=> handler.UpdateBarTextColor();

		//public static void MapBarBackground(NavigationPageHandler handler, INavigationView view)
		//	=> handler.UpdateBarBackground();

		//// TODO MAUI: Task Based Mappers?
		//public static void MapTitleIcon(NavigationPageHandler handler, INavigationView view)
		//	=> handler.UpdateTitleIcon();

		//public static void MapTitleView(NavigationPageHandler handler, INavigationView view)
		//	=> handler.UpdateTitleView();
	}
}
