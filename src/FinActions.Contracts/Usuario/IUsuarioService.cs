using FinActions.Contracts.Response;
using FinActions.Contracts.Token;

namespace FinActions.Contracts.Usuario;

public interface IUsuarioService
{
    Task<UsuarioDto> ObterPorId(Guid id);
    Task<ResultadoPaginadoDto<UsuarioDto>> Obter(GetUsuarioDto parametros);
    Task<UsuarioDto> Atualizar(Guid id, InsertUpdateUsuarioDto insertUsuario);
    Task<UsuarioDto> Excluir(Guid id);
    Task<UsuarioDto> Inserir(InsertUpdateUsuarioDto insertUsuario);
    Task<bool> EhUsuarioExistente(Guid id);
    Task<bool> EhEmailExistente(string email);
    Task<bool> SenhaEhIgual(PostTokenDto dadosLogin);
}