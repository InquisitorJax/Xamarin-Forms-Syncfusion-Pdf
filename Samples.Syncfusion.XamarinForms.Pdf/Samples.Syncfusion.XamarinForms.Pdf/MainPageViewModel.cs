using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public class MainPageViewModel : BindableBase
    {
        private Invoice _model;

        public MainPageViewModel()
        {
            Model = new Invoice();
            GenerateInvoiceCommand = DelegateCommand.FromAsyncHandler(GenerateInvoiceAsync);
        }

        public ICommand GenerateInvoiceCommand { get; }

        public Invoice Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        private async Task GenerateInvoiceAsync()
        {
            throw new NotImplementedException();
        }
    }
}