using System;
using System.Collections.Generic;
using System.Linq;

namespace PontoPortaria1510
{
    public class Justificativa
    {
        public static void AplicaJustificativa(List<DataPonto> ponto, string justificativa, DateTime inicioJustificativa, DateTime fimJustificativa)
        {
            var datasAplicaveis = ponto.Where(x => x.Data.Date.CompareTo(inicioJustificativa.Date) >= 0 && x.Data.Date.CompareTo(fimJustificativa.Date) <= 0);
            foreach (var data in datasAplicaveis)
            {
                if (data.Horario == null)
                    continue;

                List<Batida> justificativas = new List<Batida>();
                //Primeiro aplica a justificativa em cima do horário, para depois remover conforme as batidas
                for (int i = 0; i < data.Horario.Length; i = i + 2)
                {
                    var horaEntrada = data.Horario[i];
                    var horaSaida = data.Horario[i + 1];
                    var entrada = new DateTime(data.Data.Year, data.Data.Month, data.Data.Day, horaEntrada.Hour, horaEntrada.Minute, 0);
                    var saida = new DateTime(data.Data.Year, data.Data.Month, data.Data.Day, horaSaida.Hour, horaSaida.Minute, 0);

                    //Se não tem overlap de datas, avança
                    if (entrada.CompareTo(fimJustificativa) > 0 || inicioJustificativa.CompareTo(saida) > 0)
                        continue;

                    //                   |--------Horário-----------|
                    //              |---justificativa---|
                    //OU
                    //                   |--------Horário-----------|
                    //              |-----------justificativa-------------|
                    if (entrada.CompareTo(inicioJustificativa) >= 0 && entrada.CompareTo(fimJustificativa) <= 0)
                    {
                        //                   |--------Horário-----------|
                        //              |---justificativa---|
                        if (saida.CompareTo(fimJustificativa) > 0)
                        {
                            justificativas.Add(new Batida(entrada, BatidaTipo.Justificada));
                            justificativas.Add(new Batida(fimJustificativa, BatidaTipo.Justificada));
                        }
                        else
                        {
                            //                   |--------Horário-----------|
                            //              |-----------justificativa-------------|
                            justificativas.Add(new Batida(entrada, BatidaTipo.Justificada));
                            justificativas.Add(new Batida(saida, BatidaTipo.Justificada));
                        }
                    }
                    //    |--------Horário-----------|
                    //                      |---justificativa---|
                    else if (saida.CompareTo(inicioJustificativa) >= 0 && saida.CompareTo(fimJustificativa) <= 0)
                    {
                        justificativas.Add(new Batida(inicioJustificativa, BatidaTipo.Justificada));
                        justificativas.Add(new Batida(saida, BatidaTipo.Justificada));
                    }
                    //           |--------Horário-----------|
                    //              |---justificativa---|
                    else
                    {
                        justificativas.Add(new Batida(inicioJustificativa, BatidaTipo.Justificada));
                        justificativas.Add(new Batida(fimJustificativa, BatidaTipo.Justificada));
                    }

                }
                if (data.Batidas != null)
                {
                    //Agora pega as batidas que conflitam as justificativas, e ajusta as justificativas
                    for (int i = 0; i < data.Batidas.Length; i = i + 2)
                    {
                        var batidaHoraEntrada = data.Batidas[i].Hora;
                        var batidaHoraSaida = data.Batidas[i + 1].Hora;
                        var batidaEntradaNormalizada = new DateTime(data.Data.Year, data.Data.Month, data.Data.Day, batidaHoraEntrada.Hour, batidaHoraEntrada.Minute, 0);
                        var batidaSaidaNormalizada = new DateTime(data.Data.Year, data.Data.Month, data.Data.Day, batidaHoraSaida.Hour, batidaHoraSaida.Minute, 0);


                        var inconsistensiasComJustificativas = new List<Batida>();
                        for (int j = 0; j < justificativas.Count; j = j + 2)
                        {
                            if (justificativas[j].Hora.TimeOfDay.CompareTo(batidaSaidaNormalizada.TimeOfDay) <= 0 && justificativas[j + 1].Hora.TimeOfDay.CompareTo(batidaEntradaNormalizada.TimeOfDay) >= 0)
                            {
                                inconsistensiasComJustificativas.Add(justificativas[j]);
                                inconsistensiasComJustificativas.Add(justificativas[j + 1]);
                            }
                        }
                        int minutosDiferenca = 1;//Quando antes ou depois da batida deve ser adicionado
                        for (int j = 0; j < inconsistensiasComJustificativas.Count; j = j + 2)
                        {
                            //                   |--------Batida-----------|
                            //              |---justificativa---|
                            //OU
                            //                   |--------Batida-----------|
                            //              |-----------justificativa-------------|
                            if (batidaHoraEntrada.TimeOfDay.CompareTo(inconsistensiasComJustificativas[j].Hora.TimeOfDay) >= 0 && batidaHoraEntrada.TimeOfDay.CompareTo(inconsistensiasComJustificativas[j + 1].Hora.TimeOfDay) <= 0)
                            {
                                //                   |--------Batida-----------|
                                //              |---justificativa---|
                                if (batidaHoraEntrada.TimeOfDay.CompareTo(inconsistensiasComJustificativas[j + 1].Hora.TimeOfDay) >= 0)
                                {
                                    //                   |--------Batida-----------|
                                    //              |-jus|
                                    inconsistensiasComJustificativas[j + 1].Hora = batidaHoraEntrada.Subtract(TimeSpan.FromMinutes(minutosDiferenca));
                                }
                                //                   |--------Batida-----------|
                                //              |-----------justificativa-------------|
                                else
                                {

                                    //                   |--------Batida-----------|
                                    //              |----|                         |------|

                                    justificativas.Add(new Batida(batidaEntradaNormalizada.Subtract(TimeSpan.FromMinutes(minutosDiferenca)), BatidaTipo.Justificada));
                                    justificativas.Add(new Batida(batidaSaidaNormalizada.Add(TimeSpan.FromMinutes(minutosDiferenca)), BatidaTipo.Justificada));
                                }
                            }
                            //    |--------Batida-----------|
                            //                      |---justificativa---|
                            else if (batidaSaidaNormalizada.TimeOfDay.CompareTo(inconsistensiasComJustificativas[j].Hora.TimeOfDay) >= 0 && batidaSaidaNormalizada.TimeOfDay.CompareTo(inconsistensiasComJustificativas[j + 1].Hora.TimeOfDay) <= 0)
                            {
                                //    |--------Batida-----------|
                                //                              |icativa---|

                                inconsistensiasComJustificativas[j].Hora = batidaSaidaNormalizada.Add(TimeSpan.FromMinutes(minutosDiferenca));

                            }
                            //           |--------Batida-----------|
                            //              |---justificativa---|
                            else
                            {
                                //           |--------Batida-----------|
                                //              xxxxxxxxxxxxxx Elimina
                                justificativas.Remove(inconsistensiasComJustificativas[j]);
                                justificativas.Remove(inconsistensiasComJustificativas[j + 1]);
                            }
                        }
                        //Se tiverem justificativas que terminam antes de começar remove
                        for (int j = 0; j < inconsistensiasComJustificativas.Count; j = j + 2)
                        {
                            if (inconsistensiasComJustificativas[j].Hora.TimeOfDay.CompareTo(inconsistensiasComJustificativas[j + 1].Hora.TimeOfDay) >= 0)
                            {
                                justificativas.Remove(inconsistensiasComJustificativas[j]);
                                justificativas.Remove(inconsistensiasComJustificativas[j+1]);
                            }
                        }
                        justificativas.Sort((x1, x2) => x1.Hora.TimeOfDay.CompareTo(x2.Hora.TimeOfDay));

                    }

                    //Justa as justificativas com as batidas
                    justificativas.AddRange(data.Batidas);
                    justificativas.Sort((x1, x2) => x1.Hora.TimeOfDay.CompareTo(x2.Hora.TimeOfDay));

                }
                data.Batidas = justificativas.ToArray();
                data.Observacao = data.Observacao == null ? justificativa : data.Observacao + ";" + justificativa;
            }
        }
    }
}
