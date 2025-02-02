using System.Text;
using Inventario.Domain.Dtos;
using Inventario.Domain.Repositories;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Inventario.Infraestructure.Repositories
{
    public class UsuarioService : IUsuario
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _authServiceUrl;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<UsuarioService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _authServiceUrl = configuration.GetConnectionString("AuthService");
            _logger = logger;
        }

        public async Task<MensajesResponse> IniciarSesion(string username, string password)
        {
            var loginRequest = new LoginRequest
            {
                Username = username,
                Password = password
            };

            var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

            try
            {
                
                var client = _httpClientFactory.CreateClient();

                var response = await client.PostAsync(_authServiceUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);

                    if (authResponse?.Resultado == true)
                    {
                        return new MensajesResponse
                        {
                            Token = authResponse.Token,
                            Resultado = true,
                            Mensaje = "Autenticación exitosa"
                        };
                    }
                    else
                    {
                        _logger.LogError($"Autenticación fallida: {authResponse?.Messaje}");
                        return new MensajesResponse
                        {
                            Token = null,
                            Resultado = false,
                            Mensaje = authResponse?.Messaje ?? "Credenciales incorrectas"
                        };
                    }
                }

                _logger.LogError($"Error en la respuesta de AuthService: {response.StatusCode}");
                return new MensajesResponse
                {
                    Token = null,
                    Resultado = false,
                    Mensaje = "No existe usuario"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al conectar con el AuthService: {ex.Message}");
                return new MensajesResponse
                {
                    Token = null,
                    Resultado = false,
                    Mensaje = "Error interno al procesar la solicitud"
                };
            }
        }
    }


}
