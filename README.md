# Ponto Portaria 1510/2009
Projeto em C# que calcula o débito, crédito e adicional noturno do dia, levando em conta as regras impostas pela portaria Nº 15.10/2009

Para cálcular o crédito, débito e adicional noturno do dia, chamar o método
```csharp
var horario = Regex.Split("13:15	17:55	18:55	23:00", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
var batidas = Regex.Split("13:15	17:53	18:56	23:10", @"\s+").Select(x => Convert.ToDateTime(x)).ToArray();
var diaPonto = new CalculoPonto().CalculaDiaPonto(horario, batidas);
Assert.AreEqual(TimeSpan.FromMinutes(0), diaPonto.Debito);
Assert.AreEqual(TimeSpan.FromMinutes(10), diaPonto.Credito);
```