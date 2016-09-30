using System;

namespace PontoPortaria1510.Calculo
{
    public class ResultadoDiaPonto
    {
        public TimeSpan Debito { get; set; } = TimeSpan.FromMinutes(0);
        public TimeSpan Credito { get; set; } = TimeSpan.FromMinutes(0);
        public TimeSpan AdicionalNoturno { get; set; } = TimeSpan.FromMinutes(0);
    }
}
