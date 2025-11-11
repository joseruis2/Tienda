using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tienda.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string Codigo { get; set; } = null!;

    public string? CodigoBarras { get; set; }

    public string Nombre { get; set; } = null!;

    [MaxLength(1000, ErrorMessage = "La descripción no puede superar los 1000 caracteres.")]
    public string? Descripcion { get; set; }

    public int CategoriaId { get; set; }

    public decimal Precio { get; set; }

    public int StockActual { get; set; }

    public string? UnidadMedida { get; set; }

    public string? ImagenPrincipal { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool? Estado { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;
}
