using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WatTmdb.V3;

namespace MoviesApp.Droid
{
    [Activity]
    public class MoviesActivity : ListActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            this.Title = "Now Playing Movies";

            base.OnCreate(bundle);

            string type = Intent.GetStringExtra("Type");

            var api = new Tmdb("e7ea08e0ed9aba51ea90d5ffe68fa672");

            var result = api.GetNowPlayingMovies(1);

            var names = result.results.Select(r => r.title).ToList();

            ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, names);
        }
    }
}