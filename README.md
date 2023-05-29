# Console Application - Folha de Pagamento

Este é um projeto de aplicativo de console desenvolvido em .NET 7 para processar a folha de pagamento de um departamento. O programa requer um arquivo CSV no formato "Departamento de Operações Especiais-Abril-2022.csv" para funcionar corretamente.

## Pré-requisitos

Certifique-se de ter os seguintes itens instalados em sua máquina de desenvolvimento:

- .NET 7 SDK (pode ser baixado em: https://dotnet.microsoft.com/download/dotnet/7.0)

## Executando o programa

Siga as etapas abaixo para executar o programa:

1. Clone este repositório em sua máquina local ou faça o download do arquivo ZIP.

2. Abra o terminal ou prompt de comando e navegue até o diretório raiz do projeto.

3. Execute o seguinte comando para compilar o projeto:

   ```shell
   dotnet build
   ```

4. Certifique-se de ter o arquivo CSV "Departamento de Operações Especiais-Abril-2022.csv" disponível no diretório raiz do projeto.

5. Execute o seguinte comando para iniciar o programa:

   ```shell
   dotnet run --project NomeDoProjeto.csproj "Departamento de Operações Especiais-Abril-2022.csv"
   ```

   Substitua "NomeDoProjeto.csproj" pelo nome do arquivo do projeto real, se necessário.

6. O programa processará o arquivo CSV e exibirá a folha de pagamento do departamento.

## Estrutura do projeto

- `Program.cs`: Contém o ponto de entrada do programa e a lógica principal de processamento.

- `Pagamento.cs`: Definição da classe `Pagamento` que representa um pagamento individual.

- `SistemaFechamentoMes.cs`: Definição da classe `SistemaFechamentoMes` que representa o sistema de fechamento do mês para um departamento.

- `PontoFuncionario.cs`: Definição da classe `PontoFuncionario` que representa um pagamento de cada funcionario.

- `Helper.cs`: Classe de utilitário para processar arquivos CSV e suas classes de cálculo.

## Observações

- Certifique-se de fornecer o arquivo CSV correto ("Departamento de Operações Especiais-Abril-2022.csv") como argumento ao executar o programa. Caso contrário, o programa não será capaz de processar a folha de pagamento corretamente.

- O projeto foi desenvolvido usando .NET 7. Portanto, certifique-se de ter o SDK do .NET 7 instalado em sua máquina antes de executar o programa.

- Este projeto é apenas um exemplo didático e pode não abordar todos os cenários e validações necessários em um sistema de folha de pagamento real.
