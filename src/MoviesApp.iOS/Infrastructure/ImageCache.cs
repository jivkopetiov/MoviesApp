using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using MonoTouch.UIKit;
using System.Runtime.InteropServices;
using System.Drawing;

namespace MoviesApp.iOS
{
	public class ImageCache
	{
		private static string _imagesPath;
		
		static ImageCache ()
		{
			_imagesPath = Path.Combine(Context.DocumentsFolder, "images");
			
			if (!Directory.Exists(_imagesPath))
				Directory.CreateDirectory(_imagesPath);
		}
		
		public static UIImage TryToGetImageFromCache(string imageUrl) {
			string localPath = Path.Combine (_imagesPath, UrlToCacheKey (imageUrl));
			
			if (File.Exists (localPath))
			{
				try {
					var image = UIImage.FromFile (localPath);
					if (image == null)
						return null;
					
					return image;
				}
				catch (Exception ex) {
					Console.WriteLine ("Corrupt image at " + localPath + ", " + ex.ToString());
					return null;
				}
			}
			else 
				return null;
		}
		
		/// <summary>
		/// It takes some time to resize the image especially if it is large
		/// </summary>
		public static UIImage ScaleImage (SizeF newSize, UIImage originalImage)
	    {
	        UIGraphics.BeginImageContext (newSize);
	        var context = UIGraphics.GetCurrentContext ();
	        context.TranslateCTM (0, newSize.Height);
	        context.ScaleCTM (1f, -1f);
	
	        context.DrawImage (new RectangleF (0, 0, newSize.Width, newSize.Height), originalImage.CGImage);
	
	        var scaledImage = UIGraphics.GetImageFromCurrentImageContext();
	        UIGraphics.EndImageContext();
	
	        return scaledImage;         
	    }
		
		public static bool DownloadImageFromWebIfNotExistsInCache (Uri imageUrl)
		{
			string localPath = Path.Combine (_imagesPath, UrlToCacheKey (imageUrl));
			
			if (File.Exists(localPath)) {
				
				var image = UIImage.FromFile(localPath);
				if (image == null)
					return false;
				
				var size = image.Size;
				if (size.IsEmpty)
					return false;
				
				return false;
			}
			
			var client = new WebClient ();
			client.DownloadFile (imageUrl, localPath);
			
			return true;
		}
		
		public static UIImage DownloadImageFromWeb (Uri imageUrl)
		{
			string localPath = Path.Combine (_imagesPath, UrlToCacheKey (imageUrl));
			
			var client = new WebClient ();
			client.DownloadFile (imageUrl, localPath);
			
			var image = UIImage.FromFile (localPath);
			if (image == null)
			{
				Console.WriteLine ("Corrupted image, failed to download from " + imageUrl);
				return null;
			}
			
			return image;
		}
		
		public static bool TryToDownloadImageFromWeb(string url) {
			
			var buffer = new byte [4*1024];
			string filepath = Path.Combine (_imagesPath, UrlToCacheKey (url));
			
			try {
				using (var file = new FileStream (filepath, FileMode.Create, FileAccess.Write, FileShare.Read)) {
	                	var req = WebRequest.Create (url) as HttpWebRequest;
					
	                using (var resp = req.GetResponse()) {
						using (var s = resp.GetResponseStream()) {
							int n;
							while ((n = s.Read (buffer, 0, buffer.Length)) > 0){
								file.Write (buffer, 0, n);
	                        }
						}
	                }
				}
				return true;
			} catch (Exception e) {
				Console.WriteLine ("Problem with {0} {1}", url, e);
				return false;
			}
		}
		
		private static readonly HashSet<char> _allowedCharactersInFileName = new HashSet<char>("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_-.()");
		
		private static string UrlToCacheKey(Uri url) {
			return UrlToCacheKey(url.AbsoluteUri);
		}
		
		private static string UrlToCacheKey (string url)
		{
			string normalized = url;
			normalized = StringExtensions.RemoveNonAsciiChars(normalized);
			normalized = normalized.Trim().Replace(" ", "-");
			
			string result = "";

            foreach (char ch in normalized)
            {
                if (_allowedCharactersInFileName.Contains(ch))
                    result += ch;
            }
			
            return result;
		}
	}
}
