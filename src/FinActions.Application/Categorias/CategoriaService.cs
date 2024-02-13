using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinActions.Contracts.Categorias;
using FinActions.Contracts.Response;
using FinActions.Contracts.Usuario;
using FinActions.Domain.Categorias;
using FinActions.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FinActions.Application.Categorias;

public class CategoriaService : ICategoriaService
{
    private readonly FinActionsContext _context;
    private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(FinActionsContext context, ICategoriaRepository categoriaRepository)
    {
        _context = context;
        _categoriaRepository = categoriaRepository;
    }


    public Task<CategoriaDTO> Atualizar(Guid id, UpdateCategoriaDto updateCategoria)
    {
        var categoriaToUpdate = _context.Categorias.First(x => x.Id == id);
        categoriaToUpdate.Nome = updateCategoria.Nome;
        categoriaToUpdate.Cor = updateCategoria.Cor;
        categoriaToUpdate.EditedDate = DateTime.UtcNow;
        var updatedCategoria = _context.Categorias.Update(categoriaToUpdate).Entity;

        _context.SaveChanges();
        return Task.FromResult(new CategoriaDTO(
            updatedCategoria.Id,
            updatedCategoria.Nome,
            updatedCategoria.Cor,
            updatedCategoria.CreationDate
        ));
    }

    public Task<bool> EhCategoriaExistente(Guid id)
    {
        return Task.FromResult(
            _context.Categorias.Any(x => x.Id == id)
        );
    }

    public Task<CategoriaDTO> Excluir(Guid id)
    {
        var categoriaToUpdate = _context.Categorias
                            .First(x => x.Id == id);
                        
        categoriaToUpdate.DeletedDate = DateTime.UtcNow;
        categoriaToUpdate.IsDeleted = true;

        var updatedCategoria = _context.Categorias.Update(categoriaToUpdate).Entity;

        _context.SaveChanges();

        return Task.FromResult(new CategoriaDTO(
            updatedCategoria.Id,
            updatedCategoria.Nome,
            updatedCategoria.Cor,
            updatedCategoria.CreationDate
        ));
    }

    public Task<CategoriaDTO> Inserir(InsertCategoriaDto insertCategoria)
    {
        var newCategoria = _context.Categorias.Add(new Domain.Categorias.Categoria()
        {
            Id = new Guid(),
            Nome = insertCategoria.Nome,
            Cor = insertCategoria.Cor,
            CreationDate = DateTime.UtcNow
        }).Entity;

        _context.SaveChanges();

        return Task.FromResult(
            new CategoriaDTO(
                newCategoria.Id,
                newCategoria.Nome,
                newCategoria.Cor,
                newCategoria.CreationDate
            )
        );
    }

    public async Task<ResultadoPaginadoDto<CategoriaDTO>> ObterPaginado(GetCategoriaDto parametros)
    {
        var query = _context.Categorias.AsQueryable();

        if (!string.IsNullOrEmpty(parametros.Nome))
            query = query.Where(x => x.Nome.Contains(parametros.Nome));
        if (!string.IsNullOrEmpty(parametros.Cor))
            query = query.Where(x => x.Cor.Contains(parametros.Cor));

        query = query.Skip(parametros.SkipCount)
                    .Take(parametros.TakeCount);

        var categorias = await query.ToListAsync();
        var dtos = new List<CategoriaDTO>();
        categorias.ForEach(x => 
        {
            dtos.Add(new CategoriaDTO(x.Id,
                                    x.Nome,
                                    x.Cor,
                                    x.CreationDate));
        });

        return new ResultadoPaginadoDto<CategoriaDTO>(){
            SkipCount = parametros.SkipCount,
            TakeCount = parametros.TakeCount,
            TotalCount = categorias.Count,
            Items = dtos
        };
    }

    public Task<CategoriaDTO> ObterPorId(Guid Id)
    {
        var categoria = _context.Categorias.First(x =>  x.Id == Id);
        return Task.FromResult(new CategoriaDTO(
            categoria.Id,
            categoria.Nome,
            categoria.Cor,
            categoria.CreationDate
        ));
    }
}
