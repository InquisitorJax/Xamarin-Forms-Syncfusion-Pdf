using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using System.Threading.Tasks;
using Wibci.LogicCommand;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public interface IChoosePictureCommand : IAsyncLogicCommand<ChoosePictureRequest, SelectPictureResult>
    {
    }

    public class ChoosePictureCommand : AsyncLogicCommand<ChoosePictureRequest, SelectPictureResult>, IChoosePictureCommand
    {
        private IMedia MediaPicker
        {
            get { return CrossMedia.Current; }
        }

        public override async Task<SelectPictureResult> ExecuteAsync(ChoosePictureRequest request)
        {
            var retResult = new SelectPictureResult();

            //NOTE: send suspend event BEFORE page_disappearing event fires to page is not removed from the view stack
            //...   resume will be called by generic life-cycle

            if (!MediaPicker.IsPickPhotoSupported)
            {
                retResult.Notification.Add("No camera available :(");
                retResult.TaskResult = TaskResult.Failed;
                return retResult;
            }

            var options = new PickMediaOptions();
            if (request.MaxPixelDimension.HasValue)
            {
                options.PhotoSize = PhotoSize.Custom;
                options.CustomPhotoSize = request.MaxPixelDimension.Value;
            }

            var mediaFile = await MediaPicker.PickPhotoAsync(options);

            if (mediaFile != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    mediaFile.GetStream().CopyTo(ms);
                    retResult.Image = ms.ToArray();
                }

                retResult.TaskResult = TaskResult.Success;
            }
            else
            {
                retResult.TaskResult = TaskResult.Canceled;
                retResult.Notification.Add("Select picture canceled");
            }

            return retResult;
        }
    }
}