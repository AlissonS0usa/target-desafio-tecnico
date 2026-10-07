namespace TargetChallenge.Estoque.Models;

public enum TipoMovimentacao
{
    Entrada = 1,
    Saida = 2
}

public class Movimentacao
{
    public Guid Id { get; set;}
    public int CodigoProduto { get; set; }

    public TipoMovimentacao Tipo { get; set; }

    public int Quantidade { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public DateTime Data { get; set; }
}