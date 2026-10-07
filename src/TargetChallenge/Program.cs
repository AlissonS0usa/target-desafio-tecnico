using System.Globalization;
using System.Text.Json;

using TargetChallenge.Comissoes;
using TargetChallenge.Comissoes.Models;

using TargetChallenge.Estoque;
using TargetChallenge.Estoque.Models;

using TargetChallenge.Juros;

while (true)
{
    Console.Clear();

    Console.WriteLine("==================================");
    Console.WriteLine("       DESAFIO TARGET SISTEMAS");
    Console.WriteLine("==================================");
    Console.WriteLine("1 - Comissão de vendedores");
    Console.WriteLine("2 - Movimentação de estoque");
    Console.WriteLine("3 - Cálculo de juros");
    Console.WriteLine("0 - Sair");
    Console.WriteLine("==================================");

    Console.Write("Escolha uma opção: ");
    string? opcao = Console.ReadLine();

    Console.Clear();

    switch (opcao)
    {
        case "1":
            ExecutarDesafioComissoes();
            break;

        case "2":
            ExecutarDesafioEstoque();
            break;

        case "3":
            ExecutarDesafioJuros();
            break;

        case "0":
            Console.WriteLine("Programa encerrado.");
            return;

        default:
            Console.WriteLine("Opção inválida.");
            break;
    }

    Console.WriteLine();
    Console.WriteLine("Pressione ENTER para voltar ao menu...");
    Console.ReadLine();
}


/* ==================================
    DESAFIO 1 - COMISSÕES
 ==================================*/

static void ExecutarDesafioComissoes()
{
    string caminhoArquivo = Path.Combine(
        AppContext.BaseDirectory,
        "dados",
        "vendas.json"
    );

    if (!File.Exists(caminhoArquivo))
    {
        Console.WriteLine("Arquivo vendas.json não encontrado.");
        return;
    }

    string json = File.ReadAllText(caminhoArquivo);

    var opcoesJson = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    VendasData? dados = JsonSerializer.Deserialize<VendasData>(
        json,
        opcoesJson
    );

    if (dados is null || dados.Vendas.Count == 0)
    {
        Console.WriteLine("Nenhuma venda encontrada.");
        return;
    }

    var comissaoService = new ComissaoService();

    var comissoes =
        comissaoService.CalcularComissaoPorVendedor(
            dados.Vendas
        );

    Console.WriteLine("==================================");
    Console.WriteLine("      COMISSÃO POR VENDEDOR");
    Console.WriteLine("==================================");

    foreach (var resultado in comissoes)
    {
        Console.WriteLine(
            $"{resultado.Key}: {resultado.Value:C}"
        );
    }
}


/*==================================
  DESAFIO 2 - ESTOQUE
 ==================================*/

static void ExecutarDesafioEstoque()
{
    string caminhoEstoque = Path.Combine(
        AppContext.BaseDirectory,
        "dados",
        "estoque.json"
    );

    if (!File.Exists(caminhoEstoque))
    {
        Console.WriteLine("Arquivo estoque.json não encontrado.");
        return;
    }

    string jsonEstoque =
        File.ReadAllText(caminhoEstoque);

    var opcoesJson = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    EstoqueData? dadosEstoque =
        JsonSerializer.Deserialize<EstoqueData>(
            jsonEstoque,
            opcoesJson
        );

    if (dadosEstoque is null ||
        dadosEstoque.Estoque.Count == 0)
    {
        Console.WriteLine("Nenhum produto encontrado.");
        return;
    }

    Console.WriteLine("==================================");
    Console.WriteLine("       MOVIMENTAÇÃO DE ESTOQUE");
    Console.WriteLine("==================================");

    foreach (var produto in dadosEstoque.Estoque)
    {
        Console.WriteLine(
            $"{produto.CodigoProduto} - " +
            $"{produto.DescricaoProduto} - " +
            $"Estoque: {produto.Estoque}"
        );
    }

    Console.WriteLine();
    Console.Write("Digite o código do produto: ");

    if (!int.TryParse(
            Console.ReadLine(),
            out int codigoProduto))
    {
        Console.WriteLine("Código inválido.");
        return;
    }

    Produto? produtoSelecionado =
        dadosEstoque.Estoque
            .FirstOrDefault(
                p => p.CodigoProduto == codigoProduto
            );

    if (produtoSelecionado is null)
    {
        Console.WriteLine("Produto não encontrado.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine(
        $"Produto: {produtoSelecionado.DescricaoProduto}"
    );

    Console.WriteLine(
        $"Estoque atual: {produtoSelecionado.Estoque}"
    );

    Console.WriteLine();
    Console.WriteLine("1 - Entrada");
    Console.WriteLine("2 - Saída");

    Console.Write(
        "Escolha o tipo de movimentação: "
    );

    if (!int.TryParse(
            Console.ReadLine(),
            out int tipoInformado) ||
        !Enum.IsDefined(
            typeof(TipoMovimentacao),
            tipoInformado))
    {
        Console.WriteLine(
            "Tipo de movimentação inválido."
        );

        return;
    }

    TipoMovimentacao tipo =
        (TipoMovimentacao)tipoInformado;

    Console.Write("Digite a quantidade: ");

    if (!int.TryParse(
            Console.ReadLine(),
            out int quantidade))
    {
        Console.WriteLine("Quantidade inválida.");
        return;
    }

    Console.Write(
        "Digite a descrição da movimentação: "
    );

    string descricao =
        Console.ReadLine() ?? string.Empty;

    var estoqueService =
        new EstoqueService();

    try
    {
        int estoqueAnterior =
            produtoSelecionado.Estoque;

        Movimentacao movimentacao =
            estoqueService.MovimentarEstoque(
                produtoSelecionado,
                tipo,
                quantidade,
                descricao
            );

        Console.WriteLine();
        Console.WriteLine("==================================");
        Console.WriteLine("     MOVIMENTAÇÃO REALIZADA");
        Console.WriteLine("==================================");

        Console.WriteLine(
            $"ID: {movimentacao.Id}"
        );

        Console.WriteLine(
            $"Produto: {produtoSelecionado.DescricaoProduto}"
        );

        Console.WriteLine(
            $"Tipo: {movimentacao.Tipo}"
        );

        Console.WriteLine(
            $"Quantidade: {movimentacao.Quantidade}"
        );

        Console.WriteLine(
            $"Descrição: {movimentacao.Descricao}"
        );

        Console.WriteLine(
            $"Estoque anterior: {estoqueAnterior}"
        );

        Console.WriteLine(
            $"Estoque final: {produtoSelecionado.Estoque}"
        );
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(
            $"Erro: {ex.Message}"
        );
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(
            $"Erro: {ex.Message}"
        );
    }
}


/* ==================================
    DESAFIO 3 - JUROS
   ==================================*/

static void ExecutarDesafioJuros()
{
    Console.WriteLine("==================================");
    Console.WriteLine("        CÁLCULO DE JUROS");
    Console.WriteLine("==================================");

    Console.Write("Digite o valor: ");

    if (!decimal.TryParse(
            Console.ReadLine(),
            NumberStyles.Number,
            CultureInfo.CurrentCulture,
            out decimal valor))
    {
        Console.WriteLine("Valor inválido.");
        return;
    }

    Console.Write(
        "Digite a data de vencimento (dd/MM/yyyy): "
    );

    if (!DateTime.TryParseExact(
            Console.ReadLine(),
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime dataVencimento))
    {
        Console.WriteLine("Data inválida.");
        return;
    }

    DateTime dataAtual =
        DateTime.Today;

    var jurosService =
        new JurosService();

    try
    {
        decimal juros =
            jurosService.CalcularJuros(
                valor,
                dataVencimento,
                dataAtual
            );

        int diasAtraso =
            Math.Max(
                0,
                (dataAtual -
                 dataVencimento.Date).Days
            );

        decimal valorFinal =
            valor + juros;

        Console.WriteLine();
        Console.WriteLine("==================================");
        Console.WriteLine("           RESULTADO");
        Console.WriteLine("==================================");

        Console.WriteLine(
            $"Valor original: {valor:C}"
        );

        Console.WriteLine(
            $"Data de vencimento: {dataVencimento:dd/MM/yyyy}"
        );

        Console.WriteLine(
            $"Data atual: {dataAtual:dd/MM/yyyy}"
        );

        Console.WriteLine(
            $"Dias em atraso: {diasAtraso}"
        );

        Console.WriteLine(
            "Taxa diária: 2,5%"
        );

        Console.WriteLine(
            $"Juros: {juros:C}"
        );

        Console.WriteLine(
            $"Valor final: {valorFinal:C}"
        );
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(
            $"Erro: {ex.Message}"
        );
    }
}