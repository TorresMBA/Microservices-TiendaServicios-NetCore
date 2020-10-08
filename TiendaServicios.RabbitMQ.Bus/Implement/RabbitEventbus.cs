using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.Comandos;
using TiendaServicios.RabbitMQ.Bus.Eventos;

namespace TiendaServicios.RabbitMQ.Bus.Implement {
    public class RabbitEventbus : IRabbitEventBus {

        private readonly IMediator _mediator;

        private readonly Dictionary<String, List<Type>> _manejadores;

        private readonly List<Type> _eventoTipos;

        public RabbitEventbus(IMediator mediator) {
            _mediator = mediator;
            _manejadores = new Dictionary<string, List<Type>>();
            _eventoTipos = new List<Type>();
        }

        public Task EnviarComando<T>(T comando) where T : Comando {
            return _mediator.Send(comando);
        }

        public void Publish<T>(T evento) where T : Evento {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection()) {
                using(var channel = connection.CreateModel()) {
                    var eventName = evento.GetType().Name;
                    channel.QueueDeclare(eventName, false, false, false, null);
                    var message = JsonConvert.SerializeObject(evento);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("", eventName, null, body);
                }
            }
        }

        public void Suscribe<T, TH>() where T : Evento
                                      where TH : IEventoManejador<T> {
            throw new NotImplementedException();
        }
    }
}
