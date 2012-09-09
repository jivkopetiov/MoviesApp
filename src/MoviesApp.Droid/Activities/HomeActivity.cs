using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MoviesApp.Droid
{
    [Activity(Label = "Movies App", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;
            SetContentView(layout);

            var nowPlaying = new Button(this);
            nowPlaying.Text = "Now Playing";
            nowPlaying.Click += delegate
            {
                var intent = new Intent(this, typeof(MoviesActivity));
                intent.PutExtra("Type", "NowPlaying");
                StartActivity(intent);
            };

            layout.AddView(nowPlaying);
        }
    }
}

