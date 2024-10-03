using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriasController> _logger;
        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriasParameters);

            if(!categorias.Any())
            {
                return NotFound("Categorias não encontradas...");
            }

            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            }; 

            Response.Headers.Append("x-pagination", JsonConvert.SerializeObject(metadata));
            
            var categoriasDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }

            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoria = categoriaDTO.ToCategoria();

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria!);
            await _uof.CommitAsync();

            var novaCategoriaDTO = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new {id = novaCategoriaDTO?.CategoriaId, categoria = novaCategoriaDTO});
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoria = categoriaDTO.ToCategoria();

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria!);
            await _uof.CommitAsync();

            var categoriaDtoAtualizada = categoria?.ToCategoriaDTO();

            return Ok(categoriaDtoAtualizada);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }

            var categoriaDeletada = _uof.CategoriaRepository.Delete(categoria);
            await _uof.CommitAsync();

            var categoriaDeletadaDTO = categoriaDeletada.ToCategoriaDTO();

            return Ok(categoriaDeletadaDTO);
        }

    }
}
