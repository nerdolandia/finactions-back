using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinActions.Domain.Base;

namespace FinActions.Domain.Categorias
{
    public class Categoria : BaseEntity
    {
        public string Nome { get; set; } = null!;
        public string Cor { get; set; } = "#FFF";
    }
}