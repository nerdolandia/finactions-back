using FinActions.Domain.Base;

namespace FinActions.Domain.Usuarios;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<Usuario> ObterPorEmail(string email);

    Task<bool> EhEmailExistente(string email);

    Task<bool> EhUsuarioExistente(Guid id);
}