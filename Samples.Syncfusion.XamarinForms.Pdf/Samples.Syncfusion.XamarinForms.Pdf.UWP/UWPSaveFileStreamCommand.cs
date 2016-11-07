using System;
using System.IO;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Windows.Storage;

namespace Samples.Syncfusion.XamarinForms.Pdf.UWP
{
    public class UWPSaveFileStreamCommand : AsyncLogicCommand<SaveFileStreamContext, DeviceCommandResult>, ISaveFileStreamCommand
    {
        public override async Task<DeviceCommandResult> ExecuteAsync(SaveFileStreamContext request)
        {
            StorageFolder local = ApplicationData.Current.LocalFolder;

            StorageFile outFile = await local.CreateFileAsync(request.FileName, CreationCollisionOption.ReplaceExisting);

            using (Stream outStream = await outFile.OpenStreamForWriteAsync())
            {
                outStream.Write(request.Stream.ToArray(), 0, (int)request.Stream.Length);
            }

            if (request.LaunchFile)
            {
                await Windows.System.Launcher.LaunchFileAsync(outFile);
            }

            return new DeviceCommandResult();
        }
    }
}