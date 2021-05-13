using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Microsoft.Maui.Graphics;
using AView = Android.Views.View;

namespace Microsoft.Maui
{
	public class GestureManager : IGestureManager
	{
		readonly Lazy<TapGestureDetector> _tapDetector;

		IViewHandler? _handler;
		bool _disposed;

		public GestureManager()
		{
			_tapDetector = new Lazy<TapGestureDetector>(InitializeTapDetector);
		}

		IView? VirtualView => _handler?.VirtualView;

		AView? NativeView => (AView?)_handler?.NativeView;

		public void SetViewHandler(IViewHandler handler)
		{
			_handler = handler;
		}

		public bool OnTouchEvent(MotionEvent e)
		{
			if (NativeView == null)
			{
				return false;
			}

			if (!DetectorsValid())
			{
				return false;
			}

			var eventConsumed = _tapDetector.Value.OnTouchEvent(e);

			return eventConsumed;
		}
				
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			_disposed = true;

			if (disposing)
			{
				if (_tapDetector.IsValueCreated)
				{
					_tapDetector.Value.Dispose();
				}

				_handler = null;
			}
		}

		bool DetectorsValid()
		{
			// Make sure we're not testing for gestures on old motion events after our 
			// detectors have already been disposed

			if (_tapDetector.IsValueCreated && _tapDetector.Value.Handle == IntPtr.Zero)
			{
				return false;
			}

			return true;
		}

		TapGestureDetector InitializeTapDetector()
		{
			var context = NativeView?.Context;

			var listener = new InnerGestureListener(new TapGestureHandler(() => VirtualView!, () =>
			{
				if (VirtualView is IView view)
					return view.GetChildElements(Point.Zero) ?? new List<IGestureView>();

				return new List<IGestureView>();
			}));

			return new TapGestureDetector(context, listener);
		}

		public class TapGestureDetector : GestureDetector
		{
			InnerGestureListener? _listener;

			public TapGestureDetector(Context? context, InnerGestureListener listener) : base(context, listener)
			{
				_listener = listener;
			}

			public override bool OnTouchEvent(MotionEvent? ev)
			{
				if (base.OnTouchEvent(ev))
					return true;

				return false;
			}

			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);

				if (disposing)
				{
					if (_listener != null)
					{
						_listener.Dispose();
						_listener = null;
					}
				}
			}
		}
	}
}