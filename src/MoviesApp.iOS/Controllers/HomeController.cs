using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace MoviesApp.iOS
{
	public class HomeController : UIViewController
	{
		private DialogViewController dvc;
		private UIViewController _next;

		public HomeController () 
		{   
			Title = "Home";
			dvc = new DialogViewController(CreateRootElement(), true);
			View.AddSubview (dvc.View);
		}

		private RootElement CreateRootElement() {
			var root = new RootElement("Movies App");

			root.Add (new[] {
				new Section() {
					new StringElement("Now Playing Movies", delegate {
						_next = new MoviesController();
						NavigationController.PushViewController (_next, true);
					}),
					new StringElement("Upcoming Movies", delegate {
						_next = new MoviesController();
						NavigationController.PushViewController (_next, true);
					})
				}
			});

			return root;
		}
	}
}

