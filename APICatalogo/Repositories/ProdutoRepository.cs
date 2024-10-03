using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository: Repository<Produto>, IProdutoRepository
    {
        
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
        {
            var produtos = await GetAllAsync();

            if (produtosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
            {
                if (produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
            }
            var produtosFiltrados = produtos.ToPagedList(produtosFiltroParams.PageNumber, produtosFiltroParams.PageSize);
            return produtosFiltrados;
        }

        public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParamiters)
        {
            var produtos = await GetAllAsync();

            var produtosOrdenados = produtos.OrderBy(p => p.Nome).AsQueryable();

            return produtosOrdenados.ToPagedList(
                produtosParamiters.PageNumber, produtosParamiters.PageSize); 
        }

        public async Task<IPagedList<Produto>> GetProdutosPorCategoriaAsync(int id, ProdutosParameters produtosParamiters)
        {
            var produtos = await GetAllAsync();

            var produtosCategoria = produtos.Where(c => c.CategoriaId == id).AsQueryable();

            return produtosCategoria.ToPagedList(
                produtosParamiters.PageNumber, produtosParamiters.PageSize);
        }

    }
}
