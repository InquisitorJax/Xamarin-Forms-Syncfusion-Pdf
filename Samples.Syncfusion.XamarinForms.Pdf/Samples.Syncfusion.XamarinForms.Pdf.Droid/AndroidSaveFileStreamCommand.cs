using Android.Content;
using Java.IO;
using System;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Xamarin.Forms;

namespace Samples.Syncfusion.XamarinForms.Pdf.Droid
{
    public class AndroidSaveFileStreamCommand : AsyncLogicCommand<SaveFileStreamContext, DeviceCommandResult>, ISaveFileStreamCommand
    {
        public override Task<DeviceCommandResult> ExecuteAsync(SaveFileStreamContext request)
        {
            var retResult = new DeviceCommandResult();

            string root = null;

            if (Android.OS.Environment.IsExternalStorageEmulated)

            {
                root = Android.OS.Environment.ExternalStorageDirectory.ToString();
            }
            else
            {
                root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            File myDir = new File(root + "/Syncfusion");

            myDir.Mkdir();

            File file = new File(myDir, request.FileName);

            if (file.Exists()) file.Delete();

            try

            {
                FileOutputStream outs = new FileOutputStream(file);

                outs.Write(request.Stream.ToArray());

                outs.Flush();

                outs.Close();
            }
            catch (Exception e)
            {
                retResult.Notification.Add(e.Message);
            }

            if (file.Exists())

            {
                Android.Net.Uri path = Android.Net.Uri.FromFile(file);

                string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());

                string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);

                Intent intent = new Intent(Intent.ActionView);

                intent.SetDataAndType(path, mimeType);

                Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
            }

            return Task.FromResult(new DeviceCommandResult());

            //for platform implementations, see https://help.syncfusion.com/file-formats/pdf/working-with-xamarin
        }
    }
}