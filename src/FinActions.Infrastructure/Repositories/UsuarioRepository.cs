using FinActions.Domain.Usuarios;
using FinActions.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FinActions.Infrastructure.Repositories;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(FinActionsContext context) : base(context)
    {
    }

    public Task<Usuario> ObterPorEmail(string email)
    {
        return Task.FromResult(ObterQueryComDetalhes()
                                .First(x => x.Email == email));
    }

    protected override IQueryable<Usuario> ObterQueryComDetalhes()
        => DbSet.Include(x => x.Papeis);
}