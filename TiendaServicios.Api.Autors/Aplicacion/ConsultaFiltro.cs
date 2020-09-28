using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion {
    public class ConsultaFiltro {

        public class AutorUnico : IRequest<AutorDto> { 
            public string AutorGuid { get; set; }
        }

        public class Manejador : IRequestHandler<AutorUnico, AutorDto> {
            
            private readonly ContextoAutor _contexto;//Instanciar a la persistencia (DBSet)
            private readonly IMapper _mapper;

            public Manejador(ContextoAutor contexto, IMapper mapper) {
                _contexto = contexto;//Tambien debemos Injectarlo 
                _mapper = mapper;
            }
            public async Task<AutorDto> Handle(AutorUnico request, CancellationToken cancellationToken) {
                var autor = await _contexto.AutorLibro.Where(x => x.AutorLibroGuid == request.AutorGuid).FirstOrDefaultAsync();
                var autorDto = _mapper.Map<AutorLibro, AutorDto>(autor);
                if(autor == null) {
                    throw new Exception("No se encontro un autor");
                }

                return autorDto;
            }
        }
    }
}
