# Desafio Técnico - Target Sistemas

A solução foi implementada em **C# com .NET 8**, utilizando aplicação de console, leitura de arquivos JSON e separação das regras de negócio por domínio.

O desafio é composto por três exercícios:

1. Cálculo de comissão de vendedores;
2. Movimentação de estoque;
3. Cálculo de juros por atraso.

---

## Tecnologias utilizadas

- C#
- .NET 8
- System.Text.Json
- LINQ
- Git

---

## Estrutura do projeto

```text
target-desafio-tecnico/
│
├── README.md
├── TargetChallenge.sln
│
├── src/
│   ├── TargetChallenge/
│   │   ├── Program.cs
│   │   │
│   │   ├── Comissoes/
│   │   │   ├── Models/
│   │   │   │   ├── Venda.cs
│   │   │   │   └── VendasData.cs
│   │   │   └── ComissaoService.cs
│   │   │
│   │   ├── Estoque/
│   │   │   ├── Models/
│   │   │   │   ├── Produto.cs
│   │   │   │   ├── Movimentacao.cs
│   │   │   │   └── EstoqueData.cs
│   │   │   └── EstoqueService.cs
│   │   │
│   │   └── Juros/
│   │       └── JurosService.cs
│   │
│   └── dados/
│       ├── vendas.json
│       └── estoque.json
```

A aplicação foi organizada de forma que o `Program.cs` seja responsável principalmente pela interação com o usuário, enquanto as regras de negócio ficam separadas em serviços específicos.

---

# Exercício 1 - Comissão de vendedores

O programa realiza a leitura dos registros de vendas a partir de um arquivo JSON e calcula a comissão total de cada vendedor.

As regras utilizadas são:

| Valor da venda | Comissão |
|---|---:|
| Menor que R$ 100,00 | 0% |
| A partir de R$ 100,00 e menor que R$ 500,00 | 1% |
| A partir de R$ 500,00 | 5% |

A aplicação agrupa as vendas por vendedor utilizando LINQ e calcula a comissão individualmente para cada venda antes de realizar a soma final.

### Exemplo

```text
==================================
      COMISSÃO POR VENDEDOR
==================================

João Silva: R$ 495,68
Maria Souza: R$ 465,95
Carlos Oliveira: R$ 379,37
Ana Lima: R$ 404,98
```

### Decisão de implementação

Foi utilizado o tipo `decimal` para valores monetários, evitando problemas de precisão comuns em operações financeiras com tipos de ponto flutuante.

---

# Exercício 2 - Movimentação de estoque

O programa permite realizar movimentações de:

- Entrada de produtos;
- Saída de produtos.

Os dados iniciais dos produtos são carregados a partir do arquivo `estoque.json`.

Cada movimentação possui:

- Identificador único;
- Código do produto;
- Tipo de movimentação;
- Quantidade;
- Descrição;
- Data da movimentação.

O identificador é gerado utilizando `Guid`.

### Regras implementadas

A aplicação também realiza algumas validações para manter a consistência do estoque:

- A quantidade da movimentação deve ser maior que zero;
- A descrição da movimentação é obrigatória;
- Uma saída não pode ser maior que o estoque disponível;
- O estoque não pode ficar negativo.

### Exemplo

```text
Produto: Caneta Azul
Estoque atual: 150

1 - Entrada
2 - Saída

Escolha o tipo de movimentação: 2
Digite a quantidade: 20
Digite a descrição da movimentação: Venda pedido 123

==================================
     MOVIMENTAÇÃO REALIZADA
==================================

Produto: Caneta Azul
Tipo: Saida
Quantidade: 20
Estoque anterior: 150
Estoque final: 130
```

### Persistência

As movimentações realizadas alteram o estoque durante a execução da aplicação.

O desafio não solicita persistência das alterações, portanto o arquivo JSON original não é modificado.

Ao reiniciar a aplicação, os valores iniciais são carregados novamente.

---

# Exercício 3 - Cálculo de juros

O terceiro exercício calcula os juros de um valor vencido considerando uma taxa de **2,5% ao dia**.

A fórmula utilizada foi:

```text
Juros = Valor × Taxa diária × Dias em atraso
```

A taxa diária utilizada é:

```text
2,5% = 0,025
```

### Exemplo

Para:

```text
Valor: R$ 1.000,00
Dias em atraso: 10
```

O cálculo será:

```text
1000 × 0,025 × 10 = 250
```

Resultado:

```text
Juros: R$ 250,00
Valor final: R$ 1.250,00
```

Caso a data de vencimento seja igual ou posterior à data atual, não são aplicados juros.

---

# Menu da aplicação

Os três exercícios foram integrados em uma única aplicação de console.

Ao executar o projeto, é apresentado o seguinte menu:

```text
==================================
       DESAFIO TARGET SISTEMAS
==================================

1 - Comissão de vendedores
2 - Movimentação de estoque
3 - Cálculo de juros
0 - Sair
```

Cada opção executa o exercício correspondente e, ao finalizar, permite retornar ao menu principal.

---

# Como executar

## Pré-requisitos

É necessário possuir o **.NET 8 SDK** instalado.

Para verificar:

```bash
dotnet --version
```

---

## Clonar o repositório

```bash
git clone https://github.com/AlissonS0usa/target-desafio-tecnico.git
```

Entre na pasta:

```bash
cd target-desafio-tecnico
```

---

## Restaurar dependências

```bash
dotnet restore
```

---

## Compilar

```bash
dotnet build
```

---

## Executar

```bash
dotnet run --project src/TargetChallenge
```

---

# Organização da solução

A solução foi dividida em três domínios principais:

```text
Comissoes
Estoque
Juros
```

Cada domínio possui sua própria regra de negócio.

Essa separação evita concentrar toda a implementação dentro do `Program.cs` e facilita a leitura, manutenção e evolução do código.

O fluxo simplificado da aplicação é:

```text
Program.cs
   │
   ├── Comissão
   │      └── ComissaoService
   │
   ├── Estoque
   │      └── EstoqueService
   │
   └── Juros
          └── JurosService
```

---

# Principais decisões técnicas

Durante o desenvolvimento foram adotadas algumas decisões para manter a solução simples e organizada:

- Uso de `decimal` para valores monetários;
- Uso de `System.Text.Json` para leitura dos arquivos JSON;
- Uso de LINQ para agrupamento das vendas;
- Uso de `Guid` para geração de identificadores únicos;
- Uso de `enum` para representar os tipos de movimentação;
- Separação das regras de negócio em serviços;
- Validação das entradas do usuário;
- Tratamento de erros relacionados às regras de negócio;
- Uso de `DateTime.Date` para evitar influência do horário no cálculo dos dias de atraso.

---

# Considerações finais

A solução foi desenvolvida buscando atender aos requisitos do desafio com foco em:

- Clareza;
- Organização;
- Legibilidade;
- Separação de responsabilidades;
- Validação das regras de negócio;
- Facilidade de manutenção.

A implementação foi mantida propositalmente simples, evitando adicionar tecnologias ou dependências que não fossem necessárias para o escopo proposto.