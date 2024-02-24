using System.Security.Claims;

namespace FinActions.Contracts.Token;

public record LoginDto(
    ClaimsIdentity Claims,
    string Token
);