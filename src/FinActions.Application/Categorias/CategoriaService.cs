using FinActions.Contracts.Categorias;
using FinActions.Contracts.Response;
using FinActions.Domain.Categorias;

namespace FinActions.Application.Categorias;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }


    public async Task<CategoriaDTO> Atualizar(Guid id, UpdateCategoriaDto updateCategoria)
    {
        var categoriaToUpdate = await _categoriaRepository.ObterPorId(id);
        categoriaToUpdate.Nome = updateCategoria.Nome;
        categoriaToUpdate.Cor = updateCategoria.Cor;
        categoriaToUpdate.EditedDate = DateTime.UtcNow;
        var updatedCategoria = await _categoriaRepository.Atualizar(categoriaToUpdate);

        return new CategoriaDTO(
            updatedCategoria.Id,
            updatedCategoria.Nome,
            updatedCategoria.Cor,
            updatedCategoria.CreationDate
        );
    }

    public async Task<bool> EhCategoriaExistente(Guid id)
    {
        return (await _categoriaRepository.ObterQueryable()).Any(x => x.Id == id);
    }

    public async Task<CategoriaDTO> Excluir(Guid id)
    {
        var categoriaToUpdate = await _categoriaRepository.ObterPorId(id);

        categoriaToUpdate.DeletedDate = DateTime.UtcNow;
        categoriaToUpdate.IsDeleted = true;

        var updatedCategoria = await _categoriaRepository.Atualizar(categoriaToUpdate);

        return new CategoriaDTO(
            updatedCategoria.Id,
            updatedCategoria.Nome,
            updatedCategoria.Cor,
            updatedCategoria.CreationDate
        );
    }

    public async Task<CategoriaDTO> Inserir(InsertCategoriaDto insertCategoria)
    {
        var newCategoria = await _categoriaRepository.Inserir(new Categoria()
        {
            Nome = insertCategoria.Nome,
            Cor = insertCategoria.Cor,
            CreationDate = DateTime.UtcNow
        });

        return new CategoriaDTO(
                newCategoria.Id,
                newCategoria.Nome,
                newCategoria.Cor,
                newCategoria.CreationDate
            );
    }

    public async Task<ResultadoPaginadoDto<CategoriaDTO>> ObterPaginado(GetCategoriaDto parametros)
    {
        var filtros = (Categoria x) => true;

        if (!string.IsNullOrEmpty(parametros.Nome))
            filtros = x => x.Nome.Contains(parametros.Nome);
        if (!string.IsNullOrEmpty(parametros.Cor))
            filtros = x => filtros(x) && x.Cor.Contains(parametros.Cor);

        var categorias = (await _categoriaRepository
                                .ObterPaginadoComFiltros(parametros.SkipCount,
                                                        parametros.TakeCount,
                                                        filtros)).ToList();

        var dtos = new List<CategoriaDTO>();
        categorias.ForEach(x =>
        {
            dtos.Add(new CategoriaDTO(x.Id,
                                    x.Nome,
                                    x.Cor,
                                    x.CreationDate));
        });

        return new ResultadoPaginadoDto<CategoriaDTO>()
        {
            SkipCount = parametros.SkipCount,
            TakeCount = parametros.TakeCount,
            TotalCount = categorias.Count,
            Items = dtos
        };
    }

    public async Task<CategoriaDTO> ObterPorId(Guid Id)
    {
        var categoria = await _categoriaRepository.ObterPorId(Id);
        return new CategoriaDTO(
            categoria.Id,
            categoria.Nome,
            categoria.Cor,
            categoria.CreationDate
        );
    }
}
