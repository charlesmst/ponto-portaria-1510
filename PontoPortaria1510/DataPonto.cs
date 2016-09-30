using PontoPortaria1510.Calculo;
using System;

namespace PontoPortaria1510
{
    public class DataPonto
    {
        public DateTime Data { get; set; }
        public DateTime[] Horario { get; set; }
        public Batida[] Batidas { get; set; }
        public ResultadoDiaPonto Ponto { get; set; }
        public string Observacao { get; set; }
    }
}
