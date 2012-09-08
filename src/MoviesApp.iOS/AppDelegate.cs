using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MoviesApp.iOS
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}

		private UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			window.MakeKeyAndVisible ();
			window.RootViewController = new HomeController();

			return true;
		}
	}
}

