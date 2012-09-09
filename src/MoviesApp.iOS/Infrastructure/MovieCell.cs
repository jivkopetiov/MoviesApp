using System;
using MonoTouch.UIKit;
using WatTmdb.V3;
using System.Drawing;
using MonoTouch.Dialog.Utilities;

namespace MoviesApp.iOS
{
	public class MovieCell : UITableViewCell, IImageUpdated {
		public static readonly string Identifier = "moviecell";
		
		private string title;
		private string description;
		private string imageUrl;


		private UITableView _table;
		private UILabel _titleLabel;
		private UILabel _detailsLabel;
		private UIImageView _imageView;
		
		private static UIFont _titleFont = UIFont.BoldSystemFontOfSize(16);
		private static UIFont _descriptionFont = UIFont.SystemFontOfSize(14);
		
		public MovieCell (string title, string description, string imageUrl, UITableView table)
		{
			this.title = title;
			this.description = description;
			this.imageUrl = imageUrl;
			_table = table;
			
			Accessory = UITableViewCellAccessory.DisclosureIndicator;
			
			float titleHeight = StringSize (title, _titleFont, new SizeF(320 - 90, 50)).Height;
			
			_titleLabel = new UILabel(new RectangleF(70, 5, 320 - 90, titleHeight));
			_titleLabel.BackgroundColor = UIColor.Clear;
			_titleLabel.Lines = 0;
			_titleLabel.Font = _titleFont;
			AddSubview (_titleLabel);

			if (!string.IsNullOrEmpty(description)) {
				float descriptionHeight = StringSize (description, _descriptionFont, new SizeF(320 - 90, 92 - titleHeight - 10)).Height;

				_detailsLabel = new UILabel(new RectangleF(70, titleHeight + 10, 320 - 90, descriptionHeight));
				_detailsLabel.BackgroundColor = UIColor.Clear;
				_detailsLabel.TextColor = UIColor.Gray;
				_detailsLabel.Lines = 0;
				_detailsLabel.Font = _descriptionFont;
				AddSubview (_detailsLabel);
			}
			
			_imageView = new UIImageView(new RectangleF(3, 3, 60, 86));
			_imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			AddSubview(_imageView);
		}

		public void UpdateUI() {
			
			if (!string.IsNullOrEmpty (imageUrl)) {
				var image = ImageLoader.DefaultRequestImage(Constants.GetImageUrl(imageUrl), this);
				_imageView.Image = image;
			}
			
			_titleLabel.Text = title;
			if (!string.IsNullOrEmpty (description))
				_detailsLabel.Text = description;
		}
		
		void IImageUpdated.UpdatedImage (Uri uri)
		{
			if (uri == null) return;
			
			_table.ReloadData ();
			//root.TableView.ReloadRows (new NSIndexPath [] { IndexPath }, UITableViewRowAnimation.None);
		}
	}
}

