using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PontoPortaria1510.Report
{
    static class Extensions
    {
        public static String DiaExtensoAbreviado(this DayOfWeek dia)
        {
            switch (dia)
            {
                case DayOfWeek.Sunday:
                    return "Dom";
                case DayOfWeek.Monday:
                    return "Seg";
                case DayOfWeek.Tuesday:
                    return "Ter";
                case DayOfWeek.Wednesday:
                    return "Qua";
                case DayOfWeek.Thursday:
                    return "Qui";
                case DayOfWeek.Friday:
                    return "Sex";
                case DayOfWeek.Saturday:
                    return "Sáb";
                default:
                    return null;
            }
        }
        /// <summary>
        /// Formata a hora com o total de horas
        /// </summary>
        /// <param name="data"></param>
        /// <param name="format">Formatação normal, com [H] para o total de horas</param>
        /// <returns></returns>
        public static String TotalHoursFormat(this DateTime data, String format)
        {
            return data.TimeOfDay.TotalHoursFormat(format);
        }
        /// <summary>
        /// Formata a hora com o total de horas
        /// </summary>
        /// <param name="data"></param>
        /// <param name="format">Formatação normal, com [H] para o total de horas</param>
        /// <returns></returns>
        public static String TotalHoursFormat(this TimeSpan data, String format)
        {
            return String.Format(format, (int)data.TotalHours,(int)data.Minutes,(int)data.Seconds);
        }
    }
}
