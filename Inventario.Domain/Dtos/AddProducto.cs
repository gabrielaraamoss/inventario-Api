public class AddProducto
{
    public Guid? IdProducto { get; set; }
    public string NombreProducto { get; set; }
    public string Codigo { get; set; }
    public string Descripcion { get; set; }
    public string? IdUsuario { get; set; }
    public string IdLote { get; set; }
    public int Stock { get; set; }
    public decimal Precio { get; set; }
    public string? FechaIngreso { get; set; }
    public string? FechaCaducidad { get; set; }
}