using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Libro.Aplicacion {
    public class LibroMaterialDto {

        //Creare las propiedades que devolvere al cliente
        public Guid? LibreriaMaterialId { get; set; }

        public string Titulo { get; set; }

        public DateTime? FechaPublicacion { get; set; }

        public Guid? AutorLibro { get; set; }
    }
}
