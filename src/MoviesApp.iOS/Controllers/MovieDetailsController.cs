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
		private TmdbMovieCast _cast;

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
				_cast = _api.GetMovieCast(_movieId);

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

			public override int NumberOfSections (UITableView tableView)
			{
				return 2;
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				if (section == 0)
					return 1;
				else if (section == 1) {
					if (parent._cast.cast.Count == 0)
						return 0;
					else if (parent._cast.cast.Count < 3)
						return parent._cast.cast.Count + 1;
					else 
						return 4;
				}
				else 
					throw new NotSupportedException("section");
			}

			public override float GetHeightForRow (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				if (indexPath.Section == 0) {
					return 92;
				}
				else if (indexPath.Section == 1) {
					if (parent._cast.cast.Count >= 3 && indexPath.Row == 3)
						return 40;
					else 
						return 76;
				}
				else 
					throw new NotSupportedException("section");
			}
			
			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				var movie = parent._data;
				UITableViewCell cell = null;

				if (indexPath.Section == 0) {
					var c = new MovieCell(movie.title, movie.overview, movie.poster_path, tableView);
					c.UpdateUI ();
					cell = c;
				}
				else if (indexPath.Section == 1) {
					if (parent._cast.cast.Count >= 3 && indexPath.Row == 3) {
						cell = new UITableViewCell(UITableViewCellStyle.Default, "cell");
						cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
						cell.TextLabel.Text = "All cast";
					}
					else {
						var person = parent._cast.cast[indexPath.Row];
						cell = new UITableViewCell(UITableViewCellStyle.Subtitle, "cell");
						cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
						cell.TextLabel.Text = person.name;
						cell.DetailTextLabel.Text = person.character;
						cell.ImageView.Image = ImageLoader.DefaultRequestImage(Constants.GetImageUrl (person.profile_path), this);
					}
				}
				else 
					throw new NotSupportedException("section");

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

