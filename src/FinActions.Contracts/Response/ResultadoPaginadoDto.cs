namespace FinActions.Contracts.Response;

public class ResultadoPaginadoDto<T> where T : class
{
    public int TakeCount { get; set; }
    public int SkipCount { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<T> Items { get; set; } = [];
}