using System;
using System.Collections.Generic;

namespace Inventario.Infraestructure.Models;

public partial class Usuario
{
    public Guid IdUsuario { get; set; }

    public string? Nombres { get; set; }

    public string? Usuario1 { get; set; }

    public string? Password { get; set; }
}
