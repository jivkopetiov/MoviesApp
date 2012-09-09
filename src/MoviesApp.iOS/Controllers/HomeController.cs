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
						_next = new MoviesController(MovieType.NowPlaying);
						NavigationController.PushViewController (_next, true);
					}),
					new StringElement("Upcoming Movies", delegate {
						_next = new MoviesController(MovieType.Upcoming);
						NavigationController.PushViewController (_next, true);
					}),
					new StringElement("Top Rated Movies", delegate {
						_next = new MoviesController(MovieType.TopRated);
						NavigationController.PushViewController (_next, true);
					}),
					new StringElement("Similar Movies", delegate {
						_next = new MoviesController(MovieType.Similar, 77948);
						NavigationController.PushViewController (_next, true);
					}),
					new StringElement("The Dark Knight Rises", delegate {
						_next = new MovieDetailsController(49026, "The Dark Knight Rises");
						NavigationController.PushViewController (_next, true);
					}),
				}
			});

			return root;
		}
	}
}

