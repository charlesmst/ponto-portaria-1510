using Microsoft.VisualStudio.TestTools.UnitTesting;
using PontoPortaria1510.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PontoPortaria1510.Report.Tests
{
    [TestClass()]
    public class PdfReportTests
    {
        [TestMethod()]
        public void GerarTest()
        {
            PontoReportDados dados = new PontoReportDados();
            dados.Admissao = DateTime.Now;
            dados.Cnpj = "00000000000000";
            dados.Empresa = "Empresa 1";
            dados.Endereco = "Rua João de Barro";
            dados.Pis = "0000000000000";
            dados.BaseHoras = "220";
            dados.Funcao = "Programador";
            dados.Funcionario = "Charles Michael Stein";
            dados.DataInicio = Convert.ToDateTime("01/10/2016");
            dados.DataFim = Convert.ToDateTime("31/10/2016");
            var horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();

            List<DataPonto> pontos = new List<DataPonto>();

            //Semana 1
            //Quando cria a dataponto do dia
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("01/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("13:10	15:00	15:30	18:00	19:00	21:08	22:08	23:40", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 01:35, Credito 00:50, Adicional 01:32
            //Descanso remunerado, mas veio trabalhar
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("02/10/2016"),
                Horario = null,

                Batidas = Regex.Split("13:10	15:00	15:30	18:00	19:00	21:08	22:08	23:40", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 01:35, Credito 00:50, Adicional 01:32


            //Semana 2
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("04/10/2016"),
                Horario = horario,
                TipoData = TipoData.Feriado,
                LegendaTipoHorario = "Feriado",
                Batidas = Regex.Split("13:17	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 00:00, Credito 00:00, Adicional 01:00
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
            pontos.Add(new DataPonto()
            {
                Data = Convert.ToDateTime("16/10/2016"),
                Horario = horario,
                Batidas = Regex.Split("12:00	17:55	18:55	23:00", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
            });//Debito 00:00, Credito 01:15, Adicional 01:00

            dados.Pontos = pontos;

            var gerador = PdfReport.Gerador(PontoReportTipo.Semanal);
            using (var stream = new FileStream("ponto.pdf", FileMode.Create))
            {
                gerador.Write(new List<PontoReportDados>()
                {
                    dados
                }, stream);
            }
            //dados.Pontos = new List<DataPonto>();
        }
    }
}