using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        private readonly ICategoriaRepository _categoriaRep;
        public ProdutoRepository(ICategoriaRepository categoriaRep , ApplicationContext contexto) : base(contexto)
        {
            _categoriaRep = categoriaRep;
        }

        public IList<Produto> GetProdutos()
        {
            return dbSet.Include(t => t.Categoria).ToList();
        }

        public async Task SaveProdutos(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                if (!dbSet.Where(p => p.Codigo == livro.Codigo).Any())
                {
                    var categoria = _categoriaRep.GetCategorias().Where(t => t.Nome.Equals(livro.Categoria)).FirstOrDefault();
                    if (categoria == null)
                    {
                        var cat = new List<Categoria>();
                        cat.Add(new Categoria { Nome = livro.Categoria });
                        await _categoriaRep.SaveCategorias(cat);
                        categoria = _categoriaRep.GetCategorias().Where(t => t.Nome.Equals(livro.Categoria)).FirstOrDefault();
                    }

                    dbSet.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco, categoria));
                }
            }
            await contexto.SaveChangesAsync();
        }
    }

    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public decimal Preco { get; set; }
    }
}
