using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinActions.Domain.Base
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> Inserir(T entity, Guid idUsuario);
        Task<T> Atualizar(T entity, Guid idUsuario);
        Task<T> Excluir(T entity, Guid idUsuario);
        Task<T> ObterPorId(Guid id);
        Task<IEnumerable<T>> ObterPaginado(int take, int skip);
        Task<int> Contar();
    }
}