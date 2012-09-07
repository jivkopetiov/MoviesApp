using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatTmdb.V3;

namespace MoviesApp.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var api = new Tmdb("e7ea08e0ed9aba51ea90d5ffe68fa672");
            var result = api.GetPopularMovies(1);
            foreach (var a in result.results)
            {
                Console.WriteLine(a.title);
            }
        }
    }
}
