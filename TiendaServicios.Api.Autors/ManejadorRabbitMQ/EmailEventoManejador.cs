using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Interface;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Modelo;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;

namespace TiendaServicios.Api.Autors.ManejadorRabbitMQ {
    public class EmailEventoManejador : IEventoManejador<EmailEventQueue> {

        private readonly ILogger<EmailEventoManejador> _logger;

        private readonly ISendGridEnviar _sendGridEnviar;

        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration; // Para poder usar el appsettings desde una clase C# se tiene que injectar el IConfiguration

        public EmailEventoManejador() { }

        public EmailEventoManejador(ILogger<EmailEventoManejador> logger, ISendGridEnviar sendGridEnviar, Microsoft.Extensions.Configuration.IConfiguration configuration) {
           _logger = logger;
            _sendGridEnviar = sendGridEnviar;
            _configuration = configuration;
        }

        public async Task Handle(EmailEventQueue @event) {
            //_logger.LogInformation($"Este es el valor que consumo desde RabbitMQ {@event.Titulo}");
            var objData = new SendGridData();
            objData.Contenido = @event.Contenido;
            objData.EmailDestinatario = @event.Destinatanario;
            objData.NombreDestinatario = @event.Destinatanario;
            objData.Titulo = @event.Titulo;
            objData.SendGridAPiKey = _configuration["SendGrid:ApiKey"];

            var resultado = await _sendGridEnviar.EnviarEmail(objData);
            if(resultado.resultado) {
                await Task.CompletedTask;
                return;
            }
        }
    }
}
