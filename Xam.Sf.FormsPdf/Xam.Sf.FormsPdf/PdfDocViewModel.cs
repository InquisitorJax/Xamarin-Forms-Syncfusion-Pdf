using Prism.Mvvm;
using Samples.Syncfusion.XamarinForms.Pdf;
using System.IO;

namespace Xam.Sf.FormsPdf
{
	public class PdfDocViewModel : BindableBase
	{
		private Stream _documentFileStream;
		private byte[] _document;

		public Stream DocumentFileStream
		{
			get { return _documentFileStream; }
			set { SetProperty(ref _documentFileStream, value); }
		}

		public PdfDocViewModel()
		{
			if (_documentFileStream == null)
			{
				_document = "LongPdf.pdf".LoadAppResourceFromFile();
			}
			DocumentFileStream = _document.AsMemoryStream();
		}

	}
}
