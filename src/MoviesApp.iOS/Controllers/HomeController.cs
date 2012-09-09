using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;

namespace MoviesApp.iOS
{
	public class HomeController : UIViewController
	{
		private DialogViewController dvc;
		private UIViewController _next;
		private UISearchBar _search;

		public override void ViewDidLoad ()
		{
			Title = "Home";
			dvc = new DialogViewController(CreateRootElement(), true);
			dvc.View.Frame = new System.Drawing.RectangleF(0, 44, View.Frame.Width, View.Frame.Height);
			View.Add(dvc.View);

			_search = new UISearchBar(new RectangleF(0, 0, View.Frame.Width, 44));

			_search.SearchButtonClicked += delegate {
				if (string.IsNullOrEmpty (_search.Text)) return;

				_search.ResignFirstResponder ();

				_next = new MoviesController(MovieType.Search, null, _search.Text);
				NavigationController.PushViewController(_next, true);
			};

			View.Add(_search);
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

