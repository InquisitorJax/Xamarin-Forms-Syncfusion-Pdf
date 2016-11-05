using Xamarin.Forms;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<ITakePictureCommand, TakePictureCommand>();
            DependencyService.Register<IGenerateInvoiceCommand, GenerateInvoiceCommand>();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }
    }
}