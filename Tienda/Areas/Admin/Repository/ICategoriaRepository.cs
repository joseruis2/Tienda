using Tienda.Models;

namespace Tienda.Areas.Admin.Repository
{
    public interface ICategoriaRepository
    {
        Task<List<Categoria>> GetAllAsync();
        Task<Categoria?> GetByIdAsync(int id);
        Task AddAsync(Categoria rol);
        Task UpdateAsync(Categoria rol);
        Task DeleteAsync(int id);
    }
}
