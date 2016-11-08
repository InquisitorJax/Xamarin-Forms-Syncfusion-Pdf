using QuickLook;
using System;

namespace Samples.Syncfusion.XamarinForms.Pdf.iOS
{
    public class PreviewControllerDS : QLPreviewControllerDataSource
    {
        private QLPreviewItem _item;

        public PreviewControllerDS(QLPreviewItem item)
        {
            _item = item;
        }

        public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
        {
            return _item;
        }

        public override nint PreviewItemCount(QLPreviewController controller)
        {
            return (nint)1;
        }
    }
}