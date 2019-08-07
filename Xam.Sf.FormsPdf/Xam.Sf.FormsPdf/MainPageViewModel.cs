using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Wibci.LogicCommand;
using Wibci.Xamarin.Images;
using Xam.Sf.FormsPdf;
using Xamarin.Forms;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
	public class MainPageViewModel : BindableBase
	{
		private uint _logoHeight;
		private uint _logoWidth;
		private Invoice _model;

		private int _numberOfInvoiceItems;
		private bool _useCamera;

		private bool _useSimpleTable;

		public event EventHandler<EventArgs> RequestShowPdf;

		public MainPageViewModel()
		{
			Model = new Invoice();
			NumberOfInvoiceItems = 30;
			Model.GenDefault(NumberOfInvoiceItems);
			GenerateInvoiceCommand = new DelegateCommand(GenerateInvoice);
			SelectPictureCommand = new DelegateCommand(SelectPicture);
			UseSimpleTable = true;
		}

		public ICommand GenerateInvoiceCommand { get; }

		public Invoice Model
		{
			get { return _model; }
			set { SetProperty(ref _model, value); }
		}

		public int NumberOfInvoiceItems
		{
			get { return _numberOfInvoiceItems; }
			set { SetProperty(ref _numberOfInvoiceItems, value); }
		}

		public ICommand SelectPictureCommand { get; }

		public bool UseCamera
		{
			get { return _useCamera; }
			set { SetProperty(ref _useCamera, value); }
		}

		public bool UseSimpleTable
		{
			get { return _useSimpleTable; }
			set { SetProperty(ref _useSimpleTable, value); }
		}

		private async void GenerateInvoice()
		{
			var generateCommand = DependencyService.Get<IGenerateInvoiceCommand>();

			Model.GenerateItems(NumberOfInvoiceItems);

			var context = new GenerateInvoiceContext
			{
				FileName = "syncfusionInvoice.pdf",
				Invoice = Model,
				LogoHeight = _logoHeight / 2,
				LogoWidth = _logoWidth / 2,
				SimpleFormat = false, //simple format doesn't generate line items for each invoice item
				SimpleTableItems = UseSimpleTable, //when SimpleFormat = false - choose what kind of table to use to generate the items !simple = use pdfGrid, else use SimpleTable
				OpenFileUsingSystemApp = false
			};

			var result = await generateCommand.ExecuteAsync(context);

			if (!result.IsValid() || result.TaskResult != TaskResult.Success)
			{
				Debug.WriteLine($"Generate Invoice FAILED! {result.Notification.ToString()}");
			}

			if (result.IsValid())
			{
				PdfMemoryStore.CurrentDocument = result.PdfResult;
				//show pdf in pdf viewer
				if (RequestShowPdf != null)
					RequestShowPdf.Invoke(this, EventArgs.Empty);
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
				var cropImage = DependencyService.Get<IImageUtility>();
				var croppedImage = cropImage.TransformIntoCircle(pictureResult.Image);

				var analyseImage = DependencyService.Get<IAnalyseImageCommand>();
				var croppedAnalyseResult = await analyseImage.ExecuteAsync(new AnalyseImageContext { Image = croppedImage });				
				if (croppedAnalyseResult.IsValid())
				{
					Model.Logo = croppedImage;
					_logoWidth = croppedAnalyseResult.Width;
					_logoHeight = croppedAnalyseResult.Height;
				}
				else
				{
					Model.Logo = pictureResult.Image;
				}
			}
		}
	}
}