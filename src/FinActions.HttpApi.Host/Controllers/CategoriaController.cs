using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinActions.Domain.Categorias;
using Microsoft.AspNetCore.Mvc;
using FinActions.Contracts.Categorias;
using FinActions.Application.Categorias;
using FinActions.Contracts.Response;

namespace FinActions.HttpApi.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }
        
        [HttpGet("")]
        public async Task<ResultadoPaginadoDto<CategoriaDTO>> GetAsync(
            [FromQuery] GetCategoriaDto parametros
        )
        {
            return await _categoriaService.ObterPaginado(parametros);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            try{
                return Ok(await _categoriaService.ObterPorId(id));
            }
            catch(Exception ex){
                return NotFound("Categoria não existe");
            }
        }
        [HttpPost("")]
        public async Task<IActionResult> PostAsync(
            [FromBody] InsertCategoriaDto insertCategoria
        )
        {
            try{
                return Ok(await _categoriaService.Inserir(insertCategoria));
            }
            catch(Exception ex){
                return BadRequest("Categoria já existe");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] Guid id,
            [FromBody] UpdateCategoriaDto updateCategoria
        )
        {
            try{
                return Ok(await _categoriaService.Atualizar(id, updateCategoria));
            }
            catch(Exception ex){
                return NotFound("Categoria não existe");
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            try{
                return Ok(await _categoriaService.Excluir(id));
            }
            catch(Exception ex){
                return NotFound("Categoria não existe");
            }
        }

    }
}