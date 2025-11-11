namespace Tienda.ViewModels
{
    public class CategoriaViewModel
    {
        public int CategoriaId { get; set; }

        public string Nombre { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public string? Imagen { get; set; }

        public bool? Estado { get; set; }

        public DateTime? FechaCreacion { get; set; }

        // Campo adicional solo para la vista
        public string EstadoTexto => Estado == true ? "Activo" : "Inactivo";
    }
}
