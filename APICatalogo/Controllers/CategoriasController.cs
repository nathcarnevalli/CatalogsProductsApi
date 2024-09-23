using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoriasController(AppDbContext context) {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _context.Categorias.AsNoTracking().ToList();

            if (categorias is null)
            {
                return BadRequest();
            }

            return Ok(categorias);
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetProdutosCategoria()
        {
            var categorias = _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToList();

            if(categorias is null)
            {
                return BadRequest();
            }

            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<IEnumerable<Categoria>> Get(int id)
        {
            var categorias = _context.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId == id);

            if (categorias is null)
            {
                return BadRequest();
            }

            return Ok(categorias);
        }



        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new {id = categoria.CategoriaId, categoria = categoria});
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if(categoria is null)
            {
                return NotFound("Categoria não foi localizada...");
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }

    }
}
