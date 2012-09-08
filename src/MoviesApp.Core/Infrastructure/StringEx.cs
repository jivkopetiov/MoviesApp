using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoviesApp.Core
{
    public static class StringEx
    {
        public static string JoinStrings<T>(this IEnumerable<T> source,
                                                Func<T, string> projection, string separator)
        {
            var builder = new StringBuilder();
            bool first = true;
            foreach (T element in source)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append(separator);
                }
                builder.Append(projection(element));
            }
            return builder.ToString();
        }

        public static string JoinStrings<T>(this IEnumerable<T> source, string separator)
        {
            return JoinStrings(source, t => t.ToString(), separator);
        }
    }
}
