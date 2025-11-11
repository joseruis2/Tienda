using Tienda.Models;

namespace Tienda.Repositories
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
