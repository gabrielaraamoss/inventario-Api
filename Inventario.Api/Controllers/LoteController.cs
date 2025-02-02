using Inventario.Domain.Dtos;
using Inventario.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Controllers;

public class LoteController : ControllerBase
{

    private readonly ILogger<LoteController> _logger;
    private readonly ILote _lote;
    
    public LoteController( ILogger<LoteController> logger,ILote lote) 
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _lote = lote;
    }

    [HttpGet("ObtenerLotes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<LoteDto>>> ObtenerLotes()
    {
        try
        {
            var lotes = await _lote.ObtenerLotes();
            return Ok(lotes);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al obtener lotes: {Message}", ex.Message);
            return StatusCode(500, "Hubo un problema al obtener los lotes.");
        }
    }

    [HttpPost("AgregarLote")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AgregarLote([FromBody] LoteDto data)
    {
        try
        {
            var resultado = await _lote.AgregarLote(data);

            if (!resultado)
            {
                return BadRequest("No se pudo agregar el lote.");
            }

            return CreatedAtAction(nameof(ObtenerLotes), new { id = data.IdLote }, data); 
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al agregar lote: {Message}", ex.Message);
            return StatusCode(500, "Hubo un problema al agregar el lote.");
        }
    }

    [HttpPut("EditarLote")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> EditarLote([FromBody] LoteDto data)
    {
        try
        {

            var resultado = await _lote.EditarLote(data);

            if (!resultado)
            {
                return NotFound("Lote no encontrado.");
            }

            return NoContent(); 
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al editar lote: {Message}", ex.Message);
            return StatusCode(500, "Hubo un problema al editar el lote.");
        }
    }


    [HttpDelete("EliminarLote/{idLote}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> EliminarLote(Guid idLote)
    {
        try
        {
            var resultado = await _lote.EliminarLote(idLote);

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al eliminar lote: {Message}", ex.Message);
            return StatusCode(500, "Hubo un problema al eliminar el lote.");
        }
    }
    
    
}