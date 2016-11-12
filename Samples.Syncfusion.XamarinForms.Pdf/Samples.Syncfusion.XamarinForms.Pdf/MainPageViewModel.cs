using Prism.Commands;
using Prism.Mvvm;
using System.Diagnostics;
using System.Windows.Input;
using Wibci.Xamarin.Images;
using Xamarin.Forms;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public class MainPageViewModel : BindableBase
    {
        private int _logoHeight;
        private int _logoWidth;
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
            var context = new GenerateInvoiceContext
            {
                FileName = "syncfusionInvoice.pdf",
                Invoice = Model,
                LogoHeight = _logoHeight,
                LogoWidth = _logoWidth,
                SimpleFormat = true
            };

            var result = await generateCommand.ExecuteAsync(context);

            if (!result.IsValid() || result.TaskResult != TaskResult.Success)
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
                var resizeResult = await resizeImage.ExecuteAsync(new ResizeImageContext { Height = 130, Width = 130, OriginalImage = pictureResult.Image });
                if (resizeResult.TaskResult == Wibci.Xamarin.Images.TaskResult.Success)
                {
                    Model.Logo = resizeResult.ResizedImage;
                    _logoWidth = resizeResult.ResizedWidth;
                    _logoHeight = resizeResult.ResizedHeight;
                }
                else
                {
                    Model.Logo = pictureResult.Image;
                }
            }
        }
    }
}