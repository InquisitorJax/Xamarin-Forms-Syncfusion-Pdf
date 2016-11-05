using Prism.Mvvm;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public class Invoice : BindableBase
    {
        private string _description;
        private string _heading;
        private byte[] _logo;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public string Heading
        {
            get { return _heading; }
            set { SetProperty(ref _heading, value); }
        }

        public byte[] Logo
        {
            get { return _logo; }
            set { SetProperty(ref _logo, value); }
        }
    }
}