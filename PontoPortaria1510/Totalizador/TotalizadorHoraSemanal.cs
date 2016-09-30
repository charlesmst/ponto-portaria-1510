using System;

namespace PontoPortaria1510.Totalizador
{
    public class TotalizadorHoraSemanal
    {
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public TimeSpan Falta { get; set; } = TimeSpan.FromMinutes(0);
        public TimeSpan Horas50 { get; set; } = TimeSpan.FromMinutes(0);
        public TimeSpan Horas100 { get; set; } = TimeSpan.FromMinutes(0);
        public TimeSpan AdicionalNoturno { get; set; } = TimeSpan.FromMinutes(0);
    }
}
