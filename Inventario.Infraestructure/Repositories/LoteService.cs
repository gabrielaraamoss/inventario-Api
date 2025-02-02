using Inventario.Domain.Dtos;
using Inventario.Domain.Repositories;
using Inventario.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventario.Infraestructure.Repositories;

public class LoteService: ILote
{
    private readonly ILogger<LoteService> _logger;
    private readonly InventarioContext _context;

    public LoteService(ILogger<LoteService> logger ,InventarioContext context)
    {
        _context = context;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<LoteDto>> ObtenerLotes()
    {
        try
        {
            var lotes = await _context.Lotes
                .Select( l => new LoteDto
                    {
                        IdLote = l.IdLote,
                        NumeroLote = l.NumeroLote,
                        NombreLote = l.Descripcion
                    }
                ).ToListAsync();

            return lotes;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al obtener listado productos: {Message}", ex.Message);
            return [];
        }
    }

    public async Task<bool> AgregarLote(LoteDto data)
    {
        try
        {
            var nuevoLote = new Lote
            {
                IdLote = Guid.NewGuid(),  
                NumeroLote = data.NumeroLote,
                Descripcion = data.NombreLote 
            };

            await _context.Lotes.AddAsync(nuevoLote);
            await _context.SaveChangesAsync(); 

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al agregar lote: {Message}", ex.Message);
            return false;
        }
    }


    public async Task<ResultadoEliminacion> EliminarLote(Guid IdLote)
    {
        try
        {
            var lote = await _context.Lotes.FindAsync(IdLote);
        
            if (lote == null)
            {
                return new ResultadoEliminacion
                {
                    Resultado = false,
                    Mensaje = "No existe lote."
                };

            }

            var tieneProductos = await _context.Productos.AnyAsync(p=> p.IdLote == IdLote);

            if (tieneProductos)
            {
                return new ResultadoEliminacion
                {
                    Resultado = false,
                    Mensaje = "No se puede eliminar, tiene productos asociados."
                };
            }
            _context.Lotes.Remove(lote);
            await _context.SaveChangesAsync();

            return new ResultadoEliminacion
            {
                Resultado = true,
                Mensaje = "Lote eliminado exitosamente"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al eliminar lote: {Message}", ex.Message);
            return new ResultadoEliminacion
            {
                Resultado = false,
                Mensaje = "Error al eliminar lote"
            };
        }
    }

    public async Task<bool> EditarLote(LoteDto data)
    {
        try
        {
            var lote = await _context.Lotes.FindAsync(data.IdLote);

            if (lote == null)
            {
                return false; 
            }

            lote.NumeroLote = data.NumeroLote;
            lote.Descripcion = data.NombreLote;

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error al editar lote: {Message}", ex.Message);
            return false;
        }
    }

}