using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Tables;
using System;
using System.Collections.Generic;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public static class PdfExtensions
    {
        public static void ApplyLightTableStyle(this PdfLightTable pdfLightTable, PdfColor borderColor, PdfColor textColor, PdfColor altBackgroundColor, PdfColor altTextColor)
        {
            //TODO: Extension
            var retStyle = new PdfLightTableStyle
            {
                BorderPen = new PdfPen(borderColor),
                DefaultStyle = new PdfCellStyle
                {
                    TextBrush = new PdfSolidBrush(textColor),
                    BorderPen = new PdfPen(borderColor)
                },
                AlternateStyle = new PdfCellStyle
                {
                    TextBrush = new PdfSolidBrush(textColor),
                    BackgroundBrush = new PdfSolidBrush(altBackgroundColor),
                    BorderPen = new PdfPen(borderColor)
                },
                ShowHeader = true,
                RepeatHeader = true,
                HeaderSource = PdfHeaderSource.ColumnCaptions,
                HeaderStyle = new PdfCellStyle
                {
                    BorderPen = new PdfPen(borderColor),
                    BackgroundBrush = new PdfSolidBrush(borderColor),
                    TextBrush = new PdfSolidBrush(altTextColor)
                }
            };

            pdfLightTable.Style = retStyle;
        }

        /// <summary>
        /// Must be called AFTER data has been added to the grid
        /// </summary>
        /// <param name="column"></param>
        /// <param name="grid"></param>
        public static void SizeColumnToContent(this PdfColumn column, IEnumerable<string> data, float pageWidth, PdfFont font)
        {
            float maxWidth = 0;

            foreach (string itemText in data)
            {
                float width = font.MeasureString(itemText).Width;

                if (maxWidth < width)
                    maxWidth = width;
            }

            var columnWidth = maxWidth + 3;

            if (columnWidth < pageWidth) //This condition prevents the table width to  maintain under the page width
            {
                column.Width = (float)Math.Floor(columnWidth);
            }
        }

        public static void SizeColumnToContent(this PdfGridColumn column, IEnumerable<string> data, float pageWidth, PdfFont font)
        {
            float maxWidth = 0;

            foreach (string itemText in data)
            {
                float width = font.MeasureString(itemText).Width;

                if (maxWidth < width)
                    maxWidth = width;
            }

            var columnWidth = maxWidth + 3;

            if (columnWidth < pageWidth) //This condition prevents the table width to  maintain under the page width
            {
                column.Width = (float)Math.Floor(columnWidth);
            }
        }
    }
}