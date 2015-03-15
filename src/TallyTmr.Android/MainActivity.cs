using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Android.Support.V7.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;


namespace TallyTmr.Android
{
	[Activity (Label = "TallyTmr.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : ActionBarActivity
	{
		int _count = 0;
		TextView _setLabel;
		TextView _timeLabel;
		GridLayout _grid;
		TimeSpan _interval = new TimeSpan(0, 1, 30);
		TimeSpan _totalTime;
		string _timeFormat = @"mm\:ss\,f";
		bool _timerRunning = false;
		NumberPicker _minutePicker;
		//bool _isFlipped;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			InitalizeLayout ();

			if (_totalTime == TimeSpan.Zero)
				_totalTime = _totalTime.Add(_interval);
		}

		private void InitalizeLayout(){
			_grid = FindViewById<GridLayout> (Resource.Id.grid);
			_setLabel = FindViewById<TextView> (Resource.Id.setsLabel);
			_timeLabel = FindViewById<TextView> (Resource.Id.timeLabel);
			_minutePicker = FindViewById<NumberPicker> (Resource.Id.numberPickerMinute);
			_minutePicker.MinValue = 0;
			_minutePicker.MaxValue = 59;
			_minutePicker.Value = Convert.ToInt32(Math.Floor(_interval.TotalMinutes));
			_timeLabel.Text = _interval.ToString (_timeFormat);

			var toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
			//Toolbar will now take on default Action Bar characteristics
			SetSupportActionBar (toolbar);

			//You can now use and reference the ActionBar
			SupportActionBar.Title = "TallyTimer";
		}

		public override bool OnTouchEvent (MotionEvent e)
		{
			if (e.Action == MotionEventActions.Down) {
				_setLabel.Text = string.Format ("{0}", _count++);
				_timerRunning = !_timerRunning;
				StartTimer ();
			}
			return base.OnTouchEvent (e);
		}

		public void StopTimer(){
			_timerRunning = false;
		}

		public async void StartTimer ()
		{
			var interval = new TimeSpan (0, 0, 0, 0, 300);
			while (_timerRunning) {
				await Task.Delay(interval);
				RunOnUiThread (() => {
					_totalTime -= interval;
					_timeLabel.Text = _totalTime.ToString (_timeFormat);
				});
			}
		}

		private void ToggleTimer(){
			if (_timerRunning) {
				_timerRunning = false;
			} else {
				_timerRunning = true;
				StartTimer ();
			}
		}

		private void Reset(){
			_timerRunning = false;
			_timeLabel.Text = _interval.ToString (_timeFormat);
			_totalTime = _totalTime.Add(_interval);
			_count = 0;
		}

		void EditTime ()
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this);
			builder.SetTitle ("Tes");
			Dialog dialog = builder.Create ();
		}

		/// <Docs>The options menu in which you place your items.</Docs>
		/// <returns>To be added.</returns>
		/// <summary>
		/// This is the menu for the Toolbar/Action Bar to use
		/// </summary>
		/// <param name="menu">Menu.</param>
		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.home, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{	
			switch (item.TitleFormatted.ToString()) {
			case "Start":
			case "Pause":
				{
					ToggleTimer ();
					break;
				}
			case "Edit":
				{
					EditTime ();
					break;
				}
			case "Reset":
				{
					Reset ();
					break;
				}
			}
			//Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
			return base.OnOptionsItemSelected (item);
		}
	}
}


