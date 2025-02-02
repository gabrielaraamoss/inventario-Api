using System;
using System.Collections.Generic;

namespace Inventario.Infraestructure.Models;

public partial class Lote
{
    public Guid IdLote { get; set; }

    public string? NumeroLote { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
