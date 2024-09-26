using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {

        private readonly IUnitOfWork _uof;
        private ILogger<ProdutosController> _logger;

        public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _uof.ProdutoRepository.GetAll();

            if(produtos is null)
            {
                return NotFound();
            }

            return Ok(produtos);
        }

        [HttpGet("{id:int}" , Name="ObterProduto")]
        public ActionResult<Produto> Get(int id) 
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if(produto is null)
            {
                return NotFound("Produto não encontrado...");
            }

            return Ok(produto);
        }

        [HttpGet("categorias/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if(produtos is null)
            {
                return NotFound();
            }

            return Ok(produtos);
        }

        [HttpPost]
        public ActionResult Post(Produto produto) 
        {
            if(produto is null)
            {
                return BadRequest();
            }

            var novoProduto = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();
            return new CreatedAtRouteResult("ObterProduto", new {id = novoProduto.ProdutoId}, produto);
        }


        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
            {
                _logger.LogWarning($"Produto com id= {id} não encontrada...");
                return NotFound($"Produto com id= {id} não encontrada...");
            }

            var categoriaDeletada = _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            return Ok(produto);
        }
    }
}
