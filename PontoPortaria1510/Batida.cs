using System;

namespace PontoPortaria1510
{
    public class Batida
    {
        public DateTime Hora { get; set; }
        public BatidaTipo Tipo { get; set; }
        public Batida(DateTime hora, BatidaTipo tipo)
        {
            this.Hora = hora;
            this.Tipo = tipo;
        }
    }
}
