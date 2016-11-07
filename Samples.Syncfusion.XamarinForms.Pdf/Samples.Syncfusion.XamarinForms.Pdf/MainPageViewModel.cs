using Prism.Commands;
using Prism.Mvvm;
using System.Diagnostics;
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
            Model.GenDefault();
            GenerateInvoiceCommand = new DelegateCommand(GenerateInvoice);
            TakePictureCommand = new DelegateCommand(TakePicture);
        }

        public ICommand GenerateInvoiceCommand { get; }

        public Invoice Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        public ICommand TakePictureCommand { get; }

        private async void GenerateInvoice()
        {
            var generateCommand = DependencyService.Get<IGenerateInvoiceCommand>();
            var result = await generateCommand.ExecuteAsync(new GenerateInvoiceContext { FileName = "syncfusionInvoice.pdf", Invoice = Model });

            if (!result.IsValid())
            {
                Debug.WriteLine($"Generate Invoice FAILED! {result.Notification.ToString()}");
            }
        }

        private async void TakePicture()
        {
            var takePicture = DependencyService.Get<ITakePictureCommand>();
            var pictureResult = await takePicture.ExecuteAsync(TakePictureRequest.Default);

            if (pictureResult.TaskResult == TaskResult.Success)
            {
                var resizeImage = DependencyService.Get<IResizeImageCommand>();
                var resizeResult = await resizeImage.ExecuteAsync(new ResizeImageContext { Height = 130, Width = 280, OriginalImage = pictureResult.Image });
                if (resizeResult.TaskResult == TaskResult.Success)
                {
                    Model.Logo = resizeResult.ResizedImage;
                }
                else
                {
                    Model.Logo = pictureResult.Image;
                }
            }
        }
    }
}