using System.Linq.Expressions;
using FinActions.Domain.Usuarios;
using FinActions.Infrastructure.Context;

namespace FinActions.Infrastructure.Repositories;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(FinActionsContext context) : base(context)
    {
    }

    public async Task<Usuario> ObterPorEmail(string email)
    {
        var query = await ObterQueryComIncludesEOrdem();

        return query.First(x => x.Email == email);
    }

    protected override IEnumerable<Expression<Func<Usuario, object>>> ObterIncludes() => [(Usuario x) => x.Papeis];

    protected override IEnumerable<Expression<Func<Usuario, object>>> ObterOrdem() => [(Usuario x) => x.Nome];

    public Task<bool> EhEmailExistente(string email) => Task.FromResult(DbSet.Any(x => x.Email == email));

    public Task<bool> EhUsuarioExistente(Guid id) => Task.FromResult(DbSet.Any(x => x.Id == id));
}