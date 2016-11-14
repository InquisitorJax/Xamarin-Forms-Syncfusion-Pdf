using Syncfusion.Pdf.Graphics;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wibci.LogicCommand;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public interface IGenerateInvoiceCommand : IAsyncLogicCommand<GenerateInvoiceContext, GenerateDefaultInvoiceResult>
    {
    }

    public class GenerateDefaultInvoiceResult : DeviceCommandResult
    {
        public string FileName { get; set; }
        public byte[] PdfResult { get; set; }
    }

    public class GenerateInvoiceCommand : AsyncLogicCommand<GenerateInvoiceContext, GenerateDefaultInvoiceResult>, IGenerateInvoiceCommand
    {
        public override async Task<GenerateDefaultInvoiceResult> ExecuteAsync(GenerateInvoiceContext request)
        {
            GenerateDefaultInvoiceResult retResult = new GenerateDefaultInvoiceResult();

            //https://help.syncfusion.com/file-formats/pdf/getting-started
            PdfGenerator pdf = new PdfGenerator();
            pdf.Setup("Invoice");
            float y = GenerateHeader(request, pdf);

            if (request.SimpleFormat)
            {
                y = GenerateSimpleBody(request, pdf, y);
            }
            else
            {
                y = GenerateItemizedBody(request, pdf, y);
            }

            GenerateFooter(request, pdf);
            //Save the document.
            await pdf.SaveAsync(request.FileName);

            return retResult;
        }

        private void GenerateFooter(GenerateInvoiceContext request, PdfGenerator pdf)
        {
            //https://help.syncfusion.com/file-formats/pdf/working-with-headers-and-footers
        }

        private float GenerateHeader(GenerateInvoiceContext request, PdfGenerator pdf)
        {
            PdfLayoutResult result = null;
            float y = 10;
            //Add IMAGE
            pdf.AddImage(request.Invoice.Logo, 0, y, request.LogoWidth, request.LogoHeight);

            var font = pdf.NormalFontBold;
            result = pdf.AddText(request.Invoice.BusinessName, request.LogoWidth + 10, y, font);
            result = pdf.AddText(request.Invoice.BusinessInfo, request.LogoWidth + 10, result.Bounds.Bottom + 2);
            float businessY = result.Bounds.Bottom + 10;

            string customer = request.Invoice.Customer + Environment.NewLine + request.Invoice.Address;
            result = pdf.AddTextRight(customer, 0, y, font);
            float addressX = result.Bounds.Left;

            float addressY = result.Bounds.Bottom + 10;
            float imageY = request.LogoHeight + 15;

            float actualY = Enumerable.Max(new float[] { businessY, addressY, imageY });

            y += actualY;
            string leftText = "INVOICE #" + request.Invoice.Number.ToString();
            string rightText = DateTime.Now.ToString("dd MMM yyyy");
            result = pdf.AddRectangleText(leftText, rightText, y, 30);

            y = result.Bounds.Bottom;

            y += 15;
            //Creates text elements to add the address and draw it to the page.
            result = pdf.AddText(request.Invoice.Heading, 10, y, pdf.SubHeadingFont);
            y = result.Bounds.Bottom;

            y += 3;
            pdf.DrawHorizontalLine(0, pdf.PageWidth, y, 0.7f, pdf.AccentColor);

            y += 1;

            return y;
        }

        private float GenerateItemizedBody(GenerateInvoiceContext request, PdfGenerator pdf, float currentY)
        {
            float y = currentY;

            y += 10;
            PdfLayoutResult result = pdf.AddText(request.Invoice.Description, 10, y);

            return y;
        }

        private float GenerateSimpleBody(GenerateInvoiceContext request, PdfGenerator pdf, float currentY)
        {
            float y = currentY;

            y += 10;
            PdfLayoutResult result = pdf.AddText(request.Invoice.Description, 10, y);

            y = result.Bounds.Bottom;
            y = GenerateTotal(request, pdf, y);

            return y;
        }

        private float GenerateTotal(GenerateInvoiceContext request, PdfGenerator pdf, float currentY)
        {
            float y = currentY;

            double vatAmount = request.Invoice.Amount * (request.Invoice.VatPercentage / 100d);
            double total = request.Invoice.Amount + vatAmount;
            total = Math.Round(total, 2);

            y += 10;
            string leftText = "TOTAL";
            string rightText = request.Invoice.Currency + " " + request.Invoice.Amount.ToString();
            PdfLayoutResult result = pdf.AddRectangleText(leftText, rightText, y, 30, PdfBrushes.White, pdf.AccentBrush, pdf.NormalFontBold);
            y = result.Bounds.Bottom;

            leftText = "VAT";
            rightText = request.Invoice.VatPercentage + "%";
            result = pdf.AddRectangleText(leftText, rightText, y, 30, PdfBrushes.White, pdf.AccentBrush, pdf.NormalFontBold);
            y = result.Bounds.Bottom;

            y += 5;
            leftText = "Total Due";
            rightText = request.Invoice.Currency + " " + total.ToString();
            result = pdf.AddRectangleText(leftText, rightText, y, 30);

            return y;
        }
    }

    public class GenerateInvoiceContext
    {
        public string FileName { get; set; }

        public Invoice Invoice { get; set; }

        public int LogoHeight { get; set; }
        public int LogoWidth { get; set; }
        public bool SimpleFormat { get; set; }
    }
}