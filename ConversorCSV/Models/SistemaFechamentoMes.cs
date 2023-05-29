using System;
using System.Collections.Generic;
using System.Linq;
namespace ConversorCSV.Models
{
  public class SistemaFechamentoMes
  {
    public string? Departamento { get; set; }
    public string? MesVigencia { get; set; }
    public string? AnoVigencia { get; set; }
    public decimal TotalPagar { get; set; }
    public decimal TotalDescontos { get; set; }
    public decimal TotalExtras { get; set; }
    public List<Pagamento> Funcionarios { get; set; }
  }
}
