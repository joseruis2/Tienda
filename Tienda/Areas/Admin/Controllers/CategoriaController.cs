using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda.Areas.Admin.Services;
using Tienda.Areas.Admin.ViewModels;
using Tienda.Models;

namespace Tienda.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // ✅ LISTAR (Index)
        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaService.ObtenerCategorias();

            var viewModels = categorias.Select(c => new CategoriaViewModel
            {
                CategoriaId = c.CategoriaId,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Imagen = c.Imagen,
                Estado = c.Estado,
                FechaCreacion = c.FechaCreacion
            }).ToList();

            return View(viewModels);
        }

        // ✅ DETALLES
        public async Task<IActionResult> Detalles(int id)
        {
            var categoria = await _categoriaService.ObtenerCategoriaPorId(id);
            if (categoria == null)
                return NotFound();

            var model = new CategoriaViewModel
            {
                CategoriaId = categoria.CategoriaId,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Imagen = categoria.Imagen,
                Estado = categoria.Estado,
                FechaCreacion = categoria.FechaCreacion
            };

            return View(model);
        }

        // ✅ CREAR (GET)
        public IActionResult Crear()
        {
            return View();
        }

        // ✅ CREAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CategoriaViewModel model, IFormFile? imagenArchivo)
        {
            // 1️⃣ Validación inicial del modelo
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // 2️⃣ Procesamiento de imagen (si existe)
                string? nombreArchivo = null;

                if (imagenArchivo is { Length: > 0 })
                {
                    string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categorias");

                    // Crear carpeta si no existe
                    Directory.CreateDirectory(carpeta);

                    // Nombre único con GUID
                    nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(imagenArchivo.FileName)}";
                    string ruta = Path.Combine(carpeta, nombreArchivo);

                    // Guardar la imagen en disco
                    await using var stream = new FileStream(ruta, FileMode.Create);
                    await imagenArchivo.CopyToAsync(stream);
                }

                // 3️⃣ Validar manualmente longitud antes de insertar (evita DbUpdateException)
                if (!string.IsNullOrEmpty(model.Descripcion) && model.Descripcion.Length > 256)
                {
                    ModelState.AddModelError(nameof(model.Descripcion), "La descripción no puede tener más de 256 caracteres.");
                    return View(model);
                }

                // 4️⃣ Crear entidad
                var categoria = new Categoria
                {
                    Nombre = model.Nombre.Trim(),
                    Descripcion = model.Descripcion?.Trim(),
                    Imagen = nombreArchivo,
                    Estado = model.Estado,
                    FechaCreacion = DateTime.Now
                };

                // 5️⃣ Guardar en base de datos
                await _categoriaService.CrearCategoria(categoria);

                // 6️⃣ Redirigir con mensaje de éxito (si usas TempData)
                TempData["MensajeExito"] = "La categoría se creó correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // 7️⃣ Detectar errores específicos de MySQL (como "Data too long")
                if (ex.InnerException?.Message.Contains("Data too long") == true)
                {
                    ModelState.AddModelError(nameof(model.Descripcion), "El texto es demasiado largo para la base de datos.");
                }
                else
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar la categoría. Inténtelo nuevamente.");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                // 8️⃣ Captura genérica
                ModelState.AddModelError("", "Error inesperado: " + ex.Message);
                return View(model);
            }
        }




        // ✅ EDITAR (GET)
        public async Task<IActionResult> Editar(int id)
        {
            var categoria = await _categoriaService.ObtenerCategoriaPorId(id);
            if (categoria == null)
                return NotFound();

            var model = new CategoriaViewModel
            {
                CategoriaId = categoria.CategoriaId,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Imagen = categoria.Imagen,
                Estado = categoria.Estado,
                FechaCreacion = categoria.FechaCreacion
            };

            return View(model);
        }

        // ✅ EDITAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(CategoriaViewModel model, IFormFile? imagenArchivo)
        {
            if (!ModelState.IsValid)
                return View(model);

            var categoriaExistente = await _categoriaService.ObtenerCategoriaPorId(model.CategoriaId);
            if (categoriaExistente == null)
                return NotFound();

            // Si se sube una nueva imagen
            if (imagenArchivo != null && imagenArchivo.Length > 0)
            {
                string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categorias");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string nuevoNombre = Guid.NewGuid().ToString() + Path.GetExtension(imagenArchivo.FileName);
                string ruta = Path.Combine(carpeta, nuevoNombre);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await imagenArchivo.CopyToAsync(stream);
                }

                categoriaExistente.Imagen = nuevoNombre;
            }

            // Actualizar otros campos
            categoriaExistente.Nombre = model.Nombre;
            categoriaExistente.Descripcion = model.Descripcion;
            categoriaExistente.Estado = model.Estado;
            categoriaExistente.FechaCreacion = DateTime.Now;

            await _categoriaService.ActualizarCategoria(categoriaExistente);
            return RedirectToAction(nameof(Index));
        }


        // ✅ ELIMINAR (GET)
        public async Task<IActionResult> Eliminar(int id)
        {
            var categoria = await _categoriaService.ObtenerCategoriaPorId(id);
            if (categoria == null)
                return NotFound();

            var model = new CategoriaViewModel
            {
                CategoriaId = categoria.CategoriaId,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Imagen = categoria.Imagen,
                Estado = categoria.Estado,
                FechaCreacion = categoria.FechaCreacion
            };

            return View(model);
        }

        // ✅ ELIMINAR (POST)
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await _categoriaService.EliminarCategoria(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
