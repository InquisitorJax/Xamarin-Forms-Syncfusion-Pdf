using Xamarin.Forms;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public class ExpandingEditor : Editor
    {
        //see: http://forums.xamarin.com/discussion/21951/allow-the-editor-control-to-grow-as-content-lines-are-added#latest
        public ExpandingEditor()
        {
            //NOTE: will only work if HeightRequest is not set
            this.TextChanged += (sender, e) => { this.InvalidateMeasure(); };
        }

        //renderer for iOS
        //public class ExpandableEditorRenderer : EditorRenderer
        //{
        //    protected override void OnElementChanged(ElementChangedEventArgs e)
        //    {
        //        base.OnElementChanged(e);

        //        if (Control != null)
        //        {
        //            Control.ScrollEnabled = false;
        //        }
        //    }
        //}
    }
}