using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wibci.LogicCommand;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public interface IGenerateInvoiceCommand : IAsyncLogicCommand<GenerateInvoiceContext, GenerateDefaultInvoiceResult>
    {
    }

    public class GenerateDefaultInvoiceResult : TaskCommandResult
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

            //GenerateHeader(request, pdf);

            float y = GenerateBodyHeader(request, pdf);

            y = GenerateBody(request, pdf, y, !request.SimpleFormat);

            y = GenerateSignature(request, pdf, y);

            y += 10;

            y = GenerateTermsBody(request, pdf, y);

            GenerateFooter(request, pdf);
            //Save the document.
            await pdf.SaveAsync(request.FileName);

            return retResult;
        }

        private float GenerateBody(GenerateInvoiceContext request, PdfGenerator pdf, float currentY, bool generateItems)
        {
            float y = currentY;

            y += 10;
            PdfLayoutResult result = pdf.AddText(request.Invoice.Description, 10, y);

            y = result.Bounds.Bottom;

            if (generateItems)
            {
                if (request.SimpleTableItems)
                {
                    y = GenerateItemizedBodyWithLightTable(request, pdf, y);
                }
                else
                {
                    y = GenerateItemizedBodyWithGrid(request, pdf, y);
                }
            }

            y = GenerateTotal(request, pdf, y);

            return y;
        }

        private float GenerateBodyHeader(GenerateInvoiceContext request, PdfGenerator pdf)
        {
            PdfLayoutResult result = null;
            float y = 10;
            //Add IMAGE
            pdf.AddImage(request.Invoice.Logo, 0, y, request.LogoWidth, request.LogoHeight);

            var font = pdf.NormalFontBold;
            float x = request.LogoWidth > 0 ? request.LogoWidth + 10 : 0;
            result = pdf.AddText(request.Invoice.BusinessName, x, y, font);
            result = pdf.AddText(request.Invoice.BusinessInfo, x, result.Bounds.Bottom + 2);
            float businessY = result.Bounds.Bottom + 10;

            string twitterLink = "http://twitter.com/syncfusion";
            string webLink = "https://www.syncfusion.com/";
            string facebook = "https://www.facebook.com/Syncfusion";
            string youtube = "https://www.youtube.com/syncfusioninc";
            string linkedIn = "https://www.linkedin.com/company/syncfusion?trk=top_nav_home";
            string instagram = "http://www.instagram.com";

            string[] socialLinks = new string[] { "twitter", "website", "facebook", "youtube", "linkedIn", "instagram" };

            //left align all the social links
            float xOffsetTextSize = pdf.LongestText(socialLinks);

            result = pdf.AddWebLink(webLink, 0, y, "website", null, true, xOffsetTextSize);
            result = pdf.AddWebLink(facebook, 0, result.Bounds.Bottom, "facebook", null, true, xOffsetTextSize);
            result = pdf.AddWebLink(twitterLink, 0, result.Bounds.Bottom, "twitter", null, true, xOffsetTextSize);
            result = pdf.AddWebLink(youtube, 0, result.Bounds.Bottom, "youtube", null, true, xOffsetTextSize);
            result = pdf.AddWebLink(linkedIn, 0, result.Bounds.Bottom, "linkedin", null, true, xOffsetTextSize);
            result = pdf.AddWebLink(instagram, 0, result.Bounds.Bottom, "instagram", null, true, xOffsetTextSize);

            float socialLinksY = result.Bounds.Bottom + 10;
            float imageY = request.LogoHeight + 15;

            float actualY = Enumerable.Max(new float[] { businessY, socialLinksY, imageY });
            y = actualY;

            string customer = request.Invoice.Customer + Environment.NewLine + request.Invoice.Address;
            result = pdf.AddText(customer, 0, y, font);

            y = result.Bounds.Bottom + 10;

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

        private void GenerateFooter(GenerateInvoiceContext request, PdfGenerator pdf)
        {
            //https://help.syncfusion.com/file-formats/pdf/working-with-headers-and-footers

            RectangleF bounds = new RectangleF(0, 0, pdf.PageWidth, 50);

            PdfPageTemplateElement footer = new PdfPageTemplateElement(bounds);

            //BUG: NullReferenceException when trying to put a web link into the footer - drawing web links using footer graphics not supported yet by syncfusion
            //pdf.DrawWebLink(0, 35, "http://www.syncfusion.com", "Awesome control library for your mobile cross platform needs", pdf.NormalFont, footer.Graphics);
            pdf.DrawWebLinkPageBottom(0, 35, "http://www.syncfusion.com", "Awesome control library for your mobile cross platform needs", pdf.NormalFont);

            PdfCompositeField compositeField = new PdfCompositeField(pdf.NormalFont, pdf.AccentBrush, "http://www.syncfusion.com");
            compositeField.Bounds = footer.Bounds;

            //Draw the composite field in footer.

            compositeField.Draw(footer.Graphics, new PointF(0, 35));

            //BUG: Doesn't generate in the footer bounds - gets rendered on the page graphics instead
            pdf.DrawHorizontalLine(0, pdf.PageWidth, 0, 0.7f, pdf.AccentColor, footer.Graphics);

            pdf.Document.Template.Bottom = footer;
        }

        private void GenerateHeader(GenerateInvoiceContext request, PdfGenerator pdf)
        {
            RectangleF bounds = new RectangleF(0, 0, pdf.PageWidth, 50);

            PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

            pdf.DrawText("Header Text", 10, 0, pdf.NormalFont, pdf.AccentBrush, header.Graphics);

            pdf.Document.Template.Top = header;
        }

        private float GenerateItemizedBodyWithGrid(GenerateInvoiceContext request, PdfGenerator pdf, float y)
        {
            throw new NotImplementedException();
        }

        private float GenerateItemizedBodyWithLightTable(GenerateInvoiceContext request, PdfGenerator pdf, float currentY)
        {
            float y = currentY;

            PdfLightTable pdfLightTable = new PdfLightTable();

            //Add columns to the DataTable
            pdfLightTable.Columns.Add(new PdfColumn("Title"));
            pdfLightTable.Columns.Add(new PdfColumn("Cost"));
            pdfLightTable.Columns.Add(new PdfColumn("Qty"));
            pdfLightTable.Columns.Add(new PdfColumn("Total"));

            foreach (var item in request.Invoice.Items)
            {
                pdfLightTable.Rows.Add(new object[] { item.Name, item.ItemAmount, item.Quantity, item.Amount });
            }

            PdfLightTableLayoutFormat layoutFormat = new PdfLightTableLayoutFormat();

            layoutFormat.Break = PdfLayoutBreakType.FitPage;
            layoutFormat.Layout = PdfLayoutType.Paginate;

            var result = pdfLightTable.Draw(pdf.CurrentPage, new PointF(0, y), layoutFormat);

            y = result.Bounds.Bottom;

            return y;
        }

        private float GenerateSignature(GenerateInvoiceContext request, PdfGenerator pdf, float y)
        {
            y += 55; //gap to place signature

            pdf.DrawHorizontalLine(15, 85, y, 0.7f, pdf.AccentColor);
            pdf.DrawHorizontalLine(85, 15, y, 0.7f, pdf.AccentColor, null, true);

            y += 2;
            var result = pdf.AddText("customer", 15, y);
            result = pdf.AddText("business", 15, y, null, null, true);
            y = result.Bounds.Bottom;

            return y;
        }

        private float GenerateTermsBody(GenerateInvoiceContext request, PdfGenerator pdf, float y)
        {
            PdfLayoutResult result = pdf.AddText(request.Invoice.Terms, 10, y, pdf.NormalFontBold);
            y = result.Bounds.Bottom;
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

            leftText = "VAT" + request.Invoice.VatPercentage + "%";
            rightText = request.Invoice.Currency + " " + vatAmount.ToString();
            result = pdf.AddRectangleText(leftText, rightText, y, 30, PdfBrushes.White, pdf.AccentBrush, pdf.NormalFontBold);
            y = result.Bounds.Bottom;

            y += 5;
            leftText = "Total Due";
            rightText = request.Invoice.Currency + " " + total.ToString();
            result = pdf.AddRectangleText(leftText, rightText, y, 30);

            y = result.Bounds.Bottom + 10;

            return y;
        }
    }

    public class GenerateInvoiceContext
    {
        public string FileName { get; set; }

        public Invoice Invoice { get; set; }

        public uint LogoHeight { get; set; }
        public uint LogoWidth { get; set; }
        public bool SimpleFormat { get; set; }

        public bool SimpleTableItems { get; set; }
    }
}