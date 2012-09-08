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
			Title = "Home";

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

			private HomeController parent;
			private MovieDetailsController nextController;

			public DataSource (HomeController parent)
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

				var cell = tableView.DequeueReusableCell (MovieCell.Identifier) as MovieCell;
				if (cell == null)
					cell = new MovieCell(item, tableView);

				cell.UpdateUI();

				return cell;
			}

			public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				var movie = parent.data[indexPath.Row];

				nextController = new MovieDetailsController(movie);
				parent.NavigationController.PushViewController (nextController, true);
			}

			private class MovieCell : UITableViewCell, IImageUpdated {
				public static readonly string Identifier = "moviecell";

				private NowPlaying _movie;
				private UITableView _table;

				private UILabel _title;
				private UIImageView _imageView;

				private static UIFont _textFont = UIFont.BoldSystemFontOfSize(16);

				public MovieCell (NowPlaying movie, UITableView table)
				{
					_movie = movie;
					_table = table;

					Accessory = UITableViewCellAccessory.DisclosureIndicator;

					float height = this.StringSize (GetText (), _textFont, new SizeF(320 - 90, 92)).Height;

					_title = new UILabel(new RectangleF(70, 5, 320 - 90, height));
					_title.BackgroundColor = UIColor.Clear;
					_title.Lines = 0;
					_title.Font = _textFont;
					AddSubview (_title);

					_imageView = new UIImageView(new RectangleF(3, 3, 60, 86));
					_imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
					AddSubview(_imageView);
				}

				private string GetText() {
					string text = _movie.title;
					if (_movie.release_date.HasValue) {
						text += " (" + _movie.release_date.Value.Year.ToString()  + ")";
					}

					return text;
				}

				public void UpdateUI() {

					if (!string.IsNullOrEmpty (_movie.poster_path)) {
						var image = ImageLoader.DefaultRequestImage(Constants.GetImageUrl(_movie.poster_path), this);
						_imageView.Image = image;
					}
					
					_title.Text = GetText ();
				}

				void IImageUpdated.UpdatedImage (Uri uri)
				{
					if (uri == null) return;

					_table.ReloadData ();
					//root.TableView.ReloadRows (new NSIndexPath [] { IndexPath }, UITableViewRowAnimation.None);
				}
			}
		}
	}
}

