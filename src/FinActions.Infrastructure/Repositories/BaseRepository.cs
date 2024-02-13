using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinActions.Domain.Base;
using FinActions.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FinActions.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly FinActionsContext _context;

        public BaseRepository(FinActionsContext context)
        {
            _context = context;
        }

        public Task<T> ObterPorId(Guid id)
        {
            var dbSet = _context.Set<T>();
            var dbEntity = dbSet.First(x => x.Id == id);
            return Task.FromResult(dbEntity);
        }
        public Task<IEnumerable<T>> ObterPaginado(int skip, int take)
        {
            var dbSet = _context.Set<T>().AsNoTracking();
            var query = dbSet.AsQueryable();

            query = query.Skip(skip).Take(take);
            var dbEntity = query.AsEnumerable();
            return Task.FromResult(dbEntity);
        }
        public Task<T> Atualizar(T entity, Guid idUsuario)
        {
            entity.EditedBy = idUsuario;
            entity.EditedDate = DateTime.UtcNow;

            var dbSet = _context.Set<T>();
            var dbEntity = dbSet.Update(entity).Entity;
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

            var dbSet = _context.Set<T>();
            var dbEntity = dbSet.Update(entity).Entity;
            _context.SaveChanges();
            return Task.FromResult(dbEntity);
        }

        public Task<T> Inserir(T entity, Guid idUsuario)
        {
            entity.CreatedBy = idUsuario;
            entity.CreationDate = DateTime.UtcNow;

            var dbSet = _context.Set<T>();
            var dbEntity = dbSet.Add(entity).Entity;
            _context.SaveChanges();
            return Task.FromResult(dbEntity);
        }
    }
}