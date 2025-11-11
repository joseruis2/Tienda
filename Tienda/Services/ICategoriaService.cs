using Tienda.Models;

namespace Tienda.Services
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> ObtenerCategorias();
        Task<Categoria?> ObtenerCategoriaPorId(int id);
        Task CrearCategoria(Categoria categoria);
        Task ActualizarCategoria(Categoria categoria);
        Task EliminarCategoria(int id);
    }
}
