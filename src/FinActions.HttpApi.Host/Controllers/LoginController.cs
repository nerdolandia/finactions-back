using FinActions.Application.Token;
using FinActions.Contracts.Token;
using FinActions.Contracts.Usuario;
using FinActions.Domain.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace FinActions.HttpApi.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ITokenService _tokenService;

    public LoginController(IUsuarioService usuarioService, ITokenService tokenService)
    {
        _usuarioService = usuarioService;
        _tokenService = tokenService;
    }

    [HttpPost("")]
    public async Task<IActionResult> PostAsync([FromBody] PostTokenDto dadosLogin)
    {
        if (!await _usuarioService.EhEmailExistente(dadosLogin.Email))
            return BadRequest(UsuarioConsts.ErroNaoExiste);

        if (!await _usuarioService.SenhaEhIgual(dadosLogin))
            return Unauthorized(UsuarioConsts.ErroSenhaIncorreta);

        var loginDto = await _tokenService.Gerar(dadosLogin);

        return Ok(loginDto.Token);
    }
}