using Prism.Mvvm;
using System;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public class InvoiceItem : BindableBase
    {
        private double _amount;
        private double _itemAmount;
        private string _name;

        private int _quantity;

        public double Amount
        {
            get { return _amount; }
            private set { SetProperty(ref _amount, value); }
        }

        public double ItemAmount
        {
            get { return _itemAmount; }
            set
            {
                SetProperty(ref _itemAmount, value);
                CalculateAmount();
            }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                SetProperty(ref _quantity, value);
                CalculateAmount();
            }
        }

        private void CalculateAmount()
        {
            double amount = ItemAmount * Quantity;
            Amount = Math.Round(amount, 2);
        }
    }
}