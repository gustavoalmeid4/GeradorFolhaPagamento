using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversorCSV.Models
{
    public class Pagamento : PontoFuncionario
    {
        public decimal TotalReceber { get; set; }
        public int HorasExtras { get; set; }
        public int HorasDebito { get; set; }
        public int DiasFalta { get; set; }
        public int DiasExtras { get; set; }
        public int DiasTrabalhados { get; set; }


    }
}
