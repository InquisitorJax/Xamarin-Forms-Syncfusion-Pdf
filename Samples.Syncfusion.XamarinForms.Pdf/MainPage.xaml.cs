using Xamarin.Forms;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public partial class MainPage : ContentPage
    {
		private MainPageViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
			_viewModel = new MainPageViewModel();
			_viewModel.RequestShowPdf += ViewModel_RequestShowPdf;
            BindingContext = _viewModel;
        }

		private async void ViewModel_RequestShowPdf(object sender, System.EventArgs e)
		{
			await Navigation.PushAsync(new PdfViewer());
		}
	}
}