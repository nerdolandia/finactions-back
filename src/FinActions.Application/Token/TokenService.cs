using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinActions.Contracts.Token;
using FinActions.Domain.AppSettings;
using FinActions.Domain.Usuarios;
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

    public async Task<LoginDto> Gerar(PostTokenDto dadosLogin)
    {
        var usuario = await _usuarioRepository.ObterPorEmail(dadosLogin.Email);

        var key = Encoding.ASCII.GetBytes(_jwtOptions.SecurityKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Email, usuario.Email)
        };
        claims.AddRange(usuario.Papeis.Select(x => new Claim(ClaimTypes.Role, x.Nome)));

        var identity = new ClaimsIdentity(claims);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _jwtOptions.Audience,
            Issuer = _jwtOptions.Issuer,
            Subject = identity,
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                        SecurityAlgorithms.HmacSha256Signature),
            Expires = DateTime.UtcNow.AddSeconds(_jwtOptions.ExpirationInSeconds),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        string token = tokenHandler.WriteToken(securityToken);

        return new LoginDto(identity, token);
    }
}