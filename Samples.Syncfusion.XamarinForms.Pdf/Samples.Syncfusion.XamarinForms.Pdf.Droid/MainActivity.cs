using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;
using Wibci.Xamarin.Images;
using Wibci.Xamarin.Images.Droid;
using Xamarin.Forms;

namespace Samples.Syncfusion.XamarinForms.Pdf.Droid
{
    [Activity(Label = "Samples.Syncfusion.XamarinForms.Pdf", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            DependencyService.Register<ISaveFileStreamCommand, AndroidSaveFileStreamCommand>();
            DependencyService.Register<IResizeImageCommand, AndroidResizeImageCommand>();
            //DependencyService.Register<IMedia, CrossMedia.Current>(); //TODO: For some reason does not compile :(
        }
    }
}