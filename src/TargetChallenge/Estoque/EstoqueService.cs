using TargetChallenge.Estoque.Models;

namespace TargetChallenge.Estoque;

public class EstoqueService
{
    public Movimentacao MovimentarEstoque(
        Produto produto,
        TipoMovimentacao tipo,
        int quantidade,
        string descricao)
    {
        if (quantidade <= 0)
        {
            throw new ArgumentException("A quantidade deve ser maior que zero.");
        }
        if (string.IsNullOrWhiteSpace(descricao))
        {
            throw new ArgumentException("A descrição não pode ser vazia.");
        }
        if(tipo == TipoMovimentacao.Saida && 
        quantidade > produto.Estoque)
        {
            throw new InvalidOperationException("Não é possível realizar uma saída maior que o estoque disponível.");
        }

        if (tipo == TipoMovimentacao.Entrada)
        {
            produto.Estoque += quantidade;
        }
        else if (tipo == TipoMovimentacao.Saida)
        {
            produto.Estoque -= quantidade;
        }

        return new Movimentacao
        {
            Id = Guid.NewGuid(),
            CodigoProduto = produto.CodigoProduto,
            Tipo = tipo,
            Quantidade = quantidade,
            Descricao = descricao,
            Data = DateTime.Now
        };
    }
    
}