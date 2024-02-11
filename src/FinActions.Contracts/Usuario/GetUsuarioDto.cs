using FinActions.Contracts.Request;

namespace FinActions.Contracts.Usuario;

public class GetUsuarioDto : SolicitacaoPaginadaDto
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
}

