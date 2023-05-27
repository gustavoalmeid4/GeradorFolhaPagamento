using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversorCSV.Models
{
    public class SistemaFechamentoMes
    {
        public string Departamento { get; set; }
        public int MesVigencia { get; set; }
        public int AnoVigencia { get; set; }
        public decimal TotalPagar { get; set; }
        public decimal TotalDescontos { get; set; }
        public decimal TotalExtras { get; set; }
        public dynamic? Funcionarios { get; set; }
    }
}
