using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda.Models;

namespace Tienda.Areas.Public.Controllers
{
    [Area("Public")]
    public class HomeController : Controller
    {
        private readonly TiendadbContext _context;

        public HomeController(TiendadbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Estado == true)
                .OrderByDescending(p => p.FechaCreacion)
                .Take(9)               // Mostrar los últimos 9 productos
                .Select(p => new
                {
                    p.ProductoId,
                    p.Nombre,
                    p.Precio,
                    p.ImagenPrincipal,
                    Categoria = p.Categoria.Nombre
                })
                .ToListAsync();

            ViewBag.Productos = productos;
            return View();
        }
    }
}
