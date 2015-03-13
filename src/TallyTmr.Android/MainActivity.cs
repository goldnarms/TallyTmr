using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Windows.Threading;

namespace TallyTmr.Android
{
	[Activity (Label = "TallyTmr.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 0;
		TextView _setLabel;
		TextView _timeLabel;
		GridLayout _grid;
		int _intervalInSeconds = 90;
		TimeSpan _totalTime;
		bool _isFlipped;
		readonly System.Timers.Timer _timer = new System.Timers.Timer{ Interval = TimeSpan.FromSeconds(.1) };

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			InitalizeLayout ();

			if (_totalTime == TimeSpan.Zero)
				_totalTime = _totalTime.Add(new TimeSpan(0, 0, 0, _intervalInSeconds));

			_timer.Elapsed += TimerElapsed;
		}

		private void InitalizeLayout(){
			_grid = FindViewById<GridLayout> (Resource.Id.grid);
			_setLabel = FindViewById<TextView> (Resource.Id.setsLabel);
			_timeLabel = FindViewById<TextView> (Resource.Id.timeLabel);
		}

		public override bool OnTouchEvent (MotionEvent e)
		{
			_setLabel.Text = string.Format ("{0}", count++);
			return base.OnTouchEvent (e);
		}

		protected void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			RunOnUiThread (() => {
				var seconds = new TimeSpan (0, 0, 0, 0, 1);
				_totalTime -= seconds;
				_timeLabel.Text = _totalTime.ToString ("N0");
			});
		}
	}
}


