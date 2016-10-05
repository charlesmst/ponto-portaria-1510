using System;
using System.Collections.Generic;
using System.Linq;

namespace PontoPortaria1510
{
    public class CalculoPonto
    {
        public DiaPonto CalculaDiaPonto(DateTime[] horario, DateTime[] batidas)
        {
            if (horario.Length % 2d != 0d)
                throw new PontoException("Horário inválido");
            if (batidas.Length % 2d != 0d)
                throw new PontoException("Faltam batidas");
            
            var batidasRelacionadas = EncontraBatidaDoHorario(horario, batidas);
            //Só pra evitar brete
            if(batidasRelacionadas.Count  % 2 != 0)
            {
                throw new PontoException("Erro ao encontrar a relação de batidas");
            }
            //FAZENDO HORARIO - BATIDA
            //Para horários Com batida relacionada
            //ENTRADAS  - DEBITO
            //          + CREDITO
            //SAIDA + DEBITO
            //    - CREDITO
            DiaPonto diaPonto = new DiaPonto();
            var relacao = EncontraBatidaDoHorario(horario, batidas);
            var ultimaEntrada = DateTime.MinValue;
            var batidasJaCalculadas = new List<DateTime>();

            //Variáveis para validar se tem um ponto com mais de 5 min
            var credito10Min = true;
            var debito10Min = true;
            
            foreach (var item in relacao.OrderBy(x=>x.Key))
            {
                var res = item.Value.Hora.TimeOfDay.TotalMinutes - item.Key.TimeOfDay.TotalMinutes;//Horário - batida
                if(item.Value.Tipo == PontoTipo.Entrada)
                {
                    ultimaEntrada = item.Key;
                    if (res < 0)
                    {
                        diaPonto.Debito = diaPonto.Debito.Add(TimeSpan.FromMinutes(Math.Abs(res)));
                        if(Math.Abs(res) > 5)
                        {
                            debito10Min = false;
                        }
                    }
                    else if (res > 0)
                    {
                        diaPonto.Credito = diaPonto.Credito.Add(TimeSpan.FromMinutes(res));
                        if (Math.Abs(res) > 5)
                        {
                            credito10Min = false;
                        }
                    }
                }else
                {
                    if (res < 0)
                    {
                        diaPonto.Credito = diaPonto.Credito.Add(TimeSpan.FromMinutes(Math.Abs(res)));
                        if (Math.Abs(res) > 5)
                        {
                            credito10Min = false;
                        }
                    }
                    else if (res > 0)
                    {
                        diaPonto.Debito = diaPonto.Debito.Add(TimeSpan.FromMinutes(res));
                        if (Math.Abs(res) > 5)
                        {
                            debito10Min = false;
                        }
                    }
                }


                //Se é saida, ve todos horários que estão dentro do horário, para ajustar
                if(item.Value.Tipo == PontoTipo.Saida)
                {
                    var dentroHorarios = batidas.Where(x => x.TimeOfDay.TotalMinutes < item.Key.TimeOfDay.TotalMinutes && x.TimeOfDay.TotalMinutes > ultimaEntrada.TimeOfDay.TotalMinutes).ToList();
                    if (dentroHorarios.Count % 2 != 0)//Desnecessáuro,mas não custa nada
                        throw new PontoException("Erro no cálculo, quantidade de pontos dentrodo horário inválido");
                    for (int i = 0; i < dentroHorarios.Count; i = i + 2)
                    {
                        //Batidas no meio já invalidam a regra dos 10 min
                        debito10Min = false;
                        ////BATIDA ENTRE DOIS HORÁRIOS
                        //DEBITO SAIDA -ENTRADA
                        diaPonto.Debito = diaPonto.Debito.Add(TimeSpan.FromMinutes(dentroHorarios[i + 1].TimeOfDay.TotalMinutes - dentroHorarios[i].TimeOfDay.TotalMinutes));
                        batidasJaCalculadas.Add(dentroHorarios[i]);
                        batidasJaCalculadas.Add(dentroHorarios[i+1]);
                    }
                }
                batidasJaCalculadas.Add(item.Key);

            }
            //BATIDA SEM RELACAO	
            //CREDITO FIM -INICIO
            //Pega as batidas fora do horário de serviço
            var batidasNaoCalculadas = batidas.Where(x => !batidasJaCalculadas.Contains(x)).ToList();
            if (batidasNaoCalculadas.Count % 2 != 0)//Desnecessáuro,mas não custa nada
                throw new PontoException("Batida fora do intervalo impar");

            for(int i = 0; i < batidasNaoCalculadas.Count; i = i + 2)
            {
                //Batidas no meio já invalidam a regra dos 10 min
                debito10Min = false;
                credito10Min = false;
                diaPonto.Credito = diaPonto.Credito.Add(TimeSpan.FromMinutes(batidasNaoCalculadas[i + 1].TimeOfDay.TotalMinutes - batidasNaoCalculadas[i].TimeOfDay.TotalMinutes));
            }

            //HORARIO SEM RELAÇÃO	
            //DEBITO FIM -INICIO
            //Seleciona todos horários que estão sem relação
            var horariosNaoRelacionados = horario.Where(x => !relacao.Values.Any(h => h.Hora == x)).ToList();
            if (horariosNaoRelacionados.Count % 2 != 0)//Desnecessáuro,mas não custa nada
                throw new PontoException("Horários fora do intervalor impar");

            for (int i = 0; i < horariosNaoRelacionados.Count; i = i + 2)
            {
                //Batidas no meio já invalidam a regra dos 10 min
                debito10Min = false;
                credito10Min = false;
                diaPonto.Debito = diaPonto.Debito.Add(TimeSpan.FromMinutes(horariosNaoRelacionados[i + 1].TimeOfDay.TotalMinutes - horariosNaoRelacionados[i].TimeOfDay.TotalMinutes));
            }

            #region valida a regra dos 10 minutos
            if(debito10Min && diaPonto.Debito.CompareTo(TimeSpan.FromMinutes(10)) <= 0)
            {
                diaPonto.Debito = TimeSpan.FromMinutes(0);
            }
            if (credito10Min && diaPonto.Credito.CompareTo(TimeSpan.FromMinutes(10)) <= 0)
            {
                diaPonto.Credito = TimeSpan.FromMinutes(0);
            }
            #endregion

            return diaPonto;
        }

        protected Dictionary<DateTime, Horario> EncontraBatidaDoHorario(DateTime[] horarios, DateTime[] batidas)
        {
            //Chave batida, valor horario
            Dictionary<DateTime, Horario> relacao = new Dictionary<DateTime, Horario>();
            Queue<Horario> horariosATestar = new Queue<Horario>();
            #region Alimenta horarios a testar com primeiro as entradas, depois as saidas
            //Primeiro as entradas, depois as saidas
            for (int i = 0; i < horarios.Length; i = i + 2)
            {
                horariosATestar.Enqueue(new Horario(horarios[i], PontoTipo.Entrada));
            }
            for (int i = 1; i < horarios.Length; i = i + 2)
            {
                horariosATestar.Enqueue(new Horario(horarios[i], PontoTipo.Saida));
            }
            #endregion

            while (horariosATestar.Count > 0)
            {
                var horario = horariosATestar.Dequeue();
                Queue<DateTime> pertos = PontosPertos(horario.Hora, batidas, horario.Tipo);
                while (pertos.Count > 0)
                {
                    var batidaAtual = pertos.Dequeue();
                    //Encontra todos que tem relacao antes desse horário
                    int indiceHorarioAtual = Array.IndexOf(horarios, horario.Hora);
                    //Se for batida de saida, verifica se não é menor que o horário de entrada
                    if (horario.Tipo == PontoTipo.Saida)
                    {
                        int iEntrada = indiceHorarioAtual - 1;
                        //Se saiu antes do horário de entrada, não considera do horário, pois foi um ponto fora
                        if (horarios[iEntrada].CompareTo(batidaAtual) > 0)
                        {
                            continue;
                        }

                        //Na saida verifica se existe uma batida de entrada que seja depois do horário de saida, se não pode ser considerada uma batida fora do turno
                        if (batidas.Where((x, i) => i % 2 == 0 && x.CompareTo(horario.Hora) <= 0).Count() == 0)
                            continue;
                    }
                    else
                    {
                        //Reciprocidade também é valida
                        int iSaida = indiceHorarioAtual + 1;
                        //Se saiu antes do horário de entrada, não considera do horário, pois foi um ponto fora
                        if (horarios[iSaida].CompareTo(batidaAtual) < 0)
                        {
                            continue;
                        }
                        //Na entrada verifica se existe uma batida de saida que seja depois do horário de entrada, se não pode ser considerada uma batida fora do turno
                        if (batidas.Where((x, i) => i % 2 != 0 && x.CompareTo(horario.Hora) >= 0).Count() == 0)
                            continue;
                    }


                    //Se já existe a relação, testa qual é o mais perto
                    if (relacao.ContainsKey(batidaAtual))
                    {
                        //Pega o horário que já existe, e testa qual é mais perto
                        var antigo = relacao[batidaAtual];
                        //Se o antigo tem uma diferenca maior que a atual, ajusta o item, e manda recalcular o do horario antigo
                        if (Math.Abs(antigo.Hora.TimeOfDay.TotalMinutes - batidaAtual.TimeOfDay.TotalMinutes) > Math.Abs(horario.Hora.TimeOfDay.TotalMinutes - batidaAtual.TimeOfDay.TotalMinutes))
                        {
                            //Muda e ve se não vai invalidar os horários
                            var old = relacao[batidaAtual];
                            relacao[batidaAtual] = horario;

                            if(invalidaPorInconsistenciaCronologica(horarios,indiceHorarioAtual,batidaAtual,relacao))
                            {
                                relacao[batidaAtual] = old;
                                continue;
                            }
                            else
                            {
                                horariosATestar.Enqueue(antigo);
                                break;
                            }

                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (invalidaPorInconsistenciaCronologica(horarios,indiceHorarioAtual,batidaAtual,relacao))
                            continue;
                        relacao[batidaAtual] = horario;
                        break;
                    }
                }
            }
            return relacao;

        }
        private bool invalidaPorInconsistenciaCronologica(DateTime[] horarios, int iAtual, DateTime batidaAtual, Dictionary<DateTime, Horario> relacao)
        {
            return horarios.Where((x, i) => {
                //i != iAtual
                if (i == iAtual)
                    return false;
                //Se tiver o valor relacionado a um horário faz o teste
                var horaBatida = relacao.FirstOrDefault(y => y.Value.Hora.Equals(x));
                if (horaBatida.Key == DateTime.MinValue)
                    return false;
                return (i < iAtual ? horaBatida.Key.CompareTo(batidaAtual) >= 0 : horaBatida.Key.CompareTo(batidaAtual) <= 0);

            }).Count() > 0;
        }
        protected Queue<DateTime> PontosPertos(DateTime horario, DateTime[] batidas, PontoTipo tipo)
        {
            decimal minutoTotal = Convert.ToDecimal(horario.TimeOfDay.TotalMinutes);
            Queue<DateTime> pertos = new Queue<DateTime>();
            List<DateTime> batidasRestantes = batidas.Where((x, i) =>
            {
                return tipo == PontoTipo.Entrada ? i % 2 == 0 : i % 2 != 0;
            }).ToList();
            while (batidasRestantes.Count > 0)
            {
                decimal diferenca = -1m;

                DateTime? date = null;
                foreach (var b in batidasRestantes)
                {
                    var d = Math.Abs(Convert.ToDecimal(b.TimeOfDay.TotalMinutes) - minutoTotal);
                    if (diferenca == -1m || d < diferenca)
                    {
                        date = b;
                        diferenca = d;
                    }
                }
                batidasRestantes.Remove(date.Value);
                pertos.Enqueue(date.Value);
            }
            return pertos;
        }

    }
}
