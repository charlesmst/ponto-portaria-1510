using System;
using System.Collections.Generic;

namespace PontoPortaria1510.Report
{
    public class PontoReportDados
    {
        public string Funcao { get; set; }
        public DateTime Admissao { get; set; }
        public string Funcionario { get; set; }
        public string Empresa { get; set; }
        public string Cnpj { get; set; }
        public string Observacoes { get; set; }
        public List<DataPonto> Pontos { get; set; }
        public PontoReportTipo Tipo { get; set; }
    }
}
