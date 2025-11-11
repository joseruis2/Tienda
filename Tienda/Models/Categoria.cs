using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tienda.Models;

public partial class Categoria
{
    public int CategoriaId { get; set; }

    public string Nombre { get; set; } = null!;

    [MaxLength(256, ErrorMessage = "La descripción no puede superar los 256 caracteres.")]
    public string Descripcion { get; set; } = null!;

    public string? Imagen { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
