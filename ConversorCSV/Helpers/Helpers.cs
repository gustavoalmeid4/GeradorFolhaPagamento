using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using ConversorCSV.Models;
using System.Globalization;
using System.Drawing;

namespace ConversorCSV.Helpers
{
    public class Helpers
    {

        /// <summary>
        /// Retorna um Json do arquivo de Ponto dos Funcionarios CSV
        /// </summary>
        /// <param name="caminhoCSV"></param>
        /// <returns></returns>
        public List<PontoFuncionario> ConverteCSVParaJson(string caminhoCSV)
        {

            var records = new List<dynamic>();

            using (var parser = new TextFieldParser(caminhoCSV, Encoding.GetEncoding("ISO-8859-1")))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                string[] headers = parser.ReadFields();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    dynamic record = new ExpandoObject();

                    for (int i = 0; i < fields.Length; i++)
                    {
                        string header = headers[i];
                        string value = fields[i];
                        ((IDictionary<string, object>)record)[header] = value;
                    }

                    records.Add(record);
                }
            }

            string jsonSerialized = await JsonConvert.SerializeObject(records);

            return JsonConvert.DeserializeObject<List<PontoFuncionario>>(jsonSerialized);

            
        }

        /// <summary>
        /// Retorna um Json da Folha de Pagamento dos Funcionarios
        /// </summary>
        /// <param name="caminhoCSV"></param>
        /// <returns></returns>
        public async string GetFolhaPagamento(string caminhoCSV)
        {
            
            var listaFolhasPonto =  await ConverteCSVParaJson(caminhoCSV);

            // Dicionário para armazenar os pagamentos de cada funcionário
            Dictionary<int, Pagamento> pagamentosPorFuncionario = new Dictionary<int, Pagamento>();

            // Percorrer a lista de folhas de ponto e preencher os modelos de pagamento correspondentes
            foreach (PontoFuncionario pontoFuncionario in listaFolhasPonto)
            {
                if (pagamentosPorFuncionario.ContainsKey(pontoFuncionario.Codigo))
                {
                    Pagamento pagamentoExistente = pagamentosPorFuncionario[pontoFuncionario.Codigo];
                    pagamentoExistente.TotalReceber += Convert.ToDecimal(RemoverSimboloMoeda(pontoFuncionario.Valor));
                    pagamentoExistente.HorasExtras += CalcularHorasExtras(pontoFuncionario);
                    pagamentoExistente.DiasTrabalhados++;
                }
                else
                {
                    Pagamento novoPagamento = new Pagamento
                    {
                        Codigo = pontoFuncionario.Codigo,
                        Nome = pontoFuncionario.Nome,
                        Valor = pontoFuncionario.Valor,
                        TotalReceber = Convert.ToDecimal(RemoverSimboloMoeda(pontoFuncionario.Valor)),
                        HorasExtras = CalcularHorasExtras(pontoFuncionario),
                        HorasDebito = 0,
                        DiasFalta = 0,
                        DiasExtras = 0,
                        DiasTrabalhados = 1
                    };

                    pagamentosPorFuncionario[pontoFuncionario.Codigo] = novoPagamento;
                }
            }

            // Converter os pagamentos para JSON
            //Console.WriteLine(JsonConvert.SerializeObject(pagamentosPorFuncionario, Formatting.Indented));
            return JsonConvert.SerializeObject(pagamentosPorFuncionario, Formatting.Indented);

        }

        // Função para remover o símbolo de moeda do valor monetário
        public string RemoverSimboloMoeda(string valor)
        {
            return valor.Replace("R$", "").Replace(" ","").Trim();
        }

        // Função para calcular as horas extras com base em uma folha de ponto
        public int CalcularHorasExtras(PontoFuncionario pontoFuncionario)
        {
            TimeSpan entrada = TimeSpan.Parse(pontoFuncionario.Entrada);
            TimeSpan saida = TimeSpan.Parse(pontoFuncionario.Saida);
            TimeSpan horasTrabalhadas = saida - entrada;
            TimeSpan horasNormais = TimeSpan.FromHours(8);
            TimeSpan horasExtras = (horasTrabalhadas - CalculaHoraAlmoco(pontoFuncionario.Almoco)) - horasNormais;

            return horasExtras.TotalHours > 0 ? (int)horasExtras.TotalHours : 0;
        }

        static TimeSpan CalculaHoraAlmoco(string horarioAlmoco)
        {
            string[] horarios = horarioAlmoco.Split('-');
            string horaInicioString = horarios[0].Trim();
            string horaFimString = horarios[1].Trim();

            TimeSpan horaInicio = TimeSpan.Parse(horaInicioString);
            TimeSpan horaFim = TimeSpan.Parse(horaFimString);

            return horaFim - horaInicio;

        }



    }

}
