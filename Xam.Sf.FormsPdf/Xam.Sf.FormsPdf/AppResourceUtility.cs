using Samples.Syncfusion.XamarinForms.Pdf;
using System.IO;
using System.Reflection;

namespace Xam.Sf.FormsPdf
{
	public static class AppResourceUtility
	{
		public static byte[] LoadAppResourceFromFile(this string resourceFileName)
		{
			var assembly = typeof(PdfGenerator).GetTypeInfo().Assembly; 
			const string resourcePrefix = "Xam.Sf.FormsPdf.Resources.";
			string fileName = resourcePrefix + resourceFileName;

			byte[] buffer;
			using (Stream s = assembly.GetManifestResourceStream(fileName))
			{
				long length = s.Length;
				buffer = new byte[length];
				s.Read(buffer, 0, (int)length);
			}

			return buffer;
		}
	}
}
