using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitEventbus(IMediator mediator, IServiceScopeFactory serviceScopeFactory) {
            _mediator = mediator;
            _manejadores = new Dictionary<string, List<Type>>();
            _eventoTipos = new List<Type>();
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task EnviarComando<T>(T comando) where T : Comando {
            return _mediator.Send(comando);
        }

        public void Publish<T>(T evento) where T : Evento {
            var factory = new ConnectionFactory() { HostName = "rabbit-torres-web" };
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
            var eventNombre = typeof(T).Name;
            var manejadorEventoTipo = typeof(TH);

            if(!_eventoTipos.Contains(typeof(T))) {
                _eventoTipos.Add(typeof(T));
            }

            if(!_manejadores.ContainsKey(eventNombre)) {
                _manejadores.Add(eventNombre, new List<Type>());
            }

            if(_manejadores[eventNombre].Any(x => x.GetType() == manejadorEventoTipo)) {
                throw new ArgumentException($"El manejador { manejadorEventoTipo.Name } fur registrado anteriormente  por { eventNombre }");
            }

            _manejadores[eventNombre].Add(manejadorEventoTipo);

            var factory = new ConnectionFactory() {
                HostName = "rabbit-torres-web",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(eventNombre, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Delegate;

            channel.BasicConsume(eventNombre, true, consumer);
        }

        private async Task Consumer_Delegate(object sender, BasicDeliverEventArgs e) {
            var nombre_Evento = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try {
                if(_manejadores.ContainsKey(nombre_Evento)) {
                    using(var scope = _serviceScopeFactory.CreateScope()) {
                        var suscripcions = _manejadores[nombre_Evento];
                        foreach(var sb in suscripcions) {
                            var manejador = scope.ServiceProvider.GetService(sb);//Activator.CreateInstance(sb);
                            if(manejador == null)
                                continue;

                            var tipoEvento = _eventoTipos.SingleOrDefault(x => x.Name == nombre_Evento);

                            var eventoDS = JsonConvert.DeserializeObject(message, tipoEvento);

                            var concretoTipo = typeof(IEventoManejador<>).MakeGenericType(tipoEvento);

                            await (Task)concretoTipo.GetMethod("Handle").Invoke(manejador, new object[] { eventoDS });
                        }
                    }
                }
            } catch(Exception ex) {

            }
        }
    }
}
