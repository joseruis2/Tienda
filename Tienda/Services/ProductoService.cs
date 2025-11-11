using Tienda.Models;
using Tienda.Repositories;

namespace Tienda.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<List<Producto>> ObtenerProducto()
        {
            return await _productoRepository.GetAllAsync();
        }

        public async Task<Producto> ObtenerProductoPorId(int id)
        {
            return await _productoRepository.GetByIdAsync(id);
        }

        public async Task CrearProducto(Producto producto)
        {
            await _productoRepository.AddAsync(producto);
        }

        public async Task ActualizarProducto(Producto producto)
        {
            await _productoRepository.UpdateAsync(producto);
        }

        public async Task EliminarProducto(int id) // 👈 debería llamarse EliminarProducto
        {
            await _productoRepository.DeleteAsync(id);
        }
    }
}
