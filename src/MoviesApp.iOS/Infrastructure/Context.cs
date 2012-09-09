using System;
using System.IO;
using MonoTouch.UIKit;

namespace MoviesApp.iOS
{
	public static class Context
	{
		private static object _networkActivityLock = new object ();
		private static int _activeNetworkSessions;

		private static UIViewController _root;

		public static bool IsIpad {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad; }
		}
		
		public static bool IsIphone {
			get { return !IsIpad; }
		}
		
		public static readonly bool IsHighRes = UIDevice.CurrentDevice.IsMultitaskingSupported && UIScreen.MainScreen.Scale > 1;

		public static UIViewController Root {
			get { return _root; }
		}

		public static void SetRoot (UIViewController rootController)
		{
			_root = rootController;
		}

		public static UIInterfaceOrientation Orientation {
			get { return UIApplication.SharedApplication.StatusBarOrientation; }
		}

		public static bool IsPortrait ()
		{
			return UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.Portrait || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.PortraitUpsideDown;
		}

		public static bool IsLandscape ()
		{
			return UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight;
		}
		
		public static UIWindow Window {
			get { return UIApplication.SharedApplication.Windows[0]; }
		}

		public static void PushNetworkActivity ()
		{
			lock (_networkActivityLock) {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
				_activeNetworkSessions++;
			}
		}

		public static void PopNetworkActivity ()
		{
			lock (_networkActivityLock) {
				_activeNetworkSessions--;
				if (_activeNetworkSessions == 0)
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			}
		}

		public static string DocumentsFolder {
			get { return Environment.GetFolderPath (Environment.SpecialFolder.Personal); }
		}

		public static string ApplicationFolder {
			get { return Path.Combine (DocumentsFolder, ".."); }
		}

		public static string TempFolder {
			get { return Path.Combine (ApplicationFolder, "tmp"); }
		}

		public static string CacheFolder {
			get { return Path.Combine (ApplicationFolder, "Library/Caches"); }
		}
	}
}

