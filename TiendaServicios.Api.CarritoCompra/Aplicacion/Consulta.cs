using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion {
    public class Consulta {

        public class Ejecuta : IRequest<CarritoDto> {
            public int CarritoSesionId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDto> {

            public readonly CarritoContexto _contexto;
            public readonly ILibroService _libroService;

            public Manejador(CarritoContexto contexto, ILibroService libroService) {
                _contexto = contexto;
                _libroService = libroService;
            }

            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken) {
                var carritoSesion = await _contexto.CarritoSesion.FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);
                var carritoSesionDetalle = await _contexto.CarritoSesionDetalle.Where(x => x.CarritoSesionId == request.CarritoSesionId).ToListAsync();

                var listaCarritoDto = new List<CarritoDetalleDto>();

                foreach(var libro in carritoSesionDetalle) {
                    var response = await _libroService.GetLibro(new Guid (libro.ProductoSelecionado));
                    if(response.resultado) {
                        //objLibro es el resultado de la microservice Libro
                        var objLibro = response.Libro;

                        //Estoy usando esos resultaod para llenar el nuevo objeto CarritoDetalleDto
                        var carritoDetalle = new CarritoDetalleDto() {
                            TituloLibro = objLibro.Titulo,
                            FechaPublicacion = objLibro.FechaPublicacion,
                            LibroId = objLibro.LibreriaMaterialId
                        };

                        //una vez teniendo mi carritoDetalleDto lleno, cargado lo voy a agregar a mi lista
                        listaCarritoDto.Add(carritoDetalle);
                    }
                }

                var carritoSesionDto = new CarritoDto {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProductos = listaCarritoDto
                };

                return carritoSesionDto;
            }
        }
    }
}
