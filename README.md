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

Para horários justificados, fazer a chamada criando um array de objetos Batida, com o tipo Justificada
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

# Features
- Cálculo do crédito e débito
- Aplica a tolerância dos 10 minutos no crédito e no débito e 5 minutos nos pontos
- Cálculo de adicional noturno
- Pontos justificados não somam no adicional noturno