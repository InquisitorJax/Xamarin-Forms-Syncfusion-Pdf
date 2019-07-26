using System;
using System.Threading.Tasks;
using Android.Content;
using Java.IO;
using Samples.Syncfusion.XamarinForms.Pdf;
using Wibci.LogicCommand;
using Xamarin.Forms;

namespace Xam.Sf.FormsPdf.Droid
{
	public class AndroidSaveFileStreamCommand : AsyncLogicCommand<SaveFileStreamContext, TaskCommandResult>, ISaveFileStreamCommand
	{
		public override Task<TaskCommandResult> ExecuteAsync(SaveFileStreamContext request)
		{
			var retResult = new TaskCommandResult();

			string root = null;

			if (Android.OS.Environment.IsExternalStorageEmulated)

			{
				root = Android.OS.Environment.ExternalStorageDirectory.ToString();
			}
			else
			{
				root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
			}

			Java.IO.File myDir = new Java.IO.File(root + "/Syncfusion");

			myDir.Mkdir();

			Java.IO.File file = new Java.IO.File(myDir, request.FileName);

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

			if (request.LaunchFile && file.Exists())

			{
				Android.Net.Uri path = Android.Net.Uri.FromFile(file);

				string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());

				string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);

				Intent intent = new Intent(Intent.ActionView);

				intent.SetDataAndType(path, mimeType);

				Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
			}

			return Task.FromResult(new TaskCommandResult());

			//for platform implementations, see https://help.syncfusion.com/file-formats/pdf/working-with-xamarin
		}
	}
}