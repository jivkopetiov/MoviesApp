using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;

namespace MoviesApp.iOS
{
	public static class StringExtensions
	{
		public static string CollapseWhitespace(this string self) {
			return Regex.Replace(self, @"\s+", " ");
		}
		
		public static string RemoveNonAsciiChars(string input)
        {
            return ReplaceNonAsciiChars(input, "");
        }

        public static string ReplaceNonAsciiChars(string input, string replacement)
        {
            return Regex.Replace(input, @"[^\u0000-\u007F]", replacement);
        }
		
		public static bool Contains(this string source, string toCheck, StringComparison comparison) {
			return source.IndexOf(toCheck, comparison) >= 0;
		}
		
		public static string RemoveHtmlTags(string input)
        {
            return Regex.Replace(input, @"<[a-zA-Z\/][^>]*>", "");
        }
	}
}

