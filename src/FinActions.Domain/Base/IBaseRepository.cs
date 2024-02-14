using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinActions.Domain.Base
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IQueryable<T>> ObterQueryable();
        Task<T> Inserir(T entity);
        Task<T> Atualizar(T entity);
        Task<T> Excluir(T entity);
        Task<T> ObterPorId(Guid id);
        Task<IEnumerable<T>> ObterPaginadoComFiltros(int take, int skip, Func<T, bool> filtros);
        Task<int> Contar();
    }
}