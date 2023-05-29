
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
