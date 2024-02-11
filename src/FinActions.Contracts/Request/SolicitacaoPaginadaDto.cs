namespace FinActions.Contracts.Request;

public abstract class SolicitacaoPaginadaDto
{
    public int TakeCount { get; set; } = 20;
    public int SkipCount { get; set; }
}
