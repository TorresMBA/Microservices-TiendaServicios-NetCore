using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaServicios.RabbitMQ.Bus.Eventos {
    public abstract class Evento {
        public DateTime TimesStamp { get; protected set; }

        public Evento() {
            TimesStamp = DateTime.Now;
        }
    }
}
