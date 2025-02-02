using System;
using System.Collections.Generic;

namespace Inventario.Infraestructure.Models;

public partial class Producto
{
    public Guid IdProducto { get; set; }

    public Guid? IdUsuario { get; set; }

    public string? NombreProducto { get; set; }

    public string? Codigo { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public Guid IdLote { get; set; }

    public int? Stock { get; set; }

    public DateOnly? FechaIngreso { get; set; }

    public DateOnly? FechaCaducidad { get; set; }

    public double? Precio { get; set; }

    public virtual Lote IdLoteNavigation { get; set; } = null!;
}
