using Prism.Commands;
using Prism.Mvvm;
using System.Diagnostics;
using System.Windows.Input;
using Wibci.LogicCommand;
using Wibci.Xamarin.Images;
using Xamarin.Forms;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public class MainPageViewModel : BindableBase
    {
        private uint _logoHeight;
        private uint _logoWidth;
        private Invoice _model;

        private bool _useCamera;

        public MainPageViewModel()
        {
            Model = new Invoice();
            Model.GenDefault();
            GenerateInvoiceCommand = new DelegateCommand(GenerateInvoice);
            SelectPictureCommand = new DelegateCommand(SelectPicture);
        }

        public ICommand GenerateInvoiceCommand { get; }

        public Invoice Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        public ICommand SelectPictureCommand { get; }

        public bool UseCamera
        {
            get { return _useCamera; }
            set { SetProperty(ref _useCamera, value); }
        }

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

        private async void SelectPicture()
        {
            SelectPictureResult pictureResult = null;
            if (UseCamera)
            {
                var takePicture = DependencyService.Get<ITakePictureCommand>();
                var request = new TakePictureRequest { MaxPixelDimension = 130, CameraOption = CameraOption.Back };
                pictureResult = await takePicture.ExecuteAsync(request);
            }
            else
            {
                var choosePicture = DependencyService.Get<IChoosePictureCommand>();
                var request = new ChoosePictureRequest { MaxPixelDimension = 130 };
                pictureResult = await choosePicture.ExecuteAsync(request);
            }

            if (pictureResult.TaskResult == TaskResult.Success)
            {
                var analyseImage = DependencyService.Get<IAnalyseImageCommand>();
                var analyseResult = await analyseImage.ExecuteAsync(new AnalyseImageContext { Image = pictureResult.Image });
                if (analyseResult.IsValid())
                {
                    Model.Logo = pictureResult.Image;
                    _logoWidth = analyseResult.Width;
                    _logoHeight = analyseResult.Height;
                }
                else
                {
                    Model.Logo = pictureResult.Image;
                }
            }
        }
    }
}