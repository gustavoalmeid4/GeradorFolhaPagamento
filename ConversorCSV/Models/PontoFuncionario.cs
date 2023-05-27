using ConversorCSV.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversorCSV.Models
{
    public class PontoFuncionario : IFuncionario
    {
        public int Codigo { get; set; }
        public string? Nome { get; set; }

        /// <summary>
        /// Valor da Hora do Funcionario
        /// </summary>
        public string? Valor { get; set; }
        public string? Data { get; set; }
        public string? Entrada { get; set ; }
        public string? Saida { get; set; }
        public string? Almoco { get; set; }

    }
}
