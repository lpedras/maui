﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Compatibility.ControlGallery.Issues
{
#if UITEST
	[NUnit.Framework.Category(Compatibility.UITests.UITestCategories.Bugzilla)]
#endif
	[Issue(IssueTracker.Bugzilla, 29229, "ListView crash on Windows Phone", PlatformAffected.WinPhone, NavigationBehavior.PushModalAsync)]
	public class Bugzilla29229
		: NavigationPage
	{
		public Bugzilla29229()
		{
			var absLayout = new AbsoluteLayout();
			absLayout.BackgroundColor = Colors.Red;
			absLayout.Scale = 1;

			var cPx = new ListView();
			//Point ptx = Point.Zero;
			cPx.ItemTapped += delegate (object sender, ItemTappedEventArgs e)
			{
			};
			cPx.ClassId = "weather";
			cPx.Layout(new Graphics.Rectangle(0, 0, 480, 768));
			absLayout.Children.Add(cPx, new Graphics.Rectangle(cPx.X, cPx.Y, cPx.Width, cPx.Height), AbsoluteLayoutFlags.None);
			absLayout.LowerChild(cPx);
			PushAsync(new ContentPage { Content = absLayout });
		}
	}
}
