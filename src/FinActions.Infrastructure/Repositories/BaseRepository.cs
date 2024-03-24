using System.Linq.Expressions;
using FinActions.Domain.Base;
using FinActions.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

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

    public async Task<T> ObterPorId(Guid id)
    {
        var dbEntity = (await ObterQueryComIncludes()).First(x => x.Id == id);
        return dbEntity;
    }

    public async Task<IEnumerable<T>> ObterPaginadoComFiltros(int skip, int take, Func<T, bool> filtros)
    {
        var query = await ObterQueryComIncludesEOrdem();

        var entities = query.Where(filtros)
                            .Skip(skip)
                            .Take(take);
        return entities;
    }

    public Task<T> Atualizar(T entity)
    {
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

    public Task<T> Excluir(T entity)
    {
        entity.IsDeleted = true;
        entity.DeletedDate = DateTime.UtcNow;

        var dbEntity = _context.Update(entity).Entity;
        _context.SaveChanges();
        return Task.FromResult(dbEntity);
    }

    public Task<T> Inserir(T entity)
    {
        var dbEntity = DbSet.Add(entity).Entity;
        SaveChanges();

        return Task.FromResult(dbEntity);
    }

    protected async Task<IOrderedQueryable<T>> ObterQueryComIncludesEOrdem()
    {
        var query = await ObterQueryComIncludes();

        if (ObterOrdem() is not null && ObterOrdem().Any())
        {
            var orderedQuery = query.OrderBy(ObterOrdem().First());

            foreach (var campoParaOrdenar in ObterOrdem().Where(x => x != ObterOrdem().First()))
                orderedQuery = orderedQuery.OrderBy(campoParaOrdenar);

            return orderedQuery;
        }

        return query.OrderByDescending(x => x.CreationDate);
    }

    protected async Task<IQueryable<T>> ObterQueryComIncludes()
    {
        var query = await ObterQueryable();

        if (ObterIncludes() is not null && ObterIncludes().Any())
        {
            foreach (var tabelaFilha in ObterIncludes())
                query = query.Include(tabelaFilha);
        }

        return query;
    }

    /// <summary>
    /// Retorna uma coleção de expressões que irão ser aplicadas para trazer as entidades filhas.
    /// As expressões devem retornar o nome da entidade filha que devem ser populada.
    /// </summary>
    protected abstract IEnumerable<Expression<Func<T, object>>> ObterIncludes();

    /// <summary>
    /// Retorna uma coleção de expressões que irão ser aplicadas para ordenar as queries.
    /// As expressões devem retornar o nome da entidade filha que devem ser populada.
    /// Se a lista estiver vazia ou nula, as queries serão ordenadas pela data de criação da entidade
    /// de maneira descendente
    /// </summary>
    protected abstract IEnumerable<Expression<Func<T, object>>> ObterOrdem();

    protected int SaveChanges() => _context.SaveChanges();

    protected async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public Task<IQueryable<T>> ObterQueryable() => Task.FromResult(DbSet.AsQueryable());
}