using QuickLook;
using System;
using System.IO;
using System.Threading.Tasks;
using UIKit;
using Wibci.LogicCommand;

namespace Samples.Syncfusion.XamarinForms.Pdf.iOS
{
    public class iOSSaveFileStreamCommand : AsyncLogicCommand<SaveFileStreamContext, DeviceCommandResult>, ISaveFileStreamCommand
    {
        public override Task<DeviceCommandResult> ExecuteAsync(SaveFileStreamContext request)
        {
            var retResult = new DeviceCommandResult();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var stream = request.Stream;

            string filePath = Path.Combine(path, request.FileName);

            try
            {
                FileStream fileStream = File.Open(filePath, FileMode.Create);
                stream.Position = 0;
                stream.CopyTo(fileStream);
                fileStream.Flush();
                fileStream.Close();
            }
            catch (Exception e)

            {
            }

            UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (currentController.PresentedViewController != null)

                currentController = currentController.PresentedViewController;

            UIView currentView = currentController.View;

            QLPreviewController qlPreview = new QLPreviewController();

            QLPreviewItem item = new QLPreviewItemBundle(request.FileName, filePath);

            qlPreview.DataSource = new PreviewControllerDS(item);

            //UIViewController uiView = currentView as UIViewController;

            currentController.PresentViewController(qlPreview, true, null);

            return Task.FromResult(retResult);
        }
    }
}