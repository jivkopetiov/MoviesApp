using System;
using MonoTouch.UIKit;
using WatTmdb.V3;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;

namespace MoviesApp.iOS
{
	public class MoviesController : UIViewController
	{
		Tmdb api;

		private UITableView table;
		private List<NowPlaying> data;

		public MoviesController ()
		{
			Title = "Now Playing";

			api = new Tmdb(Constants.ApiKey);
		}

		public override void ViewDidLoad ()
		{
			table = new UITableView(new RectangleF(0, 0, View.Frame.Width, View.Frame.Height - 44), UITableViewStyle.Grouped);
			table.Source = new LoadingDataSource();
			View.Add (table);

			ThreadPool.QueueUserWorkItem (delegate {
				var result = api.GetNowPlayingMovies(1);
				data = result.results;

				InvokeOnMainThread(delegate {
					table.Source = new DataSource(this);
					table.ReloadData ();
				});
			});
		}

		public override void ViewWillAppear (bool animated)
		{
			if (table != null)
				table.DeselectRow(table.IndexPathForSelectedRow, true);
		}

		private class DataSource : UITableViewSource {

			private MoviesController parent;
			private MovieDetailsController nextController;

			public DataSource (MoviesController parent)
			{
				this.parent = parent;
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return parent.data.Count;
			}

			public override float GetHeightForRow (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				return 92;
			}

			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				var item = parent.data[indexPath.Row];

				string text = item.title;
				if (item.release_date.HasValue) {
					text += " (" + item.release_date.Value.Year.ToString()  + ")";
				}

				var cell = tableView.DequeueReusableCell (MovieCell.Identifier) as MovieCell;
				if (cell == null) {
					cell = new MovieCell(text, null, item.poster_path, tableView);
				}

				cell.UpdateUI();

				return cell;
			}

			public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				var movie = parent.data[indexPath.Row];

				nextController = new MovieDetailsController(movie);
				parent.NavigationController.PushViewController (nextController, true);
			}
		}
	}
}

