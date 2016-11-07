using Xamarin.Forms;

namespace Samples.Syncfusion.XamarinForms.Pdf.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new Samples.Syncfusion.XamarinForms.Pdf.App());

            DependencyService.Register<ISaveFileStreamCommand, UWPSaveFileStreamCommand>();
        }
    }
}