# Ponto Portaria 1510/2009
Projeto em C# que calcula o débito, crédito e adicional noturno do dia, levando em conta as regras impostas pela portaria Nº 15.10/2009

Para calcular o crédito, débito e adicional noturno do dia, chamar o método
```csharp
var horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
var batidas = Regex.Split("13:15	17:53	18:56	23:10", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
var diaPonto = new CalculoPonto().CalculaDiaPonto(horario, batidas);
Console.WriteLine(diaPonto.Debito);//00:00:00
Console.WriteLine(diaPonto.Credito);//00:10:00
Console.WriteLine(diaPonto.AdicionalNoturno);//01:10:00
```

Para horários justificados
```csharp
var horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
var batidas = new Batida[] {
	new Batida(Convert.ToDateTime("13:17"),BatidaTipo.Justificada),
	new Batida(Convert.ToDateTime("17:55"),BatidaTipo.Justificada),
	new Batida(Convert.ToDateTime("18:55"),BatidaTipo.Justificada),
	new Batida(Convert.ToDateTime("23:00"),BatidaTipo.Justificada),
};
var diaPonto = new CalculoPonto().CalculaDiaPonto(horario, batidasT);
Console.WriteLine(diaPonto.AdicionalNoturno);
//00:00:00
```

Calcular o mês todo e obter os totalizadores por semana(horas extras, faltas e adicionais noturnos)
```csharp
var calculator =  new CalculoPonto();
List<DataPonto> pontos = new List<DataPonto>();

//Semana 1
pontos.Add(new DataPonto()
{
	Data = Convert.ToDateTime("01/10/2016"),
	Horario = horario,
	Batidas = Regex.Split("13:10	15:00	15:30	18:00	19:00	21:08	22:08	23:40", @"\s+").Select(x => new Batida(Convert.ToDateTime(x), BatidaTipo.Normal)).ToArray()
});//Debito 01:35, Credito 00:50, Adicional 01:32

pontos.Add(new DataPonto()
{
	Data = Convert.ToDateTime("02/10/2016"),
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
...
//Calcula do mês
calculator.CalculaMes(pontos);
//Calcula os Totalizadores
var totalizadores = new Totalizador().HoraSemanal(pontos);
```
# Features
- [x] Cálculo do crédito e débito
- [x] Aplica a tolerância dos 10 minutos no crédito e no débito e 5 minutos nos pontos
- [x] Cálculo de adicional noturno
- [x] Pontos justificados não somam no adicional noturno
- [x] Totalizador por regime de horas extras
- [ ] Relatório PDF do funcionário no regime de horas extras