using iTextSharp.text;
using iTextSharp.text.pdf;
using PontoPortaria1510.Totalizador;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PontoPortaria1510.Report.Reports
{
    class PontoReportSemanal : IPontoReport
    {

        public void Write(List<PontoReportDados> relatorio, Stream stream)
        {
            CriaPdf(relatorio, stream);
        }
        private void CriaPdf(List<PontoReportDados> relatorios, Stream stream)
        {

            var calculator = new Calculo.CalculoPonto();
            var totalizador = new Totalizador.Totalizador();
            Document document = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);

            try
            {

                var writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                foreach (var relatorio in relatorios)
                {
                    relatorio.Pontos = calculator.CalculaMes(relatorio.Pontos,relatorio.DataInicio,relatorio.DataFim);
                    var totalizadores = totalizador.HoraSemanal(relatorio.Pontos);
                    var totalizadorMes = totalizador.TotalizadorDoMes(totalizadores);
                    var table = CriaTabela(relatorio, totalizadores, totalizadorMes);


                    document.Add(table);
                }
            }
            finally
            {
                if (document.IsOpen())
                    document.Close();
            }

        }
        private PdfPTable CriaTabela(PontoReportDados relatorio, List<Totalizador.TotalizadorHoraSemanal> totalizadores, TotalizadorHoraSemanal totalizadorMes)
        {
            var fontHeader = FontFactory.GetFont("Arial", 8, Font.BOLD);

            var font = FontFactory.GetFont("Arial", 8);
            var pontos = relatorio.Pontos;
            var table = new PdfPTable(7);
            table.TotalWidth = 800;
            table.LockedWidth = true;
            //Cabeçalho fixo em todas páginas
            table.HeaderRows = 3;
            //Proporcoes das colunas, com base em 26.5 centrimetros
            float[] widths = new float[] { 2.1f / 26.5f, 5f / 26.5f, 8.1f / 26.5f, 1.9f / 26.5f, 3f / 2f / 26.5f, 3f / 2f / 26.5f, 6f / 26.5f };
            table.SetWidths(widths);

            string colunaData = "Data";
            string colunaHorario = "Horário";
            string colunaBatidas = "Batidas";
            string colunaAdNoturno = "Ad. Noturno";
            string colunaDebito = "Falta";
            string colunaCredito = "Extra";
            string colunaObservacao = "Observações";

            //Cria cabeçalho
            #region Cabeçalho

            #region Dados do cabeçalho
            table.AddCell(new PdfPCell(new Phrase("Empresa: "+relatorio.Empresa, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER ,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Colspan = 2
            });
            table.AddCell(new PdfPCell(new Phrase("End: " + relatorio.Endereco, fontHeader))
            {
                Border = Rectangle.TOP_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });

            table.AddCell(new PdfPCell(new Phrase("CNPJ: " + relatorio.Cnpj, fontHeader))
            {
                Border =  Rectangle.TOP_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Colspan = 3
            });
            table.AddCell(new PdfPCell(new Phrase("Base Horas: " + relatorio.BaseHoras, fontHeader))
            {
                Border = Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Colspan = 3
            });
            //Fim linha 1

            table.AddCell(new PdfPCell(new Phrase("Funcionário: " + relatorio.Funcionario, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Colspan = 2
            });
            table.AddCell(new PdfPCell(new Phrase("Função: " + relatorio.Funcao, fontHeader))
            {
                Border = Rectangle.BOTTOM_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });
            table.AddCell(new PdfPCell(new Phrase("PIS: " + relatorio.Pis, fontHeader))
            {
                Border = Rectangle.BOTTOM_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Colspan = 3
            });
            table.AddCell(new PdfPCell(new Phrase("Admissão: " + relatorio.Admissao.ToString(relatorio.FormatoData), fontHeader))
            {
                Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });
            #endregion



            table.AddCell(new PdfPCell(new Phrase(colunaData, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            });
            table.AddCell(new PdfPCell(new Phrase(colunaHorario, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            });
            table.AddCell(new PdfPCell(new Phrase(colunaBatidas, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            });
            table.AddCell(new PdfPCell(new Phrase(colunaAdNoturno, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = 1,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            });
            table.AddCell(new PdfPCell(new Phrase(colunaDebito, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = 1,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            });
            table.AddCell(new PdfPCell(new Phrase(colunaCredito, fontHeader))
            {
                Border = Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = 1,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            });
            table.AddCell(new PdfPCell(new Phrase(colunaObservacao, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            });
            #endregion
           
            TotalizadorHoraSemanal ultimoTotalizador = null;
            //Vai até a última linha mais um, para adicionar o totalizador
            for (int i = 0; i < pontos.Count; i++)
            {
                DataPonto ponto = pontos[i];

                colunaData = ponto.Data.ToString(relatorio.FormatoData) + " " + ponto.Data.DayOfWeek.DiaExtensoAbreviado();
                colunaHorario = ponto.Horario != null? String.Join(" ", ponto.Horario.Select(x => x.TotalHoursFormat(relatorio.FormatoHora))):"";
                colunaBatidas = ponto.Batidas != null? String.Join(" ", ponto.Batidas.Select(x => x.Hora.TotalHoursFormat(relatorio.FormatoHora))):"";
                if (ponto.Ponto != null)
                {
                    colunaAdNoturno = ponto.Ponto.AdicionalNoturno.CompareTo(TimeSpan.FromHours(0)) != 0 ? ponto.Ponto.AdicionalNoturno.TotalHoursFormat(relatorio.FormatoHora) : "";
                    colunaDebito = ponto.Ponto.Debito.CompareTo(TimeSpan.FromHours(0)) != 0 ? ponto.Ponto.Debito.TotalHoursFormat(relatorio.FormatoHora) : "";
                    colunaCredito = ponto.Ponto.Credito.CompareTo(TimeSpan.FromHours(0)) != 0 ? ponto.Ponto.Credito.TotalHoursFormat(relatorio.FormatoHora) : "";
                }
                else
                {
                    colunaAdNoturno = "";
                    colunaDebito = "";
                    colunaCredito = "";
                }
               colunaObservacao = ponto.Observacao;

                if(ponto.TipoData != TipoData.Normal)
                {
                    colunaHorario = ponto.LegendaTipoHorario != null?ponto.LegendaTipoHorario:"";
                }

                #region Formata nas colunas
                table.AddCell(new PdfPCell(new Phrase(colunaData, font))
                {
                    Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER
                });
                table.AddCell(new PdfPCell(new Phrase(colunaHorario, font))
                {
                    Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                });
                table.AddCell(new PdfPCell(new Phrase(colunaBatidas, font))
                {
                    Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                });
                table.AddCell(new PdfPCell(new Phrase(colunaAdNoturno, font))
                {
                    Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    HorizontalAlignment = 1
                });
                table.AddCell(new PdfPCell(new Phrase(colunaDebito, font))
                {
                    Border = Rectangle.LEFT_BORDER,
                    HorizontalAlignment = 1
                });
                table.AddCell(new PdfPCell(new Phrase(colunaCredito, font))
                {
                    Border = Rectangle.RIGHT_BORDER,
                    HorizontalAlignment = 1
                });
                table.AddCell(new PdfPCell(new Phrase(colunaObservacao, font))
                {
                    Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                });
                #endregion


                //Ajusta totalizador
                if (i + 1 < pontos.Count)
                    ponto = pontos[i + 1];
                var totalizador = totalizadores.First(x => x.Inicio.Date.CompareTo(ponto.Data.Date) <= 0 && x.Fim.Date.CompareTo(ponto.Data.Date) >= 0);
                if (ultimoTotalizador == null)
                    ultimoTotalizador = totalizador;

                if (totalizador != ultimoTotalizador || i == (pontos.Count - 1))
                {
                    #region Escreve a linha do totalizador
                    colunaData = "Total Semanal";
                    colunaHorario = ultimoTotalizador.HorasTotalHorario.TotalHoursFormat(relatorio.FormatoHora);
                    colunaBatidas = "";
                    string totalizacoes = "";
                    //Se tem faltas, mostra as faltas
                    if (ultimoTotalizador.Falta.CompareTo(TimeSpan.FromHours(0)) > 0d)
                    {
                        totalizacoes = "Faltas: " + ultimoTotalizador.Falta.TotalHoursFormat(relatorio.FormatoHora);
                    }
                    else if (ultimoTotalizador.Horas50.CompareTo(TimeSpan.FromHours(0)) > 0)
                    {
                        totalizacoes = "Horas 50%: " + ultimoTotalizador.Horas50.TotalHoursFormat(relatorio.FormatoHora);
                    }

                    if (ultimoTotalizador.Horas100.CompareTo(TimeSpan.FromHours(0)) > 0)
                    {
                        totalizacoes += "      Horas 100%: " + ultimoTotalizador.Horas100.TotalHoursFormat(relatorio.FormatoHora);
                    }
                    //coluna
                    #region Formata o totalizador na coluna da tabela
                    table.AddCell(new PdfPCell(new Phrase(colunaData, fontHeader))
                    {
                        Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                        HorizontalAlignment = 1,
                    });
                    table.AddCell(new PdfPCell(new Phrase(colunaHorario, fontHeader))
                    {
                        Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                        HorizontalAlignment = 1,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                    });
                    table.AddCell(new PdfPCell(new Phrase(colunaBatidas, fontHeader))
                    {
                        Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                    });
                    table.AddCell(new PdfPCell(new Phrase(colunaAdNoturno, fontHeader))
                    {
                        Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                        HorizontalAlignment = 1,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                    });
                    table.AddCell(new PdfPCell(new Phrase(totalizacoes, fontHeader))
                    {
                        Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                        Colspan = 3,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                    });
                    #endregion
                    #endregion
                }
                ultimoTotalizador = totalizador;
            }

            //Cria o rodapé
            #region Rodapé
            colunaData = "Total";
            colunaHorario = totalizadorMes.HorasTotalHorario.TotalHoursFormat(relatorio.FormatoHora);
            int diasTrabalhados = pontos.Where(x => x.Batidas != null && x.Batidas.Length > 0).Count();
            colunaBatidas = $"Dias Trabalhados: {diasTrabalhados.ToString("00")} / {pontos.Count.ToString("00")}";
            colunaAdNoturno = totalizadorMes.AdicionalNoturno.TotalHoursFormat(relatorio.FormatoHora);
            string total1 = $"Faltas: {totalizadorMes.Falta.TotalHoursFormat(relatorio.FormatoHora)}";
            string total2 = $"Horas 50%: {totalizadorMes.Horas50.TotalHoursFormat(relatorio.FormatoHora)}";
            string total3 = $"Horas 100%: {totalizadorMes.Horas100.TotalHoursFormat(relatorio.FormatoHora)}";
            string linhaObservacao = relatorio.Observacoes;
            string colunaAssinatura = "Concordo com as marcações acima registradas\n\n Assinatura do Empregado";

            #region Formata o totalizador na coluna da tabela
            table.AddCell(new PdfPCell(new Phrase(colunaData, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = 1,
            });
            table.AddCell(new PdfPCell(new Phrase(colunaHorario, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = 1,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            });
            table.AddCell(new PdfPCell(new Phrase(colunaBatidas, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            });
            table.AddCell(new PdfPCell(new Phrase(colunaAdNoturno, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = 1,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            });
            table.AddCell(new PdfPCell(new Phrase(total1 + "\n" + total2 + "\n" + total3, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                Rowspan = 3,
                Colspan = 3,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            });

            table.AddCell(new PdfPCell(new Phrase(linhaObservacao, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                Rowspan = 5,
                Colspan = 4
            });

            table.AddCell(new PdfPCell(new Phrase(colunaAssinatura, fontHeader))
            {
                Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                Rowspan = 3,
                Colspan = 3,
                HorizontalAlignment = 1
            });
            #endregion
            #endregion
            return table;
        }
    }
}
