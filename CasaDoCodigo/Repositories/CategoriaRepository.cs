using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public IList<Categoria> GetCategorias()
        {
            return dbSet.ToList();
        }

        public async Task SaveCategorias(List<Categoria> categorias)
        {
            foreach (var categoria in categorias)
            {
                if (!dbSet.Where(p => p.Id == categoria.Id).Any())
                {
                    dbSet.Add(new Categoria(categoria.Nome));
                }
            }
            await contexto.SaveChangesAsync();
        }
    }
}
