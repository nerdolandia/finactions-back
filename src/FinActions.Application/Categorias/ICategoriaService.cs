using FinActions.Contracts.Categorias;
using FinActions.Contracts.Response;


namespace FinActions.Application.Categorias;
public interface ICategoriaService
{
    Task<CategoriaDTO> ObterPorId(Guid Id);    
    Task<ResultadoPaginadoDto<CategoriaDTO>> ObterPaginado(GetCategoriaDto parametros);
    Task<CategoriaDTO> Atualizar(Guid id, UpdateCategoriaDto updateCategoria);
    Task<CategoriaDTO> Excluir(Guid id);
    Task<CategoriaDTO> Inserir(InsertCategoriaDto insertCategoria);
    Task<bool> EhCategoriaExistente(Guid id);
}
