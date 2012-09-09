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
using System.Threading;

namespace MoviesApp.Droid
{
    [Activity]
    public class MoviesActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            this.Title = "Now Playing Movies";

            base.OnCreate(bundle);

            string type = Intent.GetStringExtra("Type");

            var api = new Tmdb("e7ea08e0ed9aba51ea90d5ffe68fa672");

            var layout = new RelativeLayout(this);
            var progressBar = new ProgressBar(this, null, Android.Resource.Attribute.ProgressBarStyleSmall);
            layout.AddView(progressBar);

            SetContentView(layout);

            ThreadPool.QueueUserWorkItem(delegate
            {
                var result = api.GetNowPlayingMovies(1);
                var names = result.results.Select(r => r.title).ToList();

                RunOnUiThread(delegate
                {
                    var listView = new ListView(this);
                    listView.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, names);
                    progressBar.Visibility = ViewStates.Invisible;
                    layout.AddView(listView);
                    layout.RemoveView(progressBar);
                });
            });
        }
    }
}