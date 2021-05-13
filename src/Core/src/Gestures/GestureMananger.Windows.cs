#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Microsoft.Maui
{
	public class GestureMananger : IGestureManager
	{
		readonly NotifyCollectionChangedEventHandler _collectionChangedHandler;

		bool _isDisposed;
		IViewHandler? _handler;

		public GestureMananger()
		{
			_collectionChangedHandler = OnGestureRecognizersCollectionChanged;
		}

		IView? VirtualView => _handler?.VirtualView;

		FrameworkElement? NativeView => (FrameworkElement?)_handler?.NativeView;

		public void SetViewHandler(IViewHandler handler)
		{
			if (_isDisposed)
				throw new ObjectDisposedException(null);

			_handler = handler ?? throw new ArgumentNullException(nameof(handler));
			
			if (VirtualView != null)
			{
				if (VirtualView is IView view)
				{
					var gestureRecognizers = (ObservableCollection<IGestureRecognizer>)view.GestureRecognizers;
					gestureRecognizers.CollectionChanged += _collectionChangedHandler;
				}
			}

			UpdatingGestureRecognizers();
		}

		public void Dispose() 
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			_isDisposed = true;

			if (!disposing)
				return;

			if (VirtualView != null)
			{
				if (VirtualView is IView view)
				{
					var oldRecognizers = (ObservableCollection<IGestureRecognizer>)view.GestureRecognizers;
					oldRecognizers.CollectionChanged -= _collectionChangedHandler;
				}
			}

			if (NativeView != null)
			{
				NativeView.Tapped -= OnTap;
				NativeView.DoubleTapped -= OnDoubleTap;
			}
		}

		void UpdatingGestureRecognizers()
		{
			if (NativeView == null)
				return;

			IList<IGestureRecognizer>? gestures = VirtualView?.GestureRecognizers;

			if (gestures == null)
				return;

			var children = VirtualView?.GetChildElements(Point.Zero);
			IList<ITapGestureRecognizer>? childGestures = children?.GetChildGesturesFor<ITapGestureRecognizer>().ToList();

			if (gestures.GetGesturesFor<ITapGestureRecognizer>(g => g.NumberOfTapsRequired == 1).Any()
				|| children?.GetChildGesturesFor<ITapGestureRecognizer>(g => g.NumberOfTapsRequired == 1).Any() == true)
			{
				NativeView.Tapped += OnTap;
			}
		
			if (gestures.GetGesturesFor<ITapGestureRecognizer>(g => g.NumberOfTapsRequired == 1 || g.NumberOfTapsRequired == 2).Any()
				|| children?.GetChildGesturesFor<ITapGestureRecognizer>(g => g.NumberOfTapsRequired == 1 || g.NumberOfTapsRequired == 2).Any() == true)
			{
				NativeView.DoubleTapped += OnDoubleTap;
			}
		}

		void OnTap(object? sender, TappedRoutedEventArgs e)
		{
			if (VirtualView is not IView view)
				return;

			if (view == null)
				return;

			var tapPosition = e.GetPosition(NativeView);
			var children = view.GetChildElements(new Point(tapPosition.X, tapPosition.Y));

			if (children != null)
				foreach (var recognizer in children.GetChildGesturesFor<ITapGestureRecognizer>(g => g.NumberOfTapsRequired == 1))
				{
					recognizer.Tapped(view);
					e.Handled = true;
				}

			if (e.Handled)
				return;

			IEnumerable<ITapGestureRecognizer> tapGestures = view.GestureRecognizers.GetGesturesFor<ITapGestureRecognizer>(g => g.NumberOfTapsRequired == 1);
			foreach (var recognizer in tapGestures)
			{
				recognizer.Tapped(view);
				e.Handled = true;
			}
		}

		void OnDoubleTap(object? sender, DoubleTappedRoutedEventArgs e)
		{
			if (VirtualView is not IView view)
				return;

			if (view == null)
				return;

			var tapPosition = e.GetPosition(NativeView);
			var children = view.GetChildElements(new Point(tapPosition.X, tapPosition.Y));

			if (children != null)
				foreach (var recognizer in children.GetChildGesturesFor<ITapGestureRecognizer>(g => g.NumberOfTapsRequired == 1 || g.NumberOfTapsRequired == 2))
				{
					recognizer.Tapped(view);
					e.Handled = true;
				}

			if (e.Handled)
				return;

			IEnumerable<ITapGestureRecognizer> doubleTapGestures = view.GestureRecognizers.GetGesturesFor<ITapGestureRecognizer>(g => g.NumberOfTapsRequired == 1 || g.NumberOfTapsRequired == 2);
			foreach (ITapGestureRecognizer recognizer in doubleTapGestures)
			{
				recognizer.Tapped(view);
				e.Handled = true;
			}
		}

		void OnGestureRecognizersCollectionChanged(object? sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			UpdatingGestureRecognizers();
		}
	}
}