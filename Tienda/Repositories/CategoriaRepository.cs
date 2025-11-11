using Microsoft.EntityFrameworkCore;
using Tienda.Models;

namespace Tienda.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly TiendadbContext _context;

        public CategoriaRepository(TiendadbContext context)
        {
            _context = context;
        }

        public async Task<List<Categoria>> GetAllAsync()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        public async Task AddAsync(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Categoria rol)
        {
            _context.Categorias.Update(rol);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rol = await _context.Categorias.FindAsync(id);
            if (rol != null)
            {
                _context.Categorias.Remove(rol);
                await _context.SaveChangesAsync();
            }
        }
    }
}
