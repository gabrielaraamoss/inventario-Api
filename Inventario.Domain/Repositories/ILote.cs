using Inventario.Domain.Dtos;

namespace Inventario.Domain.Repositories;

public interface ILote
{
    Task<List<LoteDto>> ObtenerLotes();
    Task<bool> AgregarLote(LoteDto data);
    Task<ResultadoEliminacion> EliminarLote(Guid IdLote);
    Task<bool> EditarLote(LoteDto data);
}