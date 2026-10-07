namespace TargetChallenge.Juros;

public class JurosService
{
    public decimal CalcularJuros(
        decimal valor,
        DateTime dataVencimento,
        DateTime dataAtual)
    {
        if (valor <= 0)
        {
            throw new ArgumentException(
                "O valor deve ser maior que zero."
            );
        }

        int diasAtraso =
            Math.Max(
                0,
                (dataAtual.Date - dataVencimento.Date).Days
            );

        const decimal taxaDiaria = 0.025m;

        decimal juros =
            valor * taxaDiaria * diasAtraso;

        return juros;
    }
}