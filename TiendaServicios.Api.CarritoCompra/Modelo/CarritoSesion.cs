using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.CarritoCompra.Modelo {
    public class CarritoSesion {

        public int CarritoSesionId { get; set; }

        public DateTime FechaCreacion { get; set; }

        //Mi CarrtitoSesion tiene, esta almacenando un grupo productos que el usuario va a comprar
        //Relacion Uno a Muchos, Un CarritoSesion tiene muchos detalles, muchos productos
        public ICollection<CarritoSesionDetalle> ListaDetalle { get; set; }

    }
}
