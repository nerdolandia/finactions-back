using System.Linq.Expressions;
using FinActions.Domain.Categorias;
using FinActions.Infrastructure.Context;

namespace FinActions.Infrastructure.Repositories.Categorias
{
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(FinActionsContext context) : base(context)
        {
        }

        protected override IEnumerable<Expression<Func<Categoria, object>>> ObterIncludes()
            => [];

        protected override IEnumerable<Expression<Func<Categoria, object>>> ObterOrdem()
            => [(Categoria x) => x.Nome];
    }
}