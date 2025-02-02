namespace Inventario.Domain.Dtos;
public class ProductoDto
{
    public Guid IdProducto { get; set; }
    public string NombreProducto { get; set; }
    public string Codigo { get; set; }
    public string Descripcion { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public Guid IdLote { get; set; }
    public string NumeroLote { get; set; }
    public int? Stock { get; set; }
    public string? FechaIngreso { get; set; }
    public string? FechaCaducidad { get; set; }
    public decimal Precio { get; set; }
}
