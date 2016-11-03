using System.IO;
using Wibci.LogicCommand;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public interface ISaveFileStreamCommand : IAsyncLogicCommand<SaveFileStreamContext, DeviceCommandResult>
    {
    }

    public class SaveFileStreamContext
    {
        public SaveFileStreamContext()
        {
            LaunchFile = true;
        }

        public string ContentType { get; set; }
        public string FileName { get; set; }
        public bool LaunchFile { get; set; }
        public MemoryStream Stream { get; set; }
    }
}