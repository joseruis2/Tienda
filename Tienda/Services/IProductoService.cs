using Tienda.Models;

namespace Tienda.Services
{
    public interface IProductoService
    {
        //ObtenerCategorias
        Task<List<Producto>> ObtenerProducto();

        //ObtenerCategoriaPorId
        Task<Producto> ObtenerProductoPorId(int id);

        Task CrearProducto(Producto producto);

        Task ActualizarProducto(Producto producto);

        Task EliminarProducto(int id);
    }
}
