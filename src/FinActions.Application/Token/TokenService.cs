using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinActions.Domain.Usuario;
using Microsoft.IdentityModel.Tokens;

namespace FinActions.Application.Token;

public class TokenService : ITokenService
{
    public Task<string> Gerar(Domain.Usuario.Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("hadouken123");
        var subject = new ClaimsIdentity();
        subject.AddClaim(new Claim(ClaimTypes.Name, usuario.Nome.ToString()));
        subject.AddClaims(usuario.Papeis
                            .Select(x => new Claim(ClaimTypes.Role, x.Name)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}