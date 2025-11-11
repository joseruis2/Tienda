using Tienda.Models;

namespace Tienda.Repositories
{
    public interface IProductoRepository
    {
        Task<List<Producto>> GetAllAsync();
        Task<Producto> GetByIdAsync(int id);

        Task AddAsync(Producto producto);

        Task UpdateAsync(Producto producto);

        Task DeleteAsync(int id);
    }
}
