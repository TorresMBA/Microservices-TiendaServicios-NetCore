using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteService {
    public class LibroService : ILibroService {

        private readonly IHttpClientFactory _httpCliente;
        private readonly ILogger<LibroService> _logger;

        public LibroService(IHttpClientFactory httpCliente, ILogger<LibroService> logger) {
            _httpCliente = httpCliente;
            _logger = logger;
        }

        public async Task<(bool resultado, LibroRemote Libro, string ErrorMessage)> GetLibro(Guid LibroId) {
            try {
                //"Libros" hace referencia al servicio que se agrego en la clase Startup
                //Esta instancia de la clase HttpClient esta creando un nuevo cliente y esta tomando la url base de la microservices de Libros 
                //que hemos configurado dentro de la clase Startup.cs
                var cliente = _httpCliente.CreateClient("Libros");

                //Este metodo GetAsync lo que hace es invocar al endpoint, en este caso se trata del end point que me va a devolver la descripciopn del libro
                //todos los campos de libros solo pasandole el parametro del id
                //api/LibroMaterial/{LibroId} -> ruta del controoller del microserive TiendaServicios.Api.Libro / Controllers / LibroMaterialController.cs
                var response = await cliente.GetAsync($"api/LibroMaterial/{LibroId}");
                if(response.IsSuccessStatusCode) {
                    var contenido = await response.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var resultado = JsonSerializer.Deserialize<LibroRemote>(contenido, option);
                    return (true, resultado, null);
                }

                return (false, null, response.ReasonPhrase);
            } catch(Exception ex) {
                _logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
