# Console Application - Folha de Pagamento

Este é um projeto de aplicativo de console desenvolvido em .NET 7 para processar a folha de pagamento de um departamento.

## Pré-requisitos

Certifique-se de ter os seguintes itens instalados em sua máquina de desenvolvimento:

- .NET 7 SDK (pode ser baixado em: https://dotnet.microsoft.com/download/dotnet/7.0)

- A aplicação requer um arquivo CSV no formato "Departamento de Operações Especiais-Abril-2022.csv" para funcionar corretamente.

- As colunas do arquivo CSV devem ser separadas por ";" ex: Codigo;Nome;Valor Hora;Data;Entrada;Saida;Almoco

- A coluna Valor , deve conter o Simbolo monetário. Exemplo : R$ 100,00. 

- Não deve conter Caracteres Especiais nos nomes das Colunas Exemplo: Código , deve ser informado como Codigo.

- Exemplo da estrutura do arquivo : [exemplo](https://prnt.sc/vMZ_k25KFDSR)

## Executando o programa

Siga as etapas abaixo para executar o programa:

1. Ao iniciar a aplicação , deverá ser informado o Caminho do arquivo CSV que vai ser processado e o Caminho de destino da Folha de Pagamento.
2. Após a execução do programada , o arquivo Json processado pode ser encontrado no caminho que foi passado na instrução acima .

## Observações

- Certifique-se de fornecer o arquivo CSV correto ex:("Departamento de Operações Especiais-Abril-2022.csv") como argumento ao executar o programa. Caso contrário, o programa não será capaz de processar a folha de pagamento corretamente.

- O projeto foi desenvolvido usando .NET 7. Portanto, certifique-se de ter o SDK do .NET 7 instalado em sua máquina antes de executar o programa.

- Este projeto é apenas um exemplo didático e pode não abordar todos os cenários e validações necessários em um sistema de folha de pagamento real.

##Funcionamento

