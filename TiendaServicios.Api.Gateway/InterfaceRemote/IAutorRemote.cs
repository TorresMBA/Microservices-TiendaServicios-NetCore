using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.LibroRemote;

namespace TiendaServicios.Api.Gateway.InterfaceRemote {
    public interface IAutorRemote {

        Task<(bool resultado, AutorModeloRemote AutorRemote, string ErrorMessage)> GetAutor(Guid AutorId);
    }
}
