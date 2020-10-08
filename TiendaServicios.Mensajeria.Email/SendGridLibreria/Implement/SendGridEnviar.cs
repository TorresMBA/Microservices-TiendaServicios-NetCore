using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Interface;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Modelo;

namespace TiendaServicios.Mensajeria.Email.SendGridLibreria.Implement {
    public class SendGridEnviar : ISendGridEnviar {
        public async Task<(bool resultado, string errorMensaje)> EnviarEmail(SendGridData data) {
            try {
                var sendGridCliente = new SendGridClient(data.SendGridAPiKey);
                var destinatario = new EmailAddress(data.EmailDestinatario, data.NombreDestinatario);
                var titulo = data.Titulo;
                var sender = new EmailAddress("bryan98tm@gmail.com", "Brian Torres");
                var contenidoMensaje = data.Contenido;

                var objMensaje = MailHelper.CreateSingleEmail(sender, destinatario, titulo, contenidoMensaje, contenidoMensaje);

                await sendGridCliente.SendEmailAsync(objMensaje);

                return (true, null);
            } catch(Exception ex) {
                return (false, ex.Message);
            }
        }
    }
}
