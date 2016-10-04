using Microsoft.VisualStudio.TestTools.UnitTesting;
using PontoPortaria1510;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PontoPortaria1510.Tests
{
    [TestClass()]
    public class JustificativaTests
    {
        [TestMethod()]
        public void AplicaJustificativaTest1()
        {

            List<DataPonto> pontos = new List<DataPonto>();
            var horario = new DateTime[]
            {
                Convert.ToDateTime("07:30"),
                Convert.ToDateTime("11:55"),
                Convert.ToDateTime("13:30"),
                Convert.ToDateTime("17:53"),
            };
            //Semana 1
            //Quando cria a dataponto do dia
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("01/10/2016"),
                Horario = horario,
                Batidas = null
            });

            Justificativa.AplicaJustificativa(pontos, "Teste", Convert.ToDateTime("30/09/2016"), Convert.ToDateTime("01/10/2016 10:00"));

            Assert.AreEqual(2, pontos[0].Batidas.Length);
            Assert.AreEqual(Convert.ToDateTime("07:30").TimeOfDay, pontos[0].Batidas[0].Hora.TimeOfDay);
            Assert.AreEqual(Convert.ToDateTime("10:00").TimeOfDay, pontos[0].Batidas[1].Hora.TimeOfDay);
        }

        [TestMethod()]
        public void AplicaJustificativaTest2()
        {

            List<DataPonto> pontos = new List<DataPonto>();
            var horario = new DateTime[]
            {
                Convert.ToDateTime("07:30"),
                Convert.ToDateTime("11:55"),
                Convert.ToDateTime("13:30"),
                Convert.ToDateTime("17:53"),
            };
            //Semana 1
            //Quando cria a dataponto do dia
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("01/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("12:00	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });

            Justificativa.AplicaJustificativa(pontos, "Teste", Convert.ToDateTime("30/09/2016"), Convert.ToDateTime("01/10/2016 10:00"));

            Assert.AreEqual(6, pontos[0].Batidas.Length);
            Assert.AreEqual(Convert.ToDateTime("07:30").TimeOfDay, pontos[0].Batidas[0].Hora.TimeOfDay);
            Assert.AreEqual(Convert.ToDateTime("10:00").TimeOfDay, pontos[0].Batidas[1].Hora.TimeOfDay);
        }

        [TestMethod()]
        public void AplicaJustificativaTest3()
        {

            List<DataPonto> pontos = new List<DataPonto>();
            var horario = new DateTime[]
            {
                Convert.ToDateTime("11:55"),
                Convert.ToDateTime("18:00"),
                Convert.ToDateTime("19:00"),
                Convert.ToDateTime("23:10"),
            };
            //Semana 1
            //Quando cria a dataponto do dia
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("01/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("12:00	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });

            Justificativa.AplicaJustificativa(pontos, "Teste", Convert.ToDateTime("01/10/2016 07:30"), Convert.ToDateTime("01/10/2016 23:00"));

            var expected = Regex.Split("11:55   11:59   12:00	17:55   17:56   18:00	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            Assert.AreEqual(expected.Length, pontos[0].Batidas.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i].TimeOfDay, pontos[0].Batidas[i].Hora.TimeOfDay);
            }
        }
    }
}