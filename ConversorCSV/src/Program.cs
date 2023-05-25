using ConversorCSV.Helpers;
using ConversorCSV.Models;
using Newtonsoft.Json;

Helpers helpers = new Helpers();

Console.WriteLine("Digite o caminho do arquivo CSV: ");
string? caminhoArquivo = Console.ReadLine();

try
{
    if (!string.IsNullOrEmpty(caminhoArquivo))
        helpers.GetFolhaPagamento(caminhoArquivo);
    else
        Console.WriteLine("Insira um arquivo válido e tente novamente!");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}



