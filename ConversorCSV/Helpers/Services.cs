using Microsoft.VisualBasic.FileIO;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json;
using ConversorCSV.Models;
using Newtonsoft.Json.Serialization;

namespace ConversorCSV.Helpers
{
    public class Services
    {

        /// <summary>
        /// Retorna um Json do arquivo de Ponto dos Funcionarios CSV
        /// </summary>
        /// <param name="DiretorioArquivoCSV">Diretorio do arquivo CSV</param>
        /// <returns></returns>
        private async Task<List<PontoFuncionario>> ConverteCSVParaJson(string DiretorioArquivoCSV)
        {

            var records = new List<dynamic>();

            using (var parser = new TextFieldParser(DiretorioArquivoCSV, Encoding.GetEncoding("ISO-8859-1")))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                string[] headersDocumento = parser.ReadFields();

                int valorHoraIndex = Array.IndexOf(headersDocumento, "Valor Hora");
                if (valorHoraIndex != -1)
                {
                    headersDocumento[valorHoraIndex] = "ValorHora";
                }

                while (!parser.EndOfData)
                {
                    string[] campos = parser.ReadFields();
                    dynamic record = new ExpandoObject();

                    for (int i = 0; i < campos.Length; i++)
                    {
                        string header = headersDocumento[i];
                        string value = campos[i];
                        ((IDictionary<string, object>)record)[header] = value;
                    }

                    records.Add(record);
                }
            }

            string jsonSerialized = JsonConvert.SerializeObject(records);

            List<PontoFuncionario> result = JsonConvert.DeserializeObject<List<PontoFuncionario>>(jsonSerialized);

            return await Task.FromResult(result);


        }

        /// <summary>
        /// Retorna um Json da folha de pagamento dos Funcionarios.
        /// </summary>
        /// <param name="DiretorioArquivoCSV">Diretorio do arquivo CSV</param>
        /// <returns></returns>
        private async Task<List<Pagamento>> GetFolhaPagamentoFuncionarios(string DiretorioArquivoCSV)
        {
            var listaFolhasPonto = await ConverteCSVParaJson(DiretorioArquivoCSV);

            List<Pagamento> pagamentos = new List<Pagamento>();

            foreach (PontoFuncionario pontoFuncionario in listaFolhasPonto)
            {
                var pagamentoExistente = pagamentos.FirstOrDefault(p => p.Codigo == pontoFuncionario.Codigo);

                if (pagamentoExistente != null)
                {
                    pagamentoExistente.TotalReceber += Convert.ToDecimal(RemoverSimboloMoeda(pontoFuncionario.ValorHora)) * 8;
                    pagamentoExistente.HorasExtras += CalcularHorasExtras(pontoFuncionario);
                    pagamentoExistente.DiasTrabalhados++;
                }
                else
                {
                    Pagamento novoPagamento = new Pagamento()
                    {
                        Codigo = pontoFuncionario.Codigo,
                        Nome = pontoFuncionario.Nome,
                        ValorHora = pontoFuncionario.ValorHora,
                        TotalReceber = Convert.ToDecimal(RemoverSimboloMoeda(pontoFuncionario.ValorHora)) * 8,
                        HorasExtras = CalcularHorasExtras(pontoFuncionario),
                        HorasDebito = 0,
                        DiasFalta = 0,
                        DiasExtras = 0,
                        DiasTrabalhados = 1
                    };

                    pagamentos.Add(novoPagamento);
                }
            }

            return pagamentos;
        }

        /// <summary>
        /// Retorna um Json da folha de pagamento do departamento.
        /// </summary>
        /// <param name="DiretorioArquivoCSV">Diretorio do arquivo CSV.</param>
        /// <returns></returns>
        private async Task<string> GetFolhaPagamentoDepartamento(string DiretorioArquivoCSV)
        {
            List<Pagamento> listaFolhasPonto = await GetFolhaPagamentoFuncionarios(DiretorioArquivoCSV);

            List<SistemaFechamentoMes> pagamentosPorDepartamento = new List<SistemaFechamentoMes>();

            string nomeDepartamento = GetInformacoesDepartamento(DiretorioArquivoCSV, "nomeDepartamento");

            SistemaFechamentoMes sistemaExistente = pagamentosPorDepartamento.FirstOrDefault(s => s.Departamento == nomeDepartamento);
            if (sistemaExistente != null)
            {
                sistemaExistente.Funcionarios.AddRange(listaFolhasPonto);
            }
            else
            {
                SistemaFechamentoMes sistemaFechamento = new SistemaFechamentoMes
                {
                    Departamento = nomeDepartamento,
                    MesVigencia = GetInformacoesDepartamento(DiretorioArquivoCSV, "mesVigencia"),
                    AnoVigencia = GetInformacoesDepartamento(DiretorioArquivoCSV, "anoVigencia"),
                    TotalPagar = CalcularTotalPagarDepartamento(listaFolhasPonto),
                    TotalDescontos = CalcularDescontosDepartamento(listaFolhasPonto),
                    TotalExtras = CalcularHoraExtraDepartamento(listaFolhasPonto),
                    Funcionarios = listaFolhasPonto
                };

                pagamentosPorDepartamento.Add(sistemaFechamento);
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            };

            return JsonConvert.SerializeObject(pagamentosPorDepartamento, settings);
        }

        /// <param name="DiretorioArquivoCSV">Diretorio do arquivo CSV</param>
        /// <param name="valorDepartamento">informação do Departamento</param>
        /// <returns>Retorna nomeDepartamento , anoVigencia ou mesVigencia</returns>
        private string GetInformacoesDepartamento(string DiretorioArquivoCSV, string valorDepartamento)
        {
            string nomeArquivo = Path.GetFileNameWithoutExtension(DiretorioArquivoCSV);
            string[] partesNomeArquivo = nomeArquivo.Split('-');

            if (valorDepartamento.Equals("nomeDepartamento"))
                return partesNomeArquivo[0].Trim();
            if (valorDepartamento.Equals("anoVigencia"))
                return partesNomeArquivo[1].Trim();
            else
                return partesNomeArquivo[2].Trim();

        }

        private string RemoverSimboloMoeda(string valor)
        {
            return valor.Replace("R$", "").Replace(" ", "").Trim();
        }

        private int CalcularHorasExtras(PontoFuncionario pontoFuncionario)
        {
            TimeSpan entrada = TimeSpan.Parse(pontoFuncionario.Entrada);
            TimeSpan saida = TimeSpan.Parse(pontoFuncionario.Saida);
            TimeSpan horasTrabalhadas = saida - entrada;
            TimeSpan horasNormais = TimeSpan.FromHours(8);
            TimeSpan horasExtras = (horasTrabalhadas - CalcularHoraAlmoco(pontoFuncionario.Almoco)) - horasNormais;

            return horasExtras.TotalHours > 0 ? (int)horasExtras.TotalHours : 0;
        }

        private TimeSpan CalcularHoraAlmoco(string horarioAlmoco)
        {
            string[] horarios = horarioAlmoco.Split('-');
            string horaInicioString = horarios[0].Trim();
            string horaFimString = horarios[1].Trim();

            TimeSpan horaInicio = TimeSpan.Parse(horaInicioString);
            TimeSpan horaFim = TimeSpan.Parse(horaFimString);

            return horaFim - horaInicio;

        }

        private decimal CalcularTotalPagarDepartamento(List<Pagamento> listaFolhasPonto)
        {
            decimal valorTotal = 0;
            foreach (var pagamento in listaFolhasPonto)
                valorTotal += pagamento.TotalReceber;

            return valorTotal;
        }

        private decimal CalcularDescontosDepartamento(List<Pagamento> pagamentos)
        {
            decimal totalDescontos = 0;

            foreach (Pagamento pagamento in pagamentos)
            {
                decimal descontosFuncionario = CalcularDescontosFuncionario(pagamento);
                totalDescontos += descontosFuncionario;
            }

            return totalDescontos;
        }

        private decimal CalcularDescontosFuncionario(Pagamento pagamento)
        {
            decimal salarioBase = pagamento.TotalReceber;

            decimal aliquota;
            decimal deducao;

            if (salarioBase <= 1903.98m)
            {
                aliquota = 0m;
                deducao = 0m;
            }
            else if (salarioBase <= 2826.65m)
            {
                aliquota = 0.075m;
                deducao = 142.80m;
            }
            else if (salarioBase <= 3751.05m)
            {
                aliquota = 0.15m;
                deducao = 354.80m;
            }
            else if (salarioBase <= 4664.68m)
            {
                aliquota = 0.225m;
                deducao = 636.13m;
            }
            else
            {
                aliquota = 0.275m;
                deducao = 869.36m;
            }

            decimal descontoPagamento = (salarioBase * aliquota) - deducao;

            string descontoFormatado = descontoPagamento.ToString("0.0");

            return decimal.Parse(descontoFormatado);

        }

        private decimal CalcularHoraExtraDepartamento(List<Pagamento> pagamentos)
        {
            decimal valorHorasExtras = 0;
            decimal totalHorasExtrasFuncionario = 0;
            decimal totalHorasExtrasDepartamento = 0;

            foreach (Pagamento pagamento in pagamentos)
            {
                valorHorasExtras = (Convert.ToDecimal(RemoverSimboloMoeda(pagamento.ValorHora)));
                totalHorasExtrasFuncionario = pagamento.HorasExtras * valorHorasExtras;
                totalHorasExtrasDepartamento += totalHorasExtrasFuncionario;
            }

            return totalHorasExtrasDepartamento;
        }

        public async Task GerarArquivoJsonFromCSV(string DiretorioArquivoCSV, string caminhoArquivoGerado)
        {
            string jsonDepartamento = await this.GetFolhaPagamentoDepartamento(DiretorioArquivoCSV);
            string nomeArquivoFinal = GetInformacoesDepartamento(DiretorioArquivoCSV, "nomeDepartamento");

            await File.WriteAllTextAsync(Path.Combine(caminhoArquivoGerado, $"{nomeArquivoFinal}.json"), jsonDepartamento);
        }
    }

}
