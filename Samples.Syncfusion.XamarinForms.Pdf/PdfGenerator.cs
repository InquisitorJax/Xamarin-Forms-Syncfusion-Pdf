using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Xamarin.Forms;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public class PdfGenerator
    {
        private static float WORKING_HEIGHT = 0;
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

        public PdfColor PdfGridStyle2AltColor { get; set; }

        public PdfBrush PdfGridStyle2Brush { get; set; }

        public PdfColor PdfGridStyle2Color { get; set; }

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
                    try
                    {
                        PdfBitmap bitmap = new PdfBitmap(imageStream);
                        CurrentPageGraphics.DrawImage(bitmap, new RectangleF(0, y, width, height));
                    }
                    catch (PdfException pex)
                    {
                        Debug.WriteLine("ERROR!" + pex.Message);
                        throw;
                    }
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

            AddText(rightText, 10, y, font, textBrush, true);

            return result;
        }

        public PdfLayoutResult AddText(string text, float x, float y, PdfFont font = null, PdfBrush brush = null, bool xIsRightOffset = false, float? forceTextSize = null)
        {
            font = font ?? NormalFont;
            brush = brush ?? AccentBrush;

            if (xIsRightOffset)
            {
                SizeF textSize = font.MeasureString(text);
                float textWidth = forceTextSize ?? textSize.Width;
                x = PageWidth - textWidth - x;
            }

            PdfTextElement element = new PdfTextElement(text, font, brush);
            PdfLayoutResult result = element.Draw(CurrentPage, new PointF(x, y));
            return result;
        }

        public PdfLayoutResult AddWebLink(string webLink, float x, float y, string linkText, PdfFont font = null, bool xIsRightOffset = false, float? forceTextSize = null)
        {
            font = font ?? NormalFont;

            PdfTextWebLink textLink = new PdfTextWebLink();
            textLink.Url = webLink;
            textLink.Text = linkText;
            textLink.Font = font;

            if (xIsRightOffset)
            {
                SizeF textSize = font.MeasureString(webLink);
                float textWidth = forceTextSize ?? textSize.Width;
                x = PageWidth - textWidth - x;
            }

            var result = textLink.DrawTextWebLink(CurrentPage, new PointF(x, y));
            return result;
        }

        public void DrawHorizontalLine(float xStart, float xEnd, float y, float lineWidth, PdfColor color, PdfGraphics graphics = null, bool xIsRightOffset = false)
        {
            graphics = graphics ?? CurrentPageGraphics;
            PdfPen linePen = new PdfPen(color, lineWidth);

            if (xIsRightOffset)
            {
                xStart = PageWidth - xStart;
                xEnd = PageWidth - xEnd;
            }

            PointF startPoint = new PointF(xStart, y);
            PointF endPoint = new PointF(xEnd, y);
            //Draws a line at the bottom of the address
            graphics.DrawLine(linePen, startPoint, endPoint);
        }

        public void DrawRectangle(float x, float y, float width, float height, PdfBrush brush = null)
        {
            RectangleF bounds = new RectangleF(x, y, width, height);
            brush = brush ?? AccentBrush;
            //Draws a rectangle to place the heading in that region.
            CurrentPageGraphics.DrawRectangle(brush, bounds);
        }

        public void DrawText(string text, float x, float y, PdfFont font = null, PdfBrush brush = null, PdfGraphics graphics = null)
        {
            font = font ?? NormalFont;
            brush = brush ?? AccentBrush;
            graphics = graphics ?? CurrentPageGraphics;

            PdfTextElement element = new PdfTextElement(text, font, brush);
            element.Draw(graphics, new PointF(x, y));
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

        public void DrawWebLinkPageBottom(float x, float yOffset, string webLink, string linkText, PdfFont font = null)
        {
            font = font ?? NormalFont;

            float y = CurrentPage.Graphics.ClientSize.Height - yOffset;

            PdfTextWebLink textLink = new PdfTextWebLink();
            textLink.Url = webLink;
            textLink.Text = linkText;
            textLink.Font = font;
            textLink.DrawTextWebLink(CurrentPageGraphics, new PointF(x, y));
        }

        /// <summary>
        /// Check that the generated component hasn't returned a bottom bounds that is higher than the page
        /// </summary>
        /// <param name="currentY"></param>
        /// <param name="footerHeight"></param>
        /// <returns></returns>
        public float IncrementY(float currentY, float addHeight, float footerHeight = 0, float nextElementHeight = 20)
        {
            currentY += addHeight;

            if (WORKING_HEIGHT == 0)
            {
                WORKING_HEIGHT = CurrentPage.GetClientSize().Height - footerHeight;
            }

            var bottom = WORKING_HEIGHT - nextElementHeight;

            if (bottom < currentY)
            {
                AddPage("add new page");
                currentY = 0;
            }
            return currentY;
        }

        public float LongestText(string[] text, PdfFont font = null)
        {
            font = font ?? NormalFont;
            float longest = text.Max(x => font.MeasureString(x).Width);

            return longest;
        }

        public float MeasureTextHeight(string text, PdfFont font = null)
        {
            font = font ?? NormalFont;
            float height = font.MeasureString(text).Height;

            return height;
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
            Document.Pages.PageAdded -= Pages_PageAdded;
            Document.Close(true);

            return result;
        }

        public void Setup(string firstPageName)
        {
            //Create a new PDF document.
            Document = new PdfDocument();
            Document.Pages.PageAdded += Pages_PageAdded;
            //Add a page to the document.
            AddPage(firstPageName);

            AccentColor = new PdfColor(89, 106, 122);
            AccentBrush = new PdfSolidBrush(AccentColor);
            PdfGridStyle2Color = new PdfColor(237, 125, 49);
            PdfGridStyle2AltColor = new PdfColor(251, 228, 213);
            PdfGridStyle2Brush = new PdfSolidBrush(PdfGridStyle2Color);

            NormalFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
            PdfFontStyle style = PdfFontStyle.Bold;
            NormalFontBold = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, style);
            SubHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 18);
        }

        private void Pages_PageAdded(object sender, PageAddedEventArgs args)
        {
            Debug.WriteLine("Page added!");
            //when document grows with new page, make sure current page is always the latest page added
            CurrentPage = args.Page;
        }
    }
}