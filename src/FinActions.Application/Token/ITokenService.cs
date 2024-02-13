using FinActions.Contracts.Token;

namespace FinActions.Application.Token;

public interface ITokenService
{
    Task<string> Gerar(PostTokenDto dadosLogin);
}