using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.CarritoCompra.Modelo {
    public class CarritoSesionDetalle {

        public int CarritoSesionDetalleId { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string ProductoSelecionado { get; set; }

        //Gnerando Llave Foranea
        public int CarritoSesionId { get; set; }

        //A nivel de objetos necesito soportar esa clave foraena con un objeto 
        public CarritoSesion CarritoSesion { get; set; }
        //De esta forma eh creado el ancla y la clave foranea para soportar la relacion
        //de uno a muchos entre CarritoSesion y CarritoSesionDetalle
    }
}
