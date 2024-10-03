using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IPagedList<Produto>> GetProdutosPorCategoriaAsync(int id, ProdutosParameters produtosParamiters);
        Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParamiters);
        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPeco);
    }
}
