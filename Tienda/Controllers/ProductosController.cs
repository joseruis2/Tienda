using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda.Models;
using Tienda.Services;
using Tienda.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tienda.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly TiendadbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductosController(IProductoService productoService, TiendadbContext context, IWebHostEnvironment env)
        {
            _productoService = productoService;
            _context = context;
            _env = env;
        }

        // ✅ LISTAR
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Select(p => new ProductosViewModels
                {
                    ProductoId = p.ProductoId,
                    Codigo = p.Codigo,
                    Nombre = p.Nombre,
                    CategoriaNombre = p.Categoria.Nombre,
                    Precio = p.Precio,
                    StockActual = p.StockActual,
                    ImagenPrincipal = p.ImagenPrincipal,
                    Estado = p.Estado ?? true
                })
                .ToListAsync();

            return View(productos);
        }

        // ✅ CREAR GET
        public IActionResult Crear()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre");
            return View();
        }

        // ✅ CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ProductosViewModels vm)
        {
            if (ModelState.IsValid)
            {
                string? nombreImagen = null;

                if (vm.ImagenArchivo != null)
                {
                    string carpeta = Path.Combine(_env.WebRootPath, "images/productos");
                    Directory.CreateDirectory(carpeta);

                    nombreImagen = Guid.NewGuid() + Path.GetExtension(vm.ImagenArchivo.FileName);
                    string ruta = Path.Combine(carpeta, nombreImagen);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                        await vm.ImagenArchivo.CopyToAsync(stream);
                }

                var producto = new Producto
                {
                    Codigo = vm.Codigo,
                    CodigoBarras = vm.CodigoBarras,
                    Nombre = vm.Nombre,
                    Descripcion = vm.Descripcion,
                    CategoriaId = vm.CategoriaId,
                    Precio = vm.Precio,
                    StockActual = vm.StockActual,
                    UnidadMedida = vm.UnidadMedida,
                    ImagenPrincipal = nombreImagen,
                    Estado = vm.Estado,
                    FechaCreacion = DateTime.Now
                };

                await _productoService.CrearProducto(producto);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", vm.CategoriaId);
            return View(vm);
        }

        //Gets
        public async Task<IActionResult> Editar(int id)
        {
            var producto = await _productoService.ObtenerProductoPorId(id);

            if (producto == null)
            
                return NotFound();
            

            var model = new ProductosViewModels
            {
                ProductoId = producto.ProductoId,
                Codigo = producto.Codigo,
                CodigoBarras = producto.CodigoBarras,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                CategoriaId = producto.CategoriaId,
                Precio = producto.Precio,
                StockActual = producto.StockActual,
                UnidadMedida = producto.UnidadMedida,
                ImagenPrincipal = producto.ImagenPrincipal,
                Estado = producto.Estado ?? true,
                FechaCreacion = producto.FechaCreacion

            }
            ;
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(model);

        }

        // ✅ EDITAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, ProductosViewModels vm)
        {
            if (id != vm.ProductoId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var producto = await _productoService.ObtenerProductoPorId(id);
                if (producto == null)
                    return NotFound();

                string? nombreImagen = producto.ImagenPrincipal;

                if (vm.ImagenArchivo != null)
                {
                    string carpeta = Path.Combine(_env.WebRootPath, "images/productos");
                    Directory.CreateDirectory(carpeta);

                    nombreImagen = Guid.NewGuid() + Path.GetExtension(vm.ImagenArchivo.FileName);
                    string ruta = Path.Combine(carpeta, nombreImagen);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                        await vm.ImagenArchivo.CopyToAsync(stream);
                }

                producto.Codigo = vm.Codigo;
                producto.CodigoBarras = vm.CodigoBarras;
                producto.Nombre = vm.Nombre;
                producto.Descripcion = vm.Descripcion;
                producto.CategoriaId = vm.CategoriaId;
                producto.Precio = vm.Precio;
                producto.StockActual = vm.StockActual;
                producto.UnidadMedida = vm.UnidadMedida;
                producto.ImagenPrincipal = nombreImagen;
                producto.Estado = vm.Estado;

                await _productoService.ActualizarProducto(producto);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", vm.CategoriaId);
            return View(vm);
        }

        // ✅ DETALLES
        public async Task<IActionResult> Detalles(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.ProductoId == id);

            if (producto == null)
                return NotFound();

            var vm = new ProductosViewModels
            {
                ProductoId = producto.ProductoId,
                Codigo = producto.Codigo,
                CodigoBarras = producto.CodigoBarras,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                CategoriaNombre = producto.Categoria.Nombre,
                Precio = producto.Precio,
                StockActual = producto.StockActual,
                UnidadMedida = producto.UnidadMedida,
                ImagenPrincipal = producto.ImagenPrincipal,
                Estado = producto.Estado ?? true,
                FechaCreacion = producto.FechaCreacion
            };

            return View(vm);
        }

        // ✅ ELIMINAR (GET)
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.ProductoId == id);

            if (producto == null)
                return NotFound();

            var vm = new ProductosViewModels
            {
                ProductoId = producto.ProductoId,
                Codigo = producto.Codigo,
                Nombre = producto.Nombre,
                CategoriaNombre = producto.Categoria.Nombre,
                Precio = producto.Precio,
                ImagenPrincipal = producto.ImagenPrincipal
            };

            return View(vm);
        }

        // ✅ ELIMINAR (POST)
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await _productoService.EliminarProducto(id); // ⚠️ Renómbralo luego a EliminarProducto
            return RedirectToAction(nameof(Index));
        }
    }
}
