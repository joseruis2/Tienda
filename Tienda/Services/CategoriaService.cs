using Tienda.Models;
using Tienda.Repositories;

namespace Tienda.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Categoria>> ObtenerCategorias()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Categoria?> ObtenerCategoriaPorId(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now; // 👈 agrega esta línea
            await _repository.AddAsync(categoria);
        }

        public async Task ActualizarCategoria(Categoria categoria)
        {
            var categoriaExistente = await _repository.GetByIdAsync(categoria.CategoriaId);

            if (categoriaExistente == null)
                throw new Exception("La categoría no existe");

            // ✅ Actualizar campos
            categoriaExistente.Nombre = categoria.Nombre;
            categoriaExistente.Descripcion = categoria.Descripcion;
            categoriaExistente.Imagen = categoria.Imagen;
            categoriaExistente.Estado = categoria.Estado;

            // ✅ Actualizar la fecha de creación con la fecha actual
            categoriaExistente.FechaCreacion = DateTime.Now;

            await _repository.UpdateAsync(categoriaExistente);
        }



        public async Task EliminarCategoria(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
