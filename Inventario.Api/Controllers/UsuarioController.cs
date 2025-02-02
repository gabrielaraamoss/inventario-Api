using Inventario.Domain.Dtos;
using Inventario.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventarioApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuario _usuarioService;
    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(IUsuario usuarioService, ILogger<UsuarioController> logger)
    {
        _usuarioService = usuarioService;
        _logger = logger;
    }

    [HttpPost("IniciarSesion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> IniciarSesion([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _usuarioService.IniciarSesion(request.Username, request.Password);

            return Ok(new { Token = response.Token, Mensaje = response.Mensaje });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al iniciar sesión: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { Mensaje = "Error interno del servidor." });
        }
    }

}