using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Autor.Aplicacion {

    //DTO(Data Transfer Object) ó Objecto de Tranferencia de datos
    //Es modelar la data que se va a enviar al cliente mediante filtros o en casos merge osea uniones
    public class AutorDto {
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string AutorLibroGuid { get; set; }
    }
}
