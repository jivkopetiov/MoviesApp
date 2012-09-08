using System;
using MonoTouch.UIKit;
using WatTmdb.V3;
using System.Drawing;
using System.Threading;

namespace MoviesApp.iOS
{
	public class MovieDetailsController : UIViewController
	{
		private UITableView _table;
		private Tmdb _api;
		private int	_movieId;
		private TmdbMovie _data;

		public MovieDetailsController (NowPlaying movie)
		{
			_movieId = movie.id;

			_api = new Tmdb(Constants.ApiKey);

			Title = movie.title;
		}

		public override void ViewDidLoad ()
		{
			_table = new UITableView(new RectangleF(0, 0, View.Frame.Width, View.Frame.Height - 44), UITableViewStyle.Grouped);
			_table.Source = new LoadingDataSource();
			View.Add (_table);
			
			ThreadPool.QueueUserWorkItem (delegate {
				_data = _api.GetMovieInfo(_movieId);
				
				InvokeOnMainThread(delegate {
					_table.Source = new DataSource(this);
					_table.ReloadData ();
				});
			});
		}
		
		public override void ViewWillAppear (bool animated)
		{
			if (_table != null)
				_table.DeselectRow(_table.IndexPathForSelectedRow, true);
		}
		
		private class DataSource : UITableViewSource, IImageUpdated {
			
			private MovieDetailsController parent;
			
			public DataSource (MovieDetailsController parent)
			{
				this.parent = parent;
			}
			
			public override int RowsInSection (UITableView tableview, int section)
			{
				return 2;
			}

			public override float GetHeightForRow (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				switch(indexPath.Row)
				{
					case 0: return 92;
					default: return 44;
				}
			}
			
			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				var movie = parent._data;
				UITableViewCell cell = null;

				switch(indexPath.Row) {

				case 0:
					cell = new UITableViewCell(UITableViewCellStyle.Default, "cell");
					cell.TextLabel.Text = movie.title;
					cell.ImageView.Image = ImageLoader.DefaultRequestImage(Constants.GetImageUrl(movie.poster_path), this);
					break;
				case 1:
					cell = new UITableViewCell(UITableViewCellStyle.Subtitle, "cell");
					cell.TextLabel.Text = "Overview";
					cell.DetailTextLabel.Text = movie.overview;
					break;
				default:
					throw new InvalidOperationException("Invalid row index");
				}

				return cell;
			}

			void IImageUpdated.UpdatedImage(Uri uri)
			{
				if (uri == null) return;
				parent._table.ReloadData ();
			}

			public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				tableView.DeselectRow (tableView.IndexPathForSelectedRow, true);
			}
		}
	}
}

