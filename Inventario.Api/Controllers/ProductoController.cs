using Inventario.Domain.Dtos;
using Inventario.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Controllers;


public class ProductoController : ControllerBase
{
    private readonly ILogger<ProductoController> _logger;
    private readonly IProducto _producto;
    
    public ProductoController( ILogger<ProductoController> logger,IProducto producto) 
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _producto = producto;
    }
    
    
    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "all")]
    public async Task<ActionResult> CrearProducto([FromBody] AddProducto data)
    {
        try
        {
            var resultado = await _producto.AgregarProducto(data);

            if (!resultado)
            {
                return BadRequest("No se pudo agregar producto.");
            }

            return CreatedAtAction(nameof(ObtenerProductos), new { id = data.IdProducto }, data); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al crear producto: {ex.Message}"); 
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ProductoDto>>> ObtenerProductos(Guid idUsuario)
    {
        try
        {

            var productos = await _producto.ObtenerProductos(idUsuario);
            return Ok(productos);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al obtener producto " );
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener los productos. Por favor, inténtelo de nuevo más tarde.");
        }

    }

    [HttpPut("EditarProducto")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> EditarProducto([FromBody] AddProducto producto)
    {
        try
        {
            
            var resultado = await _producto.EditarProducto(producto);

            if (!resultado)
            {
                return NotFound("Producto no encontrado o no se pudo actualizar.");
            }

            return Ok("Producto editado correctamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al editar producto: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
        }
    }

    [HttpDelete("EliminarProducto/{idProducto}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> EliminarProducto(Guid idProducto)
    {
        try
        {


            var resultado = await _producto.EliminarProducto(idProducto);

            if (!resultado)
            {
                return NotFound("Producto no encontrado o no se pudo eliminar.");
            }

            return Ok("Producto eliminado correctamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al eliminar producto: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
        }
    }
    
}