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
		private UIViewController homeController;
		private UINavigationController navController;


		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			window.MakeKeyAndVisible ();

			homeController = new HomeController();
			navController = new UINavigationController(homeController);
			window.RootViewController = navController;

			return true;
		}
	}
}

