using System;
using MediatR;
using System.Collections.Generic;
using System.Text;

namespace TiendaServicios.RabbitMQ.Bus.Eventos {
    public abstract class Message : IRequest<bool>{

        public string MessageType { get; protected set; }

        protected Message() {
            MessageType = GetType().Name;
        }
    }
}
