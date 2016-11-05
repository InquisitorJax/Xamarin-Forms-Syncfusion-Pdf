using Prism.Commands;
using Prism.Mvvm;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public class MainPageViewModel : BindableBase
    {
        private Invoice _model;

        public MainPageViewModel()
        {
            Model = new Invoice();
            Model.Heading = "heading here";
            GenerateInvoiceCommand = DelegateCommand.FromAsyncHandler(GenerateInvoiceAsync);
            TakePictureCommand = DelegateCommand.FromAsyncHandler(TakePictureAsync);
        }

        public ICommand GenerateInvoiceCommand { get; }

        public Invoice Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        public ICommand TakePictureCommand { get; }

        private async Task GenerateInvoiceAsync()
        {
            var generateCommand = DependencyService.Get<IGenerateInvoiceCommand>();
            var result = await generateCommand.ExecuteAsync(new GenerateInvoiceContext { FileName = "syncfusionInvoice.pdf", Invoice = Model });

            if (!result.IsValid())
            {
                Debug.WriteLine($"Generate Invoice FAILED! {result.Notification.ToString()}");
            }
        }

        private async Task TakePictureAsync()
        {
            var takePicture = DependencyService.Get<ITakePictureCommand>();
            var pictureResult = await takePicture.ExecuteAsync(TakePictureRequest.Default);

            if (pictureResult.TaskResult == TaskResult.Success)
            {
                Model.Logo = pictureResult.Image;
            }
        }
    }
}