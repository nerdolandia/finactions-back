using FinActions.Domain.Usuario;

namespace FinActions.Application.Token;

public interface ITokenService
{
    Task<string> Gerar(Domain.Usuario.Usuario usuario);
}