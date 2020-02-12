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
			Disappearing += MainPage_Disappearing;
        }

		private void MainPage_Disappearing(object sender, System.EventArgs e)
		{
			
		}

		private async void ViewModel_RequestShowPdf(object sender, System.EventArgs e)
		{
			await Navigation.PushAsync(new PdfViewer());
		}

		private async void Show_PdfDoc(object sender, System.EventArgs e)
		{
			await Navigation.PushAsync(new PdfDocPage());
		}
	}
}