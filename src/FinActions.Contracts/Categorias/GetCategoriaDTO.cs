using FinActions.Contracts.Request;

namespace FinActions.Contracts.Categorias;

public class GetCategoriaDto : SolicitacaoPaginadaDto
{
    public string? Nome { get; set; }
    public string? Cor { get; set; }
}

