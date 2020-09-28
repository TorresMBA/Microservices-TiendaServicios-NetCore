using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Modelo;

namespace TiendaServicios.Api.Autor.Persistencia {
    public class ContextoAutor : DbContext {//Patron CQRS Segregación de Responsabilidad de Consulta de Comando (o Command Query Responsibility Segregation, en inglés).
        public ContextoAutor(DbContextOptions<ContextoAutor> options) : base(options){ }
        public DbSet<AutorLibro> AutorLibro { get; set; }

        public DbSet<GradoAcademico> GradoAcademico { get; set; }
    }
}
