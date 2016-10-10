using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static PontoPortaria1510Tests.CustomAsserts;
using System.Text.RegularExpressions;

namespace PontoPortaria1510.Calculo.Tests
{
    [TestClass()]
    public class CalculoPontoTests : CalculoPonto
    {
        [TestMethod()]
        public void CalculaDiaPontoJustificadoTest()
        {
            DateTime[] horario;
            Batida[] batidas;
            ResultadoDiaPonto diaPonto;
            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = new Batida[] {
                 new Batida(Convert.ToDateTime("13:15"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("17:55"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("18:55"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("23:00"),BatidaTipo.Justificada),
            };
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.AdicionalNoturno);



            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = new Batida[] {
                 new Batida(Convert.ToDateTime("13:15"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("13:59"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("14:00"),BatidaTipo.Normal),
                 new Batida(Convert.ToDateTime("17:00"),BatidaTipo.Normal),
                 new Batida(Convert.ToDateTime("17:01"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("17:55"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("18:55"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("23:00"),BatidaTipo.Justificada),
            };
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.AdicionalNoturno);



            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = new Batida[] {
                 new Batida(Convert.ToDateTime("13:15"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("13:30"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("14:00"),BatidaTipo.Normal),
                 new Batida(Convert.ToDateTime("17:00"),BatidaTipo.Normal),
                 new Batida(Convert.ToDateTime("17:01"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("17:55"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("18:55"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("23:00"),BatidaTipo.Justificada),
            };
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(30), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.AdicionalNoturno);



            horario = Regex.Split("13:15	17:55	18:55	21:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = new Batida[] {
                 new Batida(Convert.ToDateTime("10:00"),BatidaTipo.Normal),
                 new Batida(Convert.ToDateTime("12:00"),BatidaTipo.Normal),
                 new Batida(Convert.ToDateTime("18:55"),BatidaTipo.Justificada),
                 new Batida(Convert.ToDateTime("21:00"),BatidaTipo.Justificada),
            };
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(new TimeSpan(4,40,0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromHours(2), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.AdicionalNoturno);
        }
        [TestMethod()]
        public void CalculaDiaPontoTest()
        {
            DateTime[] horario, batidas;
            ResultadoDiaPonto diaPonto;
            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:15	17:53	18:56	23:10", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(10), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromMinutes(70), diaPonto.AdicionalNoturno);


            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromMinutes(60), diaPonto.AdicionalNoturno);
            //Regra dos 10 minutos

            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:17	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromMinutes(60), diaPonto.AdicionalNoturno);


            horario = Regex.Split("10:00	12:00	13:15	17:55", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("08:00	12:00	13:15	17:55", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(120), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.AdicionalNoturno);


            horario = Regex.Split("10:00	12:00	13:15	17:55", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("10:00	11:56	13:27	17:34", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(37), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.AdicionalNoturno);


            horario = Regex.Split("07:30	12:00	13:30	18:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("07:21    11:55   13:04   15:08   15:34   17:55", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(36), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(35), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.AdicionalNoturno);


            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:10	15:00	15:30	18:00	19:00	21:08	22:08	23:40", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(Convert.ToDateTime("01:35").TimeOfDay, diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(50), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromMinutes(92), diaPonto.AdicionalNoturno);


            horario = Regex.Split("05:00	08:00	14:00	17:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("04:10	08:00	15:30	18:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromMinutes(50), diaPonto.AdicionalNoturno);

            //Não deve ter adicional noturno sobre justificativa
            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            var batidasT = Regex.Split("13:17	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Justificada)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidasT);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.AdicionalNoturno);

            
            horario = Regex.Split("13:30	17:30	19:00	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:28    17:31", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromHours(4), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.AdicionalNoturno);



            horario = Regex.Split("13:30	17:30	19:00	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:28    17:36   19:00   23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromHours(0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(6), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromHours(1), diaPonto.AdicionalNoturno);


            horario = Regex.Split("13:30	17:30	19:00	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            batidas = Regex.Split("13:25    17:36   19:00   23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            diaPonto = CalculaDiaPonto(horario, batidas);
            Assert.AreEqual(TimeSpan.FromHours(0), diaPonto.Debito);
            Assert.AreEqual(TimeSpan.FromMinutes(11), diaPonto.Credito);
            Assert.AreEqual(TimeSpan.FromHours(1), diaPonto.AdicionalNoturno);
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




            horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
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
            Assert.AreEqual(relacao.Count, 0);



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

        [TestMethod]
        public void CalculaMesTest()
        {
            //List<DataPonto> datas = new List<DataPonto>();
            //GC.Collect();

            //for (int i = 0; i < 5000; i++)
            //{
            //    datas.Add(new DataPonto()
            //    {
            //        Horario = Regex.Split("07:30	11:55	13:30	17:53", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray(),
            //        Batidas = Regex.Split("12:00	14:00	15:10	18:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray(),
            //        Data = DateTime.Now.AddDays(i)
            //    });
            //}

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //Parallel.ForEach(Partitioner.Create(0, datas.Count), x =>
            //{
            //    for (int i = x.Item1; i < x.Item2; i++)
            //    {
            //        var item = datas[i];
            //        item.Ponto = CalculaDiaPonto(item.Horario, item.Batidas);

            //    }
            //});
            //sw.Stop();

            //datas = new List<DataPonto>();
            //GC.Collect();
            //for (int i = 0; i < 5000; i++)
            //{
            //    datas.Add(new DataPonto()
            //    {
            //        Horario = Regex.Split("07:30	11:55	13:30	17:53", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray(),
            //        Batidas = Regex.Split("12:00	14:00	15:10	18:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray(),
            //        Data = DateTime.Now.AddDays(i)
            //    });
            //}


            //Stopwatch sw2 = new Stopwatch();
            //sw2.Start();
            //foreach (var item in datas)
            //{
            //    item.Ponto = CalculaDiaPonto(item.Horario, item.Batidas);
            //}
            //sw2.Stop();
            //var better = (sw.ElapsedTicks < sw2.ElapsedTicks?"Parallel":"Sequential");
            //Assert.Fail($"Parallel {sw.Elapsed}   Sequencial {sw2.Elapsed}  so {better} is better");

        }
    }
}