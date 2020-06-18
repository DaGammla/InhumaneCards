using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Threading;

namespace InhumaneCardsAndroid {
	[Activity(Label = "Inhumane Cards"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, AlwaysRetainTaskState = true
		, LaunchMode = LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.Landscape | ScreenOrientation.ReverseLandscape | ScreenOrientation.UserLandscape
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
	public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity {

		private View view;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			FixFullscreen();

			var g = new AndroidGame(this);
			view = (View)g.Services.GetService(typeof(View));
			SetContentView(view);
			g.Run();
		}

		protected override void OnResume() {
			base.OnResume();
			FixFullscreen();
		}

		public void FixFullscreen() {
			var uiOptions =
			SystemUiFlags.HideNavigation |
			SystemUiFlags.LayoutHideNavigation |
			SystemUiFlags.LayoutFullscreen |
			SystemUiFlags.Fullscreen |
			SystemUiFlags.LayoutStable |
			SystemUiFlags.ImmersiveSticky;

			Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
		}

		public void GetInputDialog(string title, string defaultText,Action<string> onDoneAction) {

			EditText et = new EditText(this) {
				Text = defaultText
			};
			AlertDialog.Builder ad = new AlertDialog.Builder(this);
			ad.SetTitle(title);
			ad.SetView(et);
			ad.SetPositiveButton("Okay", (arg0, arg1) => {
				onDoneAction(et.Text);
			});
			
			ad.Show();

		}

		public void PerformHapticFeedback() {
			try {
				view.PerformHapticFeedback(FeedbackConstants.VirtualKey);
			} catch (Exception e) {
				e.ToString();
			}

		}
	}
}

