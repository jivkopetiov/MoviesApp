using System;

namespace MoviesApp.iOS
{
	public class Constants
	{
		public static readonly string BaseImageUrl = "http://cf2.imgobject.com/t/p/";

		public static string GetImageUrl(string relativeUrl) {
			return BaseImageUrl + "w92" + relativeUrl;
		}

		public static readonly string ApiKey = "e7ea08e0ed9aba51ea90d5ffe68fa672";
	}
}

