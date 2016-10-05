using Microsoft.VisualStudio.TestTools.UnitTesting;
using PontoPortaria1510;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PontoPortaria1510Tests.CustomAsserts;
using System.Text.RegularExpressions;

namespace PontoPortaria1510.Tests
{
    [TestClass()]
    public class CalculoPontoTests : CalculoPonto
    {
        [TestMethod()]
        public void CalculaDiaPontoTest()
        {
            DateTime[] horario,batidas;
            DiaPonto diaPonto;
            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:15	17:53	18:56	23:10", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(10), diaPonto.Credito);

            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Credito);

            //Regra dos 10 minutos

            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:17	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Credito);


            horario = Regex.Split("10:00	12:00	13:15	17:55", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("08:00	12:00	13:15	17:55", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(120), diaPonto.Credito);



            horario = Regex.Split("10:00	12:00	13:15	17:55", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("10:00	11:56	13:27	17:34", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(37), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Credito);


            horario = Regex.Split("07:30	12:00	13:30	18:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("07:21    11:55   13:04   15:08   15:34   17:55", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(36), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(35), diaPonto.Credito);

            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:10	15:00	15:30	18:00	19:00	21:08	22:08	23:40", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(Convert.ToDateTime("01:35").TimeOfDay, diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(50), diaPonto.Credito);
        }

        [TestMethod()]
        public void encontraBatidaDoHorarioTest()
        {
            var horario = new DateTime[]
            {
                Convert.ToDateTime("07:30"),
                Convert.ToDateTime("11:55"),
                Convert.ToDateTime("13:30"),
                Convert.ToDateTime("17:53"),
            };
            var batidas = new DateTime[]
            {
                Convert.ToDateTime("07:30"),
                Convert.ToDateTime("12:10"),
                Convert.ToDateTime("13:30"),
                Convert.ToDateTime("17:00"),
            };
            var relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao[batidas[0]], new Horario(horario[0], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[2]], new Horario(horario[2], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[1]], new Horario(horario[1], PontoTipo.Saida));
            Assert.AreEqual(relacao[batidas[3]], new Horario(horario[3], PontoTipo.Saida));


            horario = new DateTime[]
            {
                Convert.ToDateTime("13:10"),
                Convert.ToDateTime("17:55"),
                Convert.ToDateTime("19:00"),
                Convert.ToDateTime("23:00"),
            };
            batidas = new DateTime[]
            {
                Convert.ToDateTime("07:30"),
                Convert.ToDateTime("12:10"),
                Convert.ToDateTime("13:30"),
                Convert.ToDateTime("17:00"),
            };
            relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao[batidas[2]], new Horario(horario[0], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[3]], new Horario(horario[1], PontoTipo.Saida));
            Assert.AreEqual(relacao.Count, 2);




            horario = Regex.Split("13:15	17:55	18:55	23:00",@"\s+").Select(x=>Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:14	17:54	18:57	23:03", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao[batidas[0]], new Horario(horario[0], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[2]], new Horario(horario[2], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[1]], new Horario(horario[1], PontoTipo.Saida));
            Assert.AreEqual(relacao[batidas[3]], new Horario(horario[3], PontoTipo.Saida));
            Assert.AreEqual(relacao.Count, 4);





            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:10	15:00	15:30	18:00	19:00	21:08	22:08	23:40", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao[batidas[0]], new Horario(horario[0], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[3]], new Horario(horario[1], PontoTipo.Saida));
            Assert.AreEqual(relacao[batidas[4]], new Horario(horario[2], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[7]], new Horario(horario[3], PontoTipo.Saida));
            Assert.AreEqual(relacao.Count, 4);



            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("10:00	14:00	15:10	18:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao[batidas[2]], new Horario(horario[0], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[3]], new Horario(horario[1], PontoTipo.Saida));
            Assert.AreEqual(relacao.Count, 2);

            horario = Regex.Split("07:30	11:55	13:30	17:53", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("10:00	14:00	15:10	18:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao[batidas[0]], new Horario(horario[0], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[2]], new Horario(horario[2], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[1]], new Horario(horario[1], PontoTipo.Saida));
            Assert.AreEqual(relacao[batidas[3]], new Horario(horario[3], PontoTipo.Saida));
            Assert.AreEqual(relacao.Count, 4);


            horario = Regex.Split("07:30	11:55	13:30	17:53", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("12:00	14:00	15:10	18:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao.Count, 2);
            Assert.AreEqual(relacao[batidas[0]], new Horario(horario[2], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[3]], new Horario(horario[3], PontoTipo.Saida));



            horario = Regex.Split("07:30	11:55	13:30	17:53", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = new DateTime[0];
            relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao.Count,0);



            horario = Regex.Split("07:30	11:55	13:30	17:53", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("07:00	14:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao.Count, 2);
            Assert.AreEqual(relacao[batidas[0]], new Horario(horario[0], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[1]], new Horario(horario[1], PontoTipo.Saida));


            horario = Regex.Split("12:30	18:30", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("12:00    14:00   14:15    18:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao.Count, 2);
            Assert.AreEqual(relacao[batidas[0]], new Horario(horario[0], PontoTipo.Entrada));
            Assert.AreEqual(relacao[batidas[3]], new Horario(horario[1], PontoTipo.Saida));



            horario = Regex.Split("12:30	18:30", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("07:00    09:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao.Count, 0);

            //Batida fora de turno não tem nenhuma relação
            horario = Regex.Split("07:00	13:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:10    19:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao.Count, 0);

            //Mas se um horário estiver dentro do periodo é válido
            horario = Regex.Split("07:00	13:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:00    19:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            relacao = EncontraBatidaDoHorario(horario, batidas);
            Assert.AreEqual(relacao.Count, 2);
        }
        [TestMethod()]
        public void PontosPertosTest()
        {
            var horario = Convert.ToDateTime("12:00");
            var batidas = new DateTime[]
            {
                Convert.ToDateTime("07:30"),
                Convert.ToDateTime("12:10"),
                Convert.ToDateTime("13:30"),
                Convert.ToDateTime("17:00"),
            };

           AreSameTime(PontosPertos(horario, batidas, PontoTipo.Saida), new DateTime[] {
               batidas[1],batidas[3]
           });
           AreSameTime(PontosPertos(horario, batidas, PontoTipo.Entrada), new DateTime[] {
               batidas[2],batidas[0]
           });


            horario = Convert.ToDateTime("13:00");
            batidas = new DateTime[]
            {
                Convert.ToDateTime("07:30"),
                Convert.ToDateTime("12:10")
            };
            AreSameTime(PontosPertos(horario, batidas, PontoTipo.Entrada), new DateTime[] {
               batidas[0]
           });
        }
    }
}