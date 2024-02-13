using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinActions.Domain.Categorias;
using FinActions.Infrastructure.Context;

namespace FinActions.Infrastructure.Repositories.Categorias
{
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(FinActionsContext context) : base(context)
        {
        }
    }
}