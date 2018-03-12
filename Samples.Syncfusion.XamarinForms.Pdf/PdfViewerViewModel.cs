using System.IO;
using Prism.Mvvm;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
	public class PdfViewerViewModel : BindableBase
	{
		private readonly byte[] _document;
		private Stream _documentFileStream;

		public Stream DocumentFileStream
		{
			get { return _documentFileStream; }
			set { SetProperty(ref _documentFileStream, value); }
		}

		public PdfViewerViewModel()
		{
			_document = PdfMemoryStore.CurrentDocument;
			if (_document != null)
			{
				DocumentFileStream = _document.AsMemoryStream();
			}
		}
	}
}