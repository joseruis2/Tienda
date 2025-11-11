using System.ComponentModel.DataAnnotations;

namespace Tienda.ViewModels
{
    public class ProductosViewModels
    {
        public int ProductoId { get; set; }

        public string Codigo { get; set; } = null!;

        public string? CodigoBarras { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public int CategoriaId { get; set; }

        public decimal Precio { get; set; }

        public int StockActual { get; set; }

        public string? UnidadMedida { get; set; }

        public string? ImagenPrincipal { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public bool Estado { get; set; } = true;

        public string? CategoriaNombre { get; set; } // 👈 Para mostrar el nombre en el listado

        public IFormFile? ImagenArchivo { get; set; } // 👈 para subir imagen desde formulario

        

        


    }
}
