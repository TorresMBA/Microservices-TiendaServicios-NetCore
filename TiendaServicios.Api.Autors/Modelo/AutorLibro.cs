using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Autor.Modelo {
    public class AutorLibro {
        public int AutorLibroId { get; set; }
        
        public string Nombre{ get; set; }

        public string Apellido{ get; set; }
        
        public DateTime? FechaNacimiento { get; set; }

        public ICollection<GradoAcademico> ListaGracoAcademico { get; set; }

        public string AutorLibroGuid { get; set; }

        //Code first - Migration De la creacion de componente sde software atravez de otro 
    }
}
