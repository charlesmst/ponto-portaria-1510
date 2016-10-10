using Microsoft.VisualStudio.TestTools.UnitTesting;
using PontoPortaria1510.Totalizador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PontoPortaria1510.Totalizador.Tests
{
    [TestClass()]
    public class TotalizadorTests
    {
        [TestMethod()]
        public void HoraSemanalTest()
        {
            var horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
            
            List<DataPonto> pontos = new List<DataPonto>();

            //Semana 1
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("01/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("13:10	15:00	15:30	18:00	19:00	21:08	22:08	23:40", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 01:35, Credito 00:50, Adicional 01:32
            

            //Semana 2
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("05/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("13:17	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 00:00, Credito 00:00, Adicional 01:00
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("06/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("13:17	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 00:00, Credito 00:00, Adicional 01:00
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("07/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("13:17	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 00:00, Credito 00:00, Adicional 01:00
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("08/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("13:10	15:00	15:30	18:00	19:00	21:08	22:08	23:40", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 01:35, Credito 00:50, Adicional 01:32
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("09/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("13:17	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 00:00, Credito 00:00, Adicional 01:00


            //Semana 3
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("12/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("12:00	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 00:00, Credito 01:15, Adicional 01:00
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("13/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("12:00	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 00:00, Credito 01:15, Adicional 01:00
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("14/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("12:00	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 00:00, Credito 01:15, Adicional 01:00
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("15/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("12:00	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 00:00, Credito 01:15, Adicional 01:00

            //Feriado é horas 100%, não importa que tem horário
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("16/10/2016"),
                Horario = horario,
                TipoData = TipoData.Feriado,
                Batidas = Regex.Split("12:00	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 00:00, Credito 01:15, Adicional 01:00

            pontos = new Calculo.CalculoPonto().CalculaMes(pontos);
            var totalizadores = new Totalizador().HoraSemanal(pontos);
            Assert.AreEqual(4, totalizadores.Count);
            Assert.AreEqual(TimeSpan.FromMinutes(0), totalizadores[0].Horas50);
            Assert.AreEqual(TimeSpan.FromMinutes(0), totalizadores[0].Horas100);
            Assert.AreEqual(TimeSpan.FromMinutes(45), totalizadores[0].Falta);
            Assert.AreEqual(TimeSpan.FromMinutes(92), totalizadores[0].AdicionalNoturno);


            Assert.AreEqual(Convert.ToDateTime("00:00").TimeOfDay, totalizadores[1].Horas50);
            Assert.AreEqual(Convert.ToDateTime("00:00").TimeOfDay, totalizadores[1].Horas100);
            Assert.AreEqual(Convert.ToDateTime("00:45").TimeOfDay, totalizadores[1].Falta);
            Assert.AreEqual(Convert.ToDateTime("04:32").TimeOfDay, totalizadores[1].AdicionalNoturno);

            //Feriado tem horas extra 100% 
            Assert.AreEqual(Convert.ToDateTime("05:00").TimeOfDay, totalizadores[2].Horas50);
            Assert.AreEqual(Convert.ToDateTime("00:00").TimeOfDay, totalizadores[2].Horas100);
            Assert.AreEqual(Convert.ToDateTime("00:00").TimeOfDay, totalizadores[2].Falta);
            Assert.AreEqual(Convert.ToDateTime("05:00").TimeOfDay, totalizadores[2].AdicionalNoturno);

            Assert.AreEqual(Convert.ToDateTime("10:00").TimeOfDay, totalizadores[3].Horas100);


        }
    }

}
