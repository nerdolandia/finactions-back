using FinActions.Application.Token;
using FinActions.Application.Usuario;
using FinActions.Contracts.Response;
using FinActions.Contracts.Token;
using FinActions.Contracts.Usuario;
using FinActions.Domain.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace FinActions.HttpApi.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(
        IUsuarioService usuarioService,
        ITokenService tokenService,
        ILogger<UsuarioController> logger)
    {
        _usuarioService = usuarioService;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<ResultadoPaginadoDto<UsuarioDto>> GetAsync(
        [FromQuery] GetUsuarioDto parametros)
    {
        _logger.LogInformation("{message}", User.Claims);
        return await _usuarioService.Obter(parametros);
    }

    [HttpPost("")]
    public async Task<IActionResult> PostAsync(
        [FromBody] InsertUpdateUsuarioDto insertUsuario
    )
    {
        if (await _usuarioService.EhEmailExistente(insertUsuario.Email))
            return BadRequest(UsuarioConsts.ErroJaExiste);

        return Ok(await _usuarioService.Inserir(insertUsuario));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(
        [FromRoute] Guid id,
        [FromBody] InsertUpdateUsuarioDto insertUsuario
    )
    {
        if (await _usuarioService.EhEmailExistente(insertUsuario.Email))
            return BadRequest(UsuarioConsts.ErroEmailJaCadastrado);

        return Ok(await _usuarioService.Atualizar(id, insertUsuario));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        if (!await _usuarioService.EhUsuarioExistente(id))
            return NotFound(UsuarioConsts.ErroNaoExiste);

        return Ok(await _usuarioService.Excluir(id));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        if (!await _usuarioService.EhUsuarioExistente(id))
            return NotFound(UsuarioConsts.ErroNaoExiste);

        return Ok(await _usuarioService.ObterPorId(id));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] PostTokenDto dadosLogin)
    {
        if (!await _usuarioService.EhEmailExistente(dadosLogin.Email))
            return BadRequest(UsuarioConsts.ErroNaoExiste);

        if (!await _usuarioService.SenhaEhIgual(dadosLogin))
            return Unauthorized(UsuarioConsts.ErroSenhaIncorreta);

        return Ok(await _tokenService.Gerar(dadosLogin));
    }
}