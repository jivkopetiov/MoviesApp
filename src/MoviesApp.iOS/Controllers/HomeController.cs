using System;
using MonoTouch.UIKit;
using WatTmdb.V3;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;

namespace MoviesApp.iOS
{
	public class HomeController : UIViewController
	{
		Tmdb api;

		private UITableView table;
		private List<NowPlaying> data;

		public HomeController ()
		{
			api = new Tmdb(Constants.ApiKey);
		}

		public override void ViewDidLoad ()
		{
			View.BackgroundColor = UIColor.Blue;

			table = new UITableView(new RectangleF(0, 0, View.Frame.Width, View.Frame.Height), UITableViewStyle.Grouped);
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

		private class DataSource : UITableViewSource {

			private HomeController parent;

			public DataSource (HomeController parent)
			{
				this.parent = parent;
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return parent.data.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				var item = parent.data[indexPath.Row];

				var cell = new UITableViewCell(UITableViewCellStyle.Value1, "cell");

				string text = item.title;
				if (item.release_date.HasValue) {
					text += " (" + item.release_date.Value.Year.ToString()  + ")";
				}

				if (!string.IsNullOrEmpty (item.poster_path))
					ImageStore.UpdateImage(cell.ImageView, Constants.GetImageUrl(item.poster_path));

				cell.TextLabel.Text = text;

				return cell;
			}
		}
	}
}

