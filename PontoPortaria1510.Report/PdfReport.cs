using PontoPortaria1510.Report.Reports;
using System;
using System.Collections.Generic;
using System.IO;

namespace PontoPortaria1510.Report
{
    public static class PdfReport
    {
        public static IPontoReport Gerador(PontoReportTipo tipo)
        {
            switch (tipo)
            {
                case PontoReportTipo.Semanal:
                    return new PontoReportSemanal();
                default:
                    throw new NotImplementedException();                    
            }
        }
    }
}
