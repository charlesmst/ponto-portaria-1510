using PontoPortaria1510.Report.Reports;
using System;
using System.IO;

namespace PontoPortaria1510.Report
{
    public static class PdfReport
    {
        public static Stream Gerar(PontoReportDados relatorio)
        {
            switch (relatorio.Tipo)
            {
                case PontoReportTipo.Semanal:
                    return new PontoReportSemanal().Gerar(relatorio);
                default:
                    throw new NotImplementedException();                    
            }
        }
    }
}
