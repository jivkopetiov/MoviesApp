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
			api = new Tmdb("e7ea08e0ed9aba51ea90d5ffe68fa672");
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
				cell.TextLabel.Text = item.title + "(" + item.release_date + ")";

				return cell;
			}
		}
	}
}

