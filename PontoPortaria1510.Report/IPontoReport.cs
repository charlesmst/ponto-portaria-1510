using System.IO;

namespace PontoPortaria1510.Report
{
    interface IPontoReport
    {
        Stream Gerar(PontoReportDados relatorio);
    }
}
