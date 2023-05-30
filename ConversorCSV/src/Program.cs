using ConversorCSV.Helpers;

namespace ConversorCSV
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Services services = new Services();

            Console.WriteLine("===== Gerador de Folha de Pagamento =====");

            Console.WriteLine("Digite o caminho do arquivo CSV: ");
            string? caminhoArquivo = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Digite o caminho de destino da Folha de Pagamento: ");
            string? caminhoDestino = Console.ReadLine();

            bool confirmado = false;
            while (!confirmado)
            {
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("|          Confirmação das Informações      |");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine();
                Console.WriteLine($"Caminho do arquivo CSV a ser processado:  { caminhoArquivo}");
                Console.WriteLine();
                Console.WriteLine($"Caminho do arquivo de destino da Folha de Pagamento:  {caminhoDestino}");
                Console.WriteLine();
                Console.WriteLine("Digite 'S' para confirmar ou 'N' para digitar novamente:");

                string? resposta = Console.ReadLine()?.ToUpper();

                if (resposta == "S")
                {
                    try
                    {
                        Console.WriteLine();
                        Console.WriteLine("========== Processando... ==========");
                        await services.GerarArquivoJsonFromCSV(caminhoArquivo, caminhoDestino);
                        Console.WriteLine();
                        Console.WriteLine($"Arquivo JSON gerado com sucesso em {caminhoDestino.ToUpper()}");
                        Console.WriteLine();
                        Console.WriteLine("========== Obrigado por utilizar o Gerador de Folha de Pagamento! ==========");
                        confirmado = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ocorreu um erro ao gerar o arquivo JSON: {ex.Message}");
                        break;
                    }
                }
                else if (resposta == "N")
                {
                    Console.WriteLine();
                    Console.WriteLine("===== Digite os caminhos novamente =====");
                    Console.WriteLine("Digite o caminho do arquivo CSV: ");
                    caminhoArquivo = Console.ReadLine();

                    Console.WriteLine("Digite o caminho de destino da Folha de Pagamento: ");
                    caminhoDestino = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Resposta inválida. Digite 'S' para confirmar ou 'N' para digitar novamente.");
                }
            }
        }
    }
}
