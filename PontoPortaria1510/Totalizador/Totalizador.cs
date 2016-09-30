using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PontoPortaria1510.Totalizador
{
    public class Totalizador
    {
        /// <summary>
        /// Calcula a quantidade de horas extras e faltas por semana, que é utilizado onde o regime é hora extra
        /// </summary>
        /// <param name="datas">Datas calculadas do ponto</param>
        /// <returns></returns>
        public List<TotalizadorHoraSemanal> HoraSemanal(List<DataPonto> datas)
        {
            if (datas == null)
                throw new ArgumentNullException(nameof(datas));

            var totalizadores = new List<TotalizadorHoraSemanal>();
            foreach (var data in datas)
            {
                if (data.Ponto == null)
                    throw new PontoException("Batidas devem estar calculadas para obter os totalizadores");
                var totalizador = totalizadores.FirstOrDefault(x => x.Inicio.CompareTo(data.Data) <= 0 && x.Fim.CompareTo(data.Data) >= 0);
                //Se não encontra o totalizador cria
                if(totalizador == null)
                {
                    var primeiroDomingoSemana = data.Data.AddDays(-((int)data.Data.DayOfWeek));
                    var ultimoDia = primeiroDomingoSemana.AddDays(7);
                    totalizador = new TotalizadorHoraSemanal();
                    totalizador.Inicio = primeiroDomingoSemana;
                    totalizador.Fim = ultimoDia;
                    totalizadores.Add(totalizador);
                }
                //Soma todos valores, depois ajusta
                totalizador.Falta = totalizador.Falta.Add(data.Ponto.Debito);
                totalizador.Horas50 = totalizador.Horas50.Add(data.Ponto.Credito);
                totalizador.AdicionalNoturno = totalizador.AdicionalNoturno.Add(data.Ponto.AdicionalNoturno);
            }
            foreach (var totalizador in totalizadores)
            {
                if(totalizador.Horas50.CompareTo(totalizador.Falta) >= 0)
                {
                    totalizador.Horas50 = totalizador.Horas50.Subtract(totalizador.Falta);
                    totalizador.Falta = TimeSpan.FromMinutes(0);
                    //Até 2 horas é hora 50, depois é 100
                    if(totalizador.Horas50.CompareTo(TimeSpan.FromHours(2)) > 0)
                    {
                        totalizador.Horas100 = totalizador.Horas50.Subtract(TimeSpan.FromHours(2));
                        totalizador.Horas50 = TimeSpan.FromHours(2);
                    }
                }else
                {
                    totalizador.Falta = totalizador.Falta.Subtract(totalizador.Horas50);
                    totalizador.Horas50 = TimeSpan.FromMinutes(0);
                }
            }
            return totalizadores;
        }
    }
}
