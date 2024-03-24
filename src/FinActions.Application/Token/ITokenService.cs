using FinActions.Contracts.Token;

namespace FinActions.Application.Token;

public interface ITokenService
{
    Task<LoginDto> Gerar(PostTokenDto dadosLogin);
}