using Inventario.Domain.Dtos;

namespace Inventario.Domain.Repositories;

public interface IProducto
{
    Task<List<ProductoDto>> ObtenerProductos(Guid idUsuario);
    Task<bool> AgregarProducto(AddProducto producto);
    Task<bool> EditarProducto(AddProducto producto);
    Task<bool> EliminarProducto(Guid idProducto);
    
}