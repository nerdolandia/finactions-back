using FinActions.Domain.Base;
using FinActions.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FinActions.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly FinActionsContext _context;
    protected DbSet<T> DbSet { get; private set; }

    public BaseRepository(FinActionsContext context)
    {
        _context = context;
        DbSet = context.Set<T>();
    }

    public Task<T> ObterPorId(Guid id)
    {
        var dbEntity = ObterQueryComDetalhes().First(x => x.Id == id);
        return Task.FromResult(dbEntity);
    }

    public Task<IEnumerable<T>> ObterPaginadoComFiltros(int skip, int take, IQueryable<T> queryComFiltros)
    {
        var query = ObterQueryComDetalhes()
                    .Concat(queryComFiltros);

        query = query.Skip(skip).Take(take);
        var dbEntity = query.AsEnumerable();
        return Task.FromResult(dbEntity);
    }

    public Task<T> Atualizar(T entity, Guid idUsuario)
    {
        entity.EditedBy = idUsuario;
        entity.EditedDate = DateTime.UtcNow;

        var dbEntity = _context.Update(entity).Entity;
        _context.SaveChanges();
        return Task.FromResult(dbEntity);
    }

    public Task<int> Contar()
    {
        var dbSet = _context.Set<T>();
        var dbEntity = dbSet.Count();
        return Task.FromResult(dbEntity);
    }

    public Task<T> Excluir(T entity, Guid idUsuario)
    {
        entity.IsDeleted = true;
        entity.DeletedDate = DateTime.UtcNow;
        entity.DeletedBy = idUsuario;

        var dbEntity = _context.Update(entity).Entity;
        _context.SaveChanges();
        return Task.FromResult(dbEntity);
    }

    public Task<T> Inserir(T entity, Guid idUsuario)
    {
        entity.CreatedBy = idUsuario;
        entity.CreationDate = DateTime.UtcNow;

        var dbEntity = DbSet.Add(entity).Entity;
        SaveChanges();

        return Task.FromResult(dbEntity);
    }

    /// <summary>
    /// Retorna a query com os Includes utilizada
    /// </summary>
    protected abstract IQueryable<T> ObterQueryComDetalhes();

    protected int SaveChanges() => _context.SaveChanges();

    protected async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public Task<IQueryable<T>> GetQueryable() => Task.FromResult(DbSet.AsQueryable());
}