using FinActions.Application.Token;
using FinActions.Contracts.Response;
using FinActions.Contracts.Usuario;
using FinActions.Domain.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinActions.HttpApi.Host.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ITokenService _tokenService;

    public UsuarioController(
        IUsuarioService usuarioService,
        ITokenService tokenService)
    {
        _usuarioService = usuarioService;
        _tokenService = tokenService;
    }

    [HttpGet("")]
    public async Task<ResultadoPaginadoDto<UsuarioDto>> GetAsync(
        [FromQuery] GetUsuarioDto parametros)
    {
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
}