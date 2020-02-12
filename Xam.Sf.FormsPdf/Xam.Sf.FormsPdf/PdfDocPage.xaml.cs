using Xam.Sf.FormsPdf;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PdfDocPage : ContentPage
	{
		public PdfDocPage()
		{
			InitializeComponent();
			BindingContext = new PdfDocViewModel();
		}

		private void Button_Clicked(object sender, System.EventArgs e)
		{
			this.Navigation.PopAsync();
		}
	}
}