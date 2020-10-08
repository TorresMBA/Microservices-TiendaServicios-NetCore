using System;
using System.Collections.Generic;
using System.Text;
using TiendaServicios.RabbitMQ.Bus.Eventos;

namespace TiendaServicios.RabbitMQ.Bus.EventoQueue {
    public class EmailEventQueue : Evento {

        public string  Destinatanario { get; set; }

        public string  Titulo { get; set; }

        public string  Contenido { get; set; }

        public EmailEventQueue(string destinatario, string titulo, string contenido) {
            Destinatanario = destinatario;
            Titulo = titulo;
            Contenido = contenido;
        }
    }
}
