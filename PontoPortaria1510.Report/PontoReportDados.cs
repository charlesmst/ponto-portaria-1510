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
        public string Endereco { get; set; }
        public string Pis { get; set; }
        public string BaseHoras { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public String FormatoData { get; set; } = "dd/MM/yyyy";
        public String FormatoHora { get; set; } = "{0:00}:{1:00}";
        
    }
}
