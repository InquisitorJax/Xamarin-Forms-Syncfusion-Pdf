using System.IO;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public static class ByteArrayExtensions
    {
        public static MemoryStream AsMemoryStream(this byte[] bytes)
        {
            return new MemoryStream(bytes);
        }
    }
}