using Samples.Syncfusion.XamarinForms.Pdf;
using System;
using System.IO;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Windows.Storage;

namespace Xam.Sf.FormsPdf.UWP
{
	public class UWPSaveFileStreamCommand : AsyncLogicCommand<SaveFileStreamContext, TaskCommandResult>, ISaveFileStreamCommand
	{
		public override async Task<TaskCommandResult> ExecuteAsync(SaveFileStreamContext request)
		{
			StorageFolder local = ApplicationData.Current.LocalFolder;

			StorageFile outFile;
			try
			{
				outFile = await local.CreateFileAsync(request.FileName, CreationCollisionOption.ReplaceExisting);
			}
			catch (UnauthorizedAccessException)
			{
				//existing file with same name is likely open
				return new TaskCommandResult { TaskResult = TaskResult.AccessDenied };
			}

			using (Stream outStream = await outFile.OpenStreamForWriteAsync())
			{
				outStream.Write(request.Stream.ToArray(), 0, (int)request.Stream.Length);
			}

			if (request.LaunchFile)
			{
				await Windows.System.Launcher.LaunchFileAsync(outFile);
			}

			return new TaskCommandResult();
		}
	}
}
