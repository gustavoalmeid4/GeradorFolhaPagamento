using ConversorCSV.Helpers;

Helpers helpers = new Helpers();

Console.WriteLine("Digite o caminho do arquivo CSV: ");
string? caminhoArquivo = Console.ReadLine();

try
{
  if (!string.IsNullOrEmpty(caminhoArquivo))
    helpers.GetFolhaPagamentoDepartamento(caminhoArquivo);
  else
    Console.WriteLine("Insira um arquivo válido e tente novamente!");
}
catch (Exception ex)
{
  Console.WriteLine(ex.Message);
}



