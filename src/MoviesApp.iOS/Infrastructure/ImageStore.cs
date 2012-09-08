using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;

namespace MoviesApp.iOS
{
	public static class ImageStore
	{
		private const int MaxRequests = 6;
		
		// A list of requests that have been issues, with a list of objects to notify.
		private static Dictionary<string, List<Action<string>>> pendingRequests;
		
		// A list of updates that have completed, we must notify the main thread about them.
		private static HashSet<string> queuedUpdates;
		
		// A queue used to avoid flooding the network stack with HTTP requests
		private static Stack<string> requestQueue;
		
		private static NSString nsDispatcher = new NSString ("x");
		
		private static int picDownloaders;
		
		static ImageStore ()
		{
			pendingRequests = new Dictionary<string, List<Action<string>>> ();
			queuedUpdates = new HashSet<string>();
			requestQueue = new Stack<string> ();
		}
		
		private class Util {
			public static void Log(string message) {
				Console.WriteLine ("UTIL: " + message);
			}
		}
		
		public static void UpdateImage(UIImageView imageView, string url) {
			
			var image = ImageCache.TryToGetImageFromCache(url);
			if (image != null)
			{
				SetImage(imageView, image, url);
				Console.WriteLine ("Successfully Updated image");
			}
			else {
				ImageStore.QueueImageRequest(url, delegate {
					var newImage = ImageCache.TryToGetImageFromCache(url);
					if (newImage != null)
						SetImage(imageView, newImage, url);
				});
			}
		}
		
		public static void UpdateButton(UIButton imageView, string url) {
			
			var image = ImageCache.TryToGetImageFromCache(url);
			if (image != null)
			{
				SetImage(imageView, image, url);
				Console.WriteLine ("Successfully Updated image");
			}
			else {
				ImageStore.QueueImageRequest(url, delegate {
					var newImage = ImageCache.TryToGetImageFromCache(url);
					if (newImage != null)
						SetImage(imageView, newImage, url);
				});
			}
		}
		
		private static void SetImage(UIButton imageView, UIImage image, string url) {
			
			if (image.Size.Width >= 1024 || image.Size.Height >= 1024) {
				Console.WriteLine ("Image is larger than 1024 pixels. Will not init the imageView with the image.");
			}
			else {
				imageView.SetBackgroundImage(image, UIControlState.Normal);
			}
		}
		
		private static void SetImage(UIImageView imageView, UIImage image, string url) {
			
			if (image.Size.Width >= 1024 || image.Size.Height >= 1024) {
				Console.WriteLine ("Image is larger than 1024 pixels. Will not init the imageView with the image.");
			}
			else {
				imageView.Image = image;
			}
		}
		
		public static void QueueImageRequest (string url, Action<string> successCallback)
		{
			lock (requestQueue){
				if (pendingRequests.ContainsKey (url)){
					pendingRequests [url].Add (successCallback);
					return;
				}
				
				var slot = new List<Action<string>> (4);
				slot.Add (successCallback);
				pendingRequests [url] = slot;
				
				if (picDownloaders > MaxRequests){
					Util.Log (string.Format("Queuing Image request because {0} >= {1} {2}", requestQueue.Count, MaxRequests, picDownloaders));
					requestQueue.Push (url);
				} else {
					ThreadPool.QueueUserWorkItem (delegate { 
							try {
								StartPicDownload (url); 
							} catch (Exception e){
								Console.WriteLine (e);
							}
						});
				}
			}
		}

		private static void StartPicDownload (string url)
		{
			Interlocked.Increment (ref picDownloaders);
			try {
				_StartPicDownload (url);
			} catch (Exception e){
				Util.Log (string.Format("CRITICAL: should have never happened {0}", e));
			}
			//Util.Log ("Leaving StartPicDownload {0}", picDownloaders);
			Interlocked.Decrement (ref picDownloaders);
		}
		
		private static readonly HashSet<char> _allowedCharactersInFileName = new HashSet<char>("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_-.()");
		
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
		
		static void _StartPicDownload (string url)
		{
			do {
				bool downloaded = false;
				
				Console.WriteLine ("Trying to download image from web: " + url);
				downloaded = ImageCache.TryToDownloadImageFromWeb(url);
				if (!downloaded)
					Console.WriteLine ("Error fetching picture from {0}", url);
				else 
					Console.WriteLine ("Downloaded image from web: " + url);
				
				// Cluster all updates together
				bool doInvoke = false;
				
				lock (requestQueue){
					if (downloaded){
						queuedUpdates.Add (url);
					
						// If this is the first queued update, must notify
						if (queuedUpdates.Count == 1)
							doInvoke = true;
					} else {
						pendingRequests.Remove (url);
					}

					// Try to get more jobs.
					if (requestQueue.Count > 0){
						url = requestQueue.Pop ();
						if (url == null){
							Console.WriteLine ("Dropping request {0} because url is null", url);
							pendingRequests.Remove (url);
						}
					} else {
						Console.WriteLine("Leaving because requestQueue.Count = {0} NOTE: {1}", requestQueue.Count, pendingRequests.Count);
						url = null;
					}
				}	
				if (doInvoke)
					nsDispatcher.BeginInvokeOnMainThread (NotifyImageListeners);
				
			} while (url != null);
		}
		
		static void NotifyImageListeners ()
		{
			lock (requestQueue) {
				foreach (string url in queuedUpdates){
					var list = pendingRequests [url];
					pendingRequests.Remove (url);
					foreach (var pr in list){
						try {
							pr(url);
						} catch (Exception e){
							Console.WriteLine (e);
						}
					}
				}
				queuedUpdates.Clear ();
			}
		}
	}
}
