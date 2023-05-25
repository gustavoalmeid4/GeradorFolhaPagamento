using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversorCSV.Interfaces
{
    public interface IFuncionario
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }

        /// <summary>
        /// Valor da Hora do Funcionario
        /// </summary>
        public string Valor { get; set; }
    }
}
