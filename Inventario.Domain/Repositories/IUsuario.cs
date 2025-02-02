using Inventario.Domain.Dtos;

namespace Inventario.Domain.Repositories;

public interface IUsuario
{
    Task<MensajesResponse> IniciarSesion(string username, string password);
}