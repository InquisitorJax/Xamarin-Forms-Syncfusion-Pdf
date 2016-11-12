using Syncfusion.Pdf.Graphics;
using System;
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

            y = GenerateSimpleBody(request, pdf, y);

            GenerateFooter(request, pdf);
            //Save the document.
            await pdf.SaveAsync(request.FileName);

            return retResult;
        }

        private void GenerateFooter(GenerateInvoiceContext request, PdfGenerator pdf)
        {
        }

        private float GenerateHeader(GenerateInvoiceContext request, PdfGenerator pdf)
        {
            PdfLayoutResult result = null;
            float y = 10;
            //Load the image from the disk.
            pdf.AddImage(request.Invoice.Logo, 0, y, request.LogoWidth, request.LogoHeight);

            y = y + 150;
            pdf.DrawRectangle(0, y, pdf.PageWidth, 30);

            //Creates a text element to add the invoice number
            y += 5;
            result = pdf.AddText("INVOICE ", 10, y, pdf.SubHeadingFont, PdfBrushes.White);
            result = pdf.AddText("#" + request.Invoice.Number.ToString(), result.Bounds.Right + 5, y, pdf.SubHeadingFont, PdfBrushes.White);

            //Draws the date on the right of the page
            string currentDate = "Date " + DateTime.Now.ToString("MM/dd/yyyy");
            pdf.DrawTextRight(currentDate, 10, y, pdf.SubHeadingFont, PdfBrushes.White);

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

        private void GenerateItemizedBody(GenerateInvoiceContext request, PdfGenerator pdf)
        {
        }

        private float GenerateSimpleBody(GenerateInvoiceContext request, PdfGenerator pdf, float currentY)
        {
            float y = currentY;

            y += 10;
            PdfLayoutResult result = pdf.AddText(request.Invoice.Description, 10, y);

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