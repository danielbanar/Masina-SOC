using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Webkit;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Android.Net;
using System;
using System.Net;
using static Android.Views.View;
using Masina;

namespace App1
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			//Window.RequestFeature(WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.activity_main);
			EditText ipbox = FindViewById<EditText>(Resource.Id.textbox_IP);
			string ip = ipbox.Text;
			Button reload = FindViewById<Button>(Resource.Id.button_reload);
			WebView video = FindViewById<WebView>(Resource.Id.webview);
			Button w = FindViewById<Button>(Resource.Id.button_w);
			Button a = FindViewById<Button>(Resource.Id.button_a);
			Button s = FindViewById<Button>(Resource.Id.button_s);
			Button d = FindViewById<Button>(Resource.Id.button_d);
			SeekBar speedbar = FindViewById<SeekBar>(Resource.Id.speedbar);
			CheckBox led = FindViewById<CheckBox>(Resource.Id.led);
			SeekBar r = FindViewById<SeekBar>(Resource.Id.r);
			SeekBar g = FindViewById<SeekBar>(Resource.Id.g);
			SeekBar b = FindViewById<SeekBar>(Resource.Id.b);
			HttpClient httpClient = new HttpClient();
			httpClient.Timeout = new TimeSpan(0, 0, 3);
			w.Touch += (object sender, TouchEventArgs e) =>
			{
				if (e.Event.Action == MotionEventActions.Down)
					httpClient.GetAsync($"http://{ip}:8000/w");
				else if (e.Event.Action == MotionEventActions.Up)
					httpClient.GetAsync($"http://{ip}:8000/W");
			};
			s.Touch += (object sender, TouchEventArgs e) =>
			{
				if (e.Event.Action == MotionEventActions.Down)
					httpClient.GetAsync($"http://{ip}:8000/s");
				else if (e.Event.Action == MotionEventActions.Up)
					httpClient.GetAsync($"http://{ip}:8000/S");
			};
			a.Touch += (object sender, TouchEventArgs e) =>
			{
				if (e.Event.Action == MotionEventActions.Down)
					httpClient.GetAsync($"http://{ip}:8000/a");
				else if (e.Event.Action == MotionEventActions.Up)
					httpClient.GetAsync($"http://{ip}:8000/A");
			};
			d.Touch += (object sender, TouchEventArgs e) =>
			{
				if (e.Event.Action == MotionEventActions.Down)
					httpClient.GetAsync($"http://{ip}:8000/d");
				else if (e.Event.Action == MotionEventActions.Up)
					httpClient.GetAsync($"http://{ip}:8000/D");
			};
			led.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
			{
				if(e.IsChecked)
					httpClient.GetAsync($"http://{ip}:8000/led");
				else
					httpClient.GetAsync($"http://{ip}:8000/LED");
			};
			speedbar.ProgressChanged += (sender, e) => { httpClient.GetAsync($"http://{ip}:8000/speed/{e.Progress}"); };
			ipbox.TextChanged += (sender, e) => { ip = ipbox.Text; };
			r.ProgressChanged += (sender, e) => { httpClient.GetAsync($"http://{ip}:8000/r/{e.Progress}"); };
			g.ProgressChanged += (sender, e) => { httpClient.GetAsync($"http://{ip}:8000/g/{e.Progress}"); };
			b.ProgressChanged += (sender, e) => { httpClient.GetAsync($"http://{ip}:8000/b/{e.Progress}"); };
			reload.Click += (sender, e) => {
				video.Settings.LoadWithOverviewMode = true;
				video.Settings.UseWideViewPort = true;
				video.LoadUrl($"http://{ip}:5000/stream.mjpg");
				video.Reload();
			};
		}
	}
}