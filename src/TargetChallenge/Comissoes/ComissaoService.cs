using TargetChallenge.Comissoes.Models;

namespace TargetChallenge.Comissoes;

public class ComissaoService
{
    public decimal CalcularComissao(decimal valorVenda)
    {
        if (valorVenda < 100m)
            return 0m;

        if (valorVenda < 500m)
            return valorVenda * 0.01m;

        return valorVenda * 0.05m;
    }

    public Dictionary<string, decimal> CalcularComissaoPorVendedor(IEnumerable<Venda> vendas)
    {
        return vendas
            .GroupBy(venda => venda.Vendedor)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Sum(venda => CalcularComissao(venda.Valor))
            );
            //aqui pega todas as vendas, agrupa pelo nome do vendedor e crie um dicionário
            //onde a chave é o vendedor e o valor é a soma das comissões de todas as 
            //vendas daquele vendedor
    }
}

