using iTextSharp.text;
using iTextSharp.text.pdf;
using System;

namespace PontoPortaria1510.Report
{
    public class PdfFooterHandler : PdfPageEventHelper
    {
        public string TextoEsquerda { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        public string TextoDireita { get; set; } = "";
        public Image ImageLogo { get; set; }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            PdfContentByte canvas = writer.DirectContent;


            float cellHeight = document.BottomMargin;
            // PDF document size      
            Rectangle page = document.PageSize;

            // create two column table
            PdfPTable footer = new PdfPTable(3);
            footer.TotalWidth = page.Width - 40;
            footer.SetWidths(new float[] { page.Width / 3f, page.Width / 3f, page.Width / 3f });
            // add image; PdfPCell() overload sizes image to fit cell
            PdfPCell c = new PdfPCell(ImageLogo, true);
            c.HorizontalAlignment = Element.ALIGN_LEFT;
            c.FixedHeight = cellHeight;
            c.Border = PdfPCell.NO_BORDER;
            footer.AddCell(c);
            
            c = new PdfPCell(new Phrase(TextoEsquerda, FontFactory.GetFont("Arial", 9, Font.BOLD)));
            c.HorizontalAlignment = Element.ALIGN_CENTER;
            c.FixedHeight = cellHeight;
            c.Border = PdfPCell.NO_BORDER;
            footer.AddCell(c);
            // add the header text
            c = new PdfPCell(new Phrase(
              TextoDireita,
               FontFactory.GetFont("Arial", 9, Font.BOLD)));
            c.Border = PdfPCell.NO_BORDER;
            c.VerticalAlignment = Element.ALIGN_CENTER;
            c.HorizontalAlignment = Element.ALIGN_RIGHT;

            c.FixedHeight = cellHeight;
            footer.AddCell(c);

            // since the table header is implemented using a PdfPTable, we call
            // WriteSelectedRows(), which requires absolute positions!
            footer.WriteSelectedRows(
              0, -1,  // first/last row; -1 flags all write all rows
              20,      // left offset
                      // ** bottom** yPos of the table
              cellHeight + footer.TotalHeight - 5,
              writer.DirectContent
            );

            //ColumnText.ShowTextAligned(
            //  canvas, Element.ALIGN_RIGHT,
            //  new Phrase(TextoDireita, FontFactory.GetFont("Arial", 9, Font.BOLD)), document.PageSize.GetLeft(100), document.PageSize.GetBottom(20), 0f
            //);

            
            //ColumnText.ShowTextAligned(
            //  canvas, Element.ALIGN_LEFT,
            //  new Phrase(TextoEsquerda, FontFactory.GetFont("Arial", 9, Font.BOLD)), document.PageSize.GetRight(95), document.PageSize.GetBottom(20), 0f
            //);
        }

    }
}
