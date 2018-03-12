using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PdfViewer : ContentPage
	{
		public PdfViewer()
		{
			InitializeComponent();
			BindingContext = new PdfViewerViewModel();
		}
	}
}