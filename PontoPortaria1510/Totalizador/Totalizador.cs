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

                //Pode ser a data que o calculomes adicionou só para completar
                if (data.Ponto != null)
                {
                    //Soma todos valores, depois ajusta
                    totalizador.Falta = totalizador.Falta.Add(data.Ponto.Debito);
                    //Se é diferente de normal, é hora 100%
                    if (data.TipoData != TipoData.Normal)
                    {
                        totalizador.Horas100 = totalizador.Horas100.Add(data.Ponto.Credito);
                    }
                    else
                    {
                        totalizador.Horas50 = totalizador.Horas50.Add(data.Ponto.Credito);
                    }
                    totalizador.AdicionalNoturno = totalizador.AdicionalNoturno.Add(data.Ponto.AdicionalNoturno);
                }

                if(data.Horario != null)
                {
                    //Soma no total da semana
                    for (int i = 0; i < data.Horario.Length; i = i + 2)
                    {
                        var saida = data.Horario[i + 1];
                        var entrada = data.Horario[i];
                        totalizador.HorasTotalHorario = totalizador.HorasTotalHorario.Add(saida.Subtract(entrada));
                    }
                }
                         
            }
            foreach (var totalizador in totalizadores)
            {
                //Ou tem horas de 50% ou tem Falta
                if(totalizador.Horas50.CompareTo(totalizador.Falta) >= 0)
                {
                    totalizador.Horas50 = totalizador.Horas50.Subtract(totalizador.Falta);
                    totalizador.Falta = TimeSpan.FromMinutes(0);                  
                }else
                {
                    totalizador.Falta = totalizador.Falta.Subtract(totalizador.Horas50);
                    totalizador.Horas50 = TimeSpan.FromMinutes(0);
                }
            }
            return totalizadores;
        }

        /// <summary>
        /// Soma o total do mes, baseado nos totais semanais
        /// </summary>
        /// <param name="totalizadores"></param>
        /// <returns></returns>
        public TotalizadorHoraSemanal TotalizadorDoMes(List<TotalizadorHoraSemanal> totalizadores)
        {
            if (totalizadores.Count == 0)
                return null;
            var t = new TotalizadorHoraSemanal();
            t.Inicio = totalizadores.Select(x => x.Inicio).Min();
            t.Fim = totalizadores.Select(x => x.Fim).Max();

            foreach (var totalizador in totalizadores)
            {
                t.Falta = t.Falta.Add(totalizador.Falta);
                t.Horas50 = t.Falta.Add(totalizador.Horas50);
                t.Horas100 = t.Falta.Add(totalizador.Horas100);
                t.AdicionalNoturno = t.Falta.Add(totalizador.AdicionalNoturno);
                t.HorasTotalHorario = t.Falta.Add(totalizador.HorasTotalHorario);
            }
            return t;
        }
    }
}
