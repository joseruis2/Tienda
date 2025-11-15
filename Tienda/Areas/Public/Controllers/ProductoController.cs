using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda.Models;

namespace Tienda.Areas.Public.Controllers
{
    [Area("Public")]
    public class ProductoController : Controller
    {
        private readonly TiendadbContext _context;

        public ProductoController(TiendadbContext context)
        {
            _context = context;
        }

        //Gets
        public async Task <IActionResult> Index()
        {
            // Cargar categorías activas
            var categorias = await _context.Categorias
                .Where(c => c.Estado == true)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            // Enviar al ViewBag para el sidebar
            ViewBag.Categorias = categorias;

            // La vista mostrará los productos dinámicamente con fetch()
            return View();
        }

        // 🔹 Endpoint JSON para el filtrado dinámico con fetch()
        [HttpGet]
        public async Task<IActionResult> Filtrar(int? categoriaId, string? buscar)
        {
            var query = _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Estado == true)
                .AsQueryable();

            if (categoriaId.HasValue)
                query = query.Where(p => p.CategoriaId == categoriaId.Value);

            if (!string.IsNullOrWhiteSpace(buscar))
                query = query.Where(p => p.Nombre.Contains(buscar) || p.Descripcion.Contains(buscar));

            var productos = await query
                .OrderBy(p => p.Nombre)
                .Select(p => new
                {
                    id = p.ProductoId,
                    nombre = p.Nombre,
                    categoria = p.Categoria.Nombre,
                    precio = p.Precio,
                    imagen = p.ImagenPrincipal ?? "no-image.png"
                })
                .ToListAsync();

            return Json(productos);
        }


        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var p = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.ProductoId == id);

            if (p == null)
                return NotFound();

            return Json(new
            {
                id = p.ProductoId,
                nombre = p.Nombre,
                descripcion = p.Descripcion,
                categoria = p.Categoria.Nombre,
                precio = p.Precio,
                codigo = p.Codigo,
                stock = p.StockActual,
                imagen = p.ImagenPrincipal ?? "no-image.png"
            });
        }
    }
}
