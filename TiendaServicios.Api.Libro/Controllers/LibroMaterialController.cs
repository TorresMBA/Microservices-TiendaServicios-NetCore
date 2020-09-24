using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Libro.Aplicacion;

namespace TiendaServicios.Api.Libro.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LibroMaterialController : ControllerBase {

        //Para que que esta injecion trabaje correctemante en este constructor necesito
        // que  el IMediator se inicialice dentro del proyecto a travez de la clase Startup.cs
        private readonly IMediator _mediator;

        public LibroMaterialController(IMediator mediator) {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data) {
            return await _mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<List<LibroMaterialDto>>> GetLibros(){
            return await _mediator.Send(new Consulta.Ejecuta());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroMaterialDto>> LibroUnico(Guid id) {
            return await _mediator.Send(new ConsultaFiltro.LibroUnico { LibroId = id });
        }
    }
}
