using Inventario.Domain.Dtos;
using Inventario.Domain.Repositories;
using Inventario.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventario.Infraestructure.Repositories;

public class ProductosServices : IProducto
{
    private readonly ILogger<ProductosServices> _logger;
    private readonly InventarioContext _context;

    public ProductosServices(ILogger<ProductosServices> logger, InventarioContext context)
    {
        _context = context;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<ProductoDto>> ObtenerProductos(Guid idUsuario)
    {
        try
        {
            var productos = await _context.Productos
                // .Where(p => p.IdUsuario == idUsuario)
                .Include(p => p.IdLoteNavigation)
                .Select(p => new ProductoDto
                {
                    IdProducto = p.IdProducto,
                    NombreProducto = p.NombreProducto,
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    FechaCreacion = p.FechaCreacion,
                    IdLote = p.IdLote,
                    NumeroLote = p.IdLoteNavigation.NumeroLote,
                    Stock = p.Stock,
                    FechaIngreso = p.FechaIngreso.ToString(),
                    FechaCaducidad = p.FechaCaducidad.ToString(),
                    Precio = (decimal)p.Precio,
                })
                .ToListAsync();

            return productos;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al obtener listado productos: {Message}", ex.Message);
            return [];
        }
    }

    public async Task<bool> AgregarProducto(AddProducto data)
    {
        try
        {
            var producto = new Producto
            {
                IdProducto = new Guid(),
                NombreProducto = data.NombreProducto,
                Descripcion = data.Descripcion,
                Codigo = data.Codigo,
                // IdUsuario = string.IsNullOrEmpty(data.IdUsuario) ? Guid.Empty : Guid.Parse(data.IdUsuario),
                FechaCreacion = DateTime.Now,
                IdLote = Guid.Parse(data.IdLote),
                Precio = (double?)data.Precio,
                FechaIngreso = DateOnly.Parse(data.FechaIngreso),
                FechaCaducidad = DateOnly.Parse(data.FechaCaducidad),
                Stock = data.Stock
            };

            _context.Productos.Add(producto);
            var resultado = await _context.SaveChangesAsync();

            return resultado > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al agregar producto: {Message}", ex.Message);
            return false;
        }
    }


    public async Task<bool> EditarProducto(AddProducto producto)
    {
        try
        {
            var productoExistente = await _context.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == producto.IdProducto);

            if (productoExistente == null)
            {
                return false;
            }

            productoExistente.NombreProducto = producto.NombreProducto;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Codigo = producto.Codigo;
            // productoExistente.IdUsuario = Guid.Parse(producto.IdUsuario);
            productoExistente.IdLote = Guid.Parse(producto.IdLote);
            productoExistente.Precio = (double)producto.Precio;
            productoExistente.FechaIngreso = DateOnly.Parse(producto.FechaIngreso);
            productoExistente.FechaCaducidad = DateOnly.Parse(producto.FechaCaducidad);

            var resultado = await _context.SaveChangesAsync();

            return resultado > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al editar producto: {Message}", ex.Message);
            return false; 
        }
    }


    public async Task<bool> EliminarProducto(Guid idProducto)
    {
        try
        {

            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == idProducto);

            if (producto == null)
            {
                return false; 
            }

            _context.Productos.Remove(producto);
            var resultado = await _context.SaveChangesAsync();

            return resultado > 0;  
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al eliminar producto: {Message}", ex.Message);
            return false;  
        }
    }
}