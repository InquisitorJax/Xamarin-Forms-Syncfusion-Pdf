using System.IO;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public static class ByteArrayExtensions
    {
        public static MemoryStream ToMemoryStream(this byte[] bytes)
        {
            return new MemoryStream(bytes);
        }
    }
}