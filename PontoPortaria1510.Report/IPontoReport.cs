using System.Collections.Generic;
using System.IO;

namespace PontoPortaria1510.Report
{
    public interface IPontoReport
    {
        void Write(List<PontoReportDados> relatorio, Stream stream);
    }
}
