using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Libros.Modelo;

namespace TiendaServicios.Api.Libros.Persistencia {
    public class ContextoLibreria : DbContext {

        public ContextoLibreria() { }

        public ContextoLibreria(DbContextOptions<ContextoLibreria> options) : base(options) { }

        public virtual DbSet<LibreriaMaterial> LibreriaMaterial { get; set; }

        //Que hace esa propiedad virtual? Lo que permite es que se pueda sobreescribir a futuro
        //que es lo que necesita nuestro poyecto tienda servicios va a tomar linreria material y la va a sobre escribir
        //sino le pones virtual sera imposible y saltaran errores
    }
}
