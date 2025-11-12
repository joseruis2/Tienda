using Microsoft.AspNetCore.Mvc;

namespace Tienda.Areas.Public.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
