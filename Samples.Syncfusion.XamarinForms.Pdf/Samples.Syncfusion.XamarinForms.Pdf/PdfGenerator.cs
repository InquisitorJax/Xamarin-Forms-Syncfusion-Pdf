using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public class PdfGenerator
    {
        private Dictionary<string, PdfPage> _pages;

        public PdfGenerator()
        {
            _pages = new Dictionary<string, PdfPage>();
        }

        public PdfBrush AccentBrush { get; set; }
        public PdfColor AccentColor { get; set; }
        public PdfPage CurrentPage { get; set; }

        public string CurrentPageName
        {
            get
            {
                string pageName = _pages.First(x => x.Value == CurrentPage).Key;
                return pageName;
            }
        }

        public PdfDocument Document { get; set; }
        public PdfGraphics Graphics { get; set; }
        public PdfFont NormalFont { get; set; }
        public PdfFont NormalFontBold { get; set; }
        public PdfPage Page { get; set; }

        public float PageWidth
        {
            get { return CurrentPageGraphics.ClientSize.Width; }
        }

        public PdfFont SubHeadingFont { get; set; }

        private PdfGraphics CurrentPageGraphics
        {
            get { return CurrentPage.Graphics; }
        }

        public void AddImage(byte[] image, float x, float y, float width, float height)
        {
            if (image != null)
            {
                using (MemoryStream imageStream = image.AsMemoryStream())
                {
                    PdfBitmap bitmap = new PdfBitmap(imageStream);
                    CurrentPageGraphics.DrawImage(bitmap, new RectangleF(0, y, width, height));
                }
            }
        }

        public void AddPage(string name)
        {
            if (_pages.ContainsKey(name))
            {
                throw new InvalidDataException("Page with that name already exists");
            }

            PdfPage page = Document.Pages.Add();
            _pages.Add(name, page);
            CurrentPage = page;
        }

        public PdfLayoutResult AddRectangleText(string leftText, string rightText, float y, float height, PdfBrush rectangleBrush = null, PdfBrush textBrush = null, PdfFont font = null)
        {
            rectangleBrush = rectangleBrush ?? AccentBrush;
            textBrush = textBrush ?? PdfBrushes.White;
            font = font ?? SubHeadingFont;

            DrawRectangle(0, y, PageWidth, height, rectangleBrush);

            y += 5;
            PdfLayoutResult result = AddText(leftText, 10, y, font, textBrush);

            AddTextRight(rightText, 10, y, font, textBrush);

            return result;
        }

        public PdfLayoutResult AddText(string text, float x, float y, PdfFont font = null, PdfBrush brush = null)
        {
            font = font ?? NormalFont;
            brush = brush ?? AccentBrush;
            PdfTextElement element = new PdfTextElement(text, font, brush);
            PdfLayoutResult result = element.Draw(CurrentPage, new PointF(x, y));
            return result;
        }

        public PdfLayoutResult AddTextRight(string text, float xOffset, float y, PdfFont font = null, PdfBrush brush = null)
        {
            font = font ?? NormalFont;
            brush = brush ?? AccentBrush;

            SizeF textSize = font.MeasureString(text);
            PointF position = new PointF(PageWidth - textSize.Width - xOffset, y);

            PdfTextElement element = new PdfTextElement(text, font, brush);
            PdfLayoutResult result = element.Draw(CurrentPage, new PointF(PageWidth - textSize.Width - xOffset, y));
            //CurrentPageGraphics.DrawString(text, font, brush, position);
            return result;
        }

        public PdfLayoutResult AddWebLink(float x, float y, string webLink, string linkText, PdfFont font = null)
        {
            font = font ?? NormalFont;

            PdfTextWebLink textLink = new PdfTextWebLink();
            textLink.Url = webLink;
            textLink.Text = linkText;
            textLink.Font = font;
            var result = textLink.DrawTextWebLink(CurrentPage, new PointF(x, y));
            return result;
        }

        public void DrawHorizontalLine(float xStart, float xEnd, float y, float lineWidth, PdfColor color, PdfGraphics graphics = null)
        {
            graphics = graphics ?? CurrentPageGraphics;
            PdfPen linePen = new PdfPen(color, lineWidth);
            PointF startPoint = new PointF(xStart, y);
            PointF endPoint = new PointF(xEnd, y);
            //Draws a line at the bottom of the address
            CurrentPageGraphics.DrawLine(linePen, startPoint, endPoint);
        }

        public void DrawRectangle(float x, float y, float width, float height, PdfBrush brush = null)
        {
            RectangleF bounds = new RectangleF(x, y, width, height);
            brush = brush ?? AccentBrush;
            //Draws a rectangle to place the heading in that region.
            CurrentPageGraphics.DrawRectangle(brush, bounds);
        }

        public void DrawWebLink(float x, float y, string webLink, string linkText, PdfFont font = null, PdfGraphics graphics = null)
        {
            font = font ?? NormalFont;
            graphics = graphics ?? CurrentPageGraphics;

            PdfTextWebLink textLink = new PdfTextWebLink();
            textLink.Url = webLink;
            textLink.Text = linkText;
            textLink.Font = font;
            textLink.DrawTextWebLink(graphics, new PointF(x, y));
        }

        public async Task<TaskResult> SaveAsync(string fileName, bool launchFile = true)
        {
            TaskResult result = TaskResult.Success;
            using (MemoryStream ms = new MemoryStream())
            {
                Document.Save(ms);
                var saveCommand = DependencyService.Get<ISaveFileStreamCommand>();
                var saveResult = await saveCommand.ExecuteAsync(new SaveFileStreamContext { FileName = fileName, ContentType = "application/pdf", Stream = ms, LaunchFile = launchFile });
                result = saveResult.TaskResult;
            }

            //Close the document.
            Document.Close(true);

            return result;
        }

        public void Setup(string firstPageName)
        {
            //Create a new PDF document.
            Document = new PdfDocument();
            //Add a page to the document.
            AddPage(firstPageName);

            AccentColor = new PdfColor(126, 151, 173);
            AccentBrush = new PdfSolidBrush(AccentColor);
            NormalFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
            PdfFontStyle style = PdfFontStyle.Bold;
            NormalFontBold = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, style);
            SubHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 18);
        }
    }
}