using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinActions.Contracts.Token;
using FinActions.Domain.AppSettings;
using FinActions.Domain.Usuarios;
using FinActions.Domain.Usuarios.Papeis;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinActions.Application.Token;

public class TokenService : ITokenService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly JwtOptions _jwtOptions;

    public TokenService(IUsuarioRepository usuarioRepository, IOptions<JwtOptions> jwtOptions)
    {
        _usuarioRepository = usuarioRepository;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<string> Gerar(PostTokenDto dadosLogin)
    {
        var usuario = await _usuarioRepository.ObterPorEmail(dadosLogin.Email);

        var key = Encoding.ASCII.GetBytes(_jwtOptions.SecurityKey);

        var subject = new ClaimsIdentity();
        subject.AddClaim(new Claim(nameof(Domain.Usuarios.Usuario.Id), usuario.Id.ToString()));
        subject.AddClaim(new Claim(nameof(Domain.Usuarios.Usuario.Nome), usuario.Nome));
        subject.AddClaim(new Claim(ClaimTypes.Name, usuario.Nome));
        subject.AddClaim(new Claim(nameof(Domain.Usuarios.Usuario.Email), usuario.Email));
        subject.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
        subject.AddClaims(usuario.Papeis
                            .Select(x => new Claim(ClaimTypes.Role, x.Nome)));
        subject.AddClaims(usuario.Papeis
                            .Select(x => new Claim(nameof(Papel), x.Nome)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _jwtOptions.Audience,
            Issuer = _jwtOptions.Issuer,
            Subject = subject,
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                        SecurityAlgorithms.HmacSha256Signature),
            Expires = DateTime.UtcNow.AddSeconds(_jwtOptions.ExpirationInSeconds),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}