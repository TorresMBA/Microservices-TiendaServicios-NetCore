using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.InterfaceRemote;
using TiendaServicios.Api.Gateway.LibroRemote;

namespace TiendaServicios.Api.Gateway.MessageHandler {
    public class LibroHandler : DelegatingHandler {

        private readonly ILogger<LibroHandler> _logger;

        private readonly IAutorRemote _autorRemote;

        public LibroHandler(ILogger<LibroHandler> logger, IAutorRemote autorRemote) {
            _logger = logger;
            _autorRemote = autorRemote;
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancel) {
            var tiempo = Stopwatch.StartNew();
            _logger.LogInformation("Inicia el request");
            var respone = await base.SendAsync(request, cancel);
            if(respone.IsSuccessStatusCode) {
                var contenido = await respone.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };
                var resultado = JsonSerializer.Deserialize<LibroModeloRemote>(contenido, options);
                var responseAutor = await _autorRemote.GetAutor(resultado.AutorLibro ?? Guid.Empty); //Si se declara un Guid? para que acepte nulos, pero nos pide un Guid que no acepte nulos entonces se agrega eso apra forzar
                if(responseAutor.resultado) {
                    var oAutor = responseAutor.AutorRemote;
                    resultado.AutorData = oAutor;
                    var resultadoStr = JsonSerializer.Serialize(resultado);
                    respone.Content = new StringContent(resultadoStr, System.Text.Encoding.UTF8, "application/json");
                }
            }

            _logger.LogInformation($"Este Proceso se hizo en {tiempo.ElapsedMilliseconds}ms");
            return respone;
        }
    }
}
