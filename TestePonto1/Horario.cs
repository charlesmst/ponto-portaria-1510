using System;

namespace PontoPortaria1510
{
    public class Horario : IEquatable<Horario>
    {
        public Horario(DateTime hora, PontoTipo tipo)
        {
            this.Hora = hora;
            this.Tipo = tipo;
        }

        public readonly DateTime Hora;
        public readonly PontoTipo Tipo;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Horario)obj);
        }
        // override object.Equals
        public bool Equals(Horario other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return DateTime.Equals(Hora, other.Hora) && Tipo == other.Tipo;

        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return  string.Format("{0}_{1}", Hora.ToLongDateString(), ((int)Tipo).ToString()).GetHashCode();;
        }

       
    }
}