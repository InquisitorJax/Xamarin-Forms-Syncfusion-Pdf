using Prism.Mvvm;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public class Invoice : BindableBase
    {
        private byte[] _logo;

        public byte[] Logo
        {
            get { return _logo; }
            set { SetProperty(ref _logo, value); }
        }
    }
}