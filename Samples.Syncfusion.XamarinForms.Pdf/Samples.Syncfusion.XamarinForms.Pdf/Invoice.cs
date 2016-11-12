using Prism.Mvvm;
using System;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public class Invoice : BindableBase
    {
        private string _address;
        private double _amount;
        private string _businessInfo;
        private string _businessName;
        private string _currency;
        private string _customer;
        private DateTime _date;
        private string _description;
        private string _heading;
        private byte[] _logo;

        private int _number;

        private int _vatPercentage;

        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        public double Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }

        public string BusinessInfo
        {
            get { return _businessInfo; }
            set { SetProperty(ref _businessInfo, value); }
        }

        public string BusinessName
        {
            get { return _businessName; }
            set { SetProperty(ref _businessName, value); }
        }

        public string Currency
        {
            get { return _currency; }
            set { SetProperty(ref _currency, value); }
        }

        public string Customer
        {
            get { return _customer; }
            set { SetProperty(ref _customer, value); }
        }

        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

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

        public int Number
        {
            get { return _number; }
            set { SetProperty(ref _number, value); }
        }

        public int VatPercentage
        {
            get { return _vatPercentage; }
            set { SetProperty(ref _vatPercentage, value); }
        }

        public void GenDefault()
        {
            Heading = "Something to be done";
            Description = $"Please do something with the tasl{Environment.NewLine}Line2: Something more.... {Environment.NewLine}Line 3: and then something more";

            Address = $"20 Dunbar Street {Environment.NewLine}Table View{Environment.NewLine}Western Cape{Environment.NewLine}South Africa{Environment.NewLine}7441";

            Currency = "R";
            Amount = 76543.21;

            Number = 12345;

            VatPercentage = 14;

            Customer = "Quark the Ferengy";

            BusinessName = "Z's Alien Hunters For Hire";

            BusinessInfo = $"Vat#: 456789{Environment.NewLine}{Environment.NewLine}13 Pragmatic Drive{Environment.NewLine}Coin Business Park{Environment.NewLine}Bellville{Environment.NewLine}7550{Environment.NewLine}{Environment.NewLine}Customer Support (555) 555-76343";
        }
    }
}