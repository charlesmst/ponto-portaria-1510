using iTextSharp.text.pdf;

namespace PontoPortaria1510.Report
{
    public interface PageHandler
    {
        PdfPageEventHelper PdfHandler();
    }
}
