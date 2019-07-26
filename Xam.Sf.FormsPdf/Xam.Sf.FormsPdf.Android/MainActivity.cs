
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Samples.Syncfusion.XamarinForms.Pdf;
using Xamarin.Forms;
using Wibci.Xamarin.Images;
using Wibci.Xamarin.Images.Droid;
using Plugin.CurrentActivity;

namespace Xam.Sf.FormsPdf.Droid
{
	[Activity(Label = "Xam.Sf.FormsPdf", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			CrossCurrentActivity.Current.Init(this, savedInstanceState);

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			LoadApplication(new App());

			DependencyService.Register<ISaveFileStreamCommand, AndroidSaveFileStreamCommand>();
			DependencyService.Register<IResizeImageCommand, AndroidResizeImageCommand>();
			DependencyService.Register<IAnalyseImageCommand, AndroidAnalyseImageCommand>();
			DependencyService.Register<IImageUtility, AndroidImageUtility>();

		}
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

	}
}