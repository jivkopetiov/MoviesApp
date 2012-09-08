using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace MoviesApp.iOS
{
	public class LoadingDataSource : UITableViewSource {
			
		public override int RowsInSection (UITableView tableview, int section)
		{
			return 1;
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var cell = new UITableViewCell(UITableViewCellStyle.Default, "loadingcell");	
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			cell.AddSubview(CellActivity(tableView));
			return cell;
		}

		public static UIActivityIndicatorView CellActivity(UITableView tableView) {
			
			var activityView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
			activityView.Frame = new RectangleF((tableView.Frame.Width - 28) / 2, 8, 28, 28);
			activityView.HidesWhenStopped = true;
			activityView.StartAnimating();
			return activityView;
		}
	}
}

