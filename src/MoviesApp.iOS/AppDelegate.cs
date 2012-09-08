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

			var api = new WatTmdb.V3.Tmdb("e7ea08e0ed9aba51ea90d5ffe68fa672");
			var result = api.GetNowPlayingMovies(1);
			foreach(var m in result.results)
				Console.WriteLine (m.title);

			return true;
		}
	}
}

