using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.IO;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Xamarin.Forms;

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

            //Create a new PDF document.
            PdfDocument document = new PdfDocument();
            //Add a page to the document.
            PdfPage page = document.Pages.Add();
            //Create PDF graphics for the page.
            PdfGraphics graphics = page.Graphics;

            PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            RectangleF bounds = new RectangleF(0, 90, graphics.ClientSize.Width, 30);
            //Draws a rectangle to place the heading in that region.
            graphics.DrawRectangle(solidBrush, bounds);
            //Creates a font for adding the heading in the page
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
            //Creates a text element to add the invoice number
            PdfTextElement element = new PdfTextElement("INVOICE 777", subHeadingFont);
            element.Brush = PdfBrushes.White;

            //Draws the heading on the page
            PdfLayoutResult result = element.Draw(page, new PointF(10, 98));
            string currentDate = "DATE " + DateTime.Now.ToString("MM/dd/yyyy");
            //Measures the width of the text to place it in the correct location
            SizeF textSize = subHeadingFont.MeasureString(currentDate);
            PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
            //Draws the date by using DrawString method
            graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);
            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
            //Creates text elements to add the address and draw it to the page.
            element = new PdfTextElement("BILL TO ", timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
            PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            //Draws a line at the bottom of the address
            graphics.DrawLine(linePen, startPoint, endPoint);

            //Load the image from the disk.
            using (MemoryStream imageStream = request.Invoice.Logo.ToMemoryStream())
            {
                PdfBitmap image = new PdfBitmap(imageStream);
                graphics.DrawImage(image, new RectangleF(176, 0, 390, 130));
            }

            //Save the document.
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms); //TODO: platform Save
                var saveCommand = DependencyService.Get<ISaveFileStreamCommand>();
                var saveResult = await saveCommand.ExecuteAsync(new SaveFileStreamContext { FileName = request.FileName, ContentType = "application/pdf", Stream = ms, LaunchFile = true });
                retResult.Notification.AddRange(saveResult.Notification);
            }

            //Close the document.
            document.Close(true);

            return retResult;
        }
    }

    public class GenerateInvoiceContext
    {
        public string FileName { get; set; }

        public Invoice Invoice { get; set; }
    }
}