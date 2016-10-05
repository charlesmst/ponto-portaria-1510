
using PontoPortaria1510.Report.Reports;
using System;

namespace PontoPortaria1510.Report
{
    public static class PdfReport
    {
        public static IPontoReport Gerador(PontoReportTipo tipo)
        {
            return Gerador(tipo, null);
        }
        public static IPontoReport Gerador(PontoReportTipo tipo, PageHandler headerFooterHandler)
        {
            switch (tipo)
            {
                case PontoReportTipo.Semanal:
                    return new PontoReportSemanal()
                    {
                        Handler = headerFooterHandler?.PdfHandler()
                    };
                default:
                    throw new NotImplementedException();                    
            }
        }
    }
}
