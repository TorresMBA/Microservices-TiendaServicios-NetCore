using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libros.Modelo;
using TiendaServicios.Api.Libros.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests {
    public class LibrosServiceTests {
        //Unit Testing

        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba() {
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            //ListOf este metoddo se encarga de ejecucion de la data y devolverme la lista de elementos
            var lista = A.ListOf<LibreriaMaterial>(30);
            lista[0].LibreriaMaterialId = Guid.Empty;

            return lista;
        }

        private Mock<ContextoLibreria> CrearContexto() {
            //Lo primero que debemos hacer en este meotodo es obtener la data de prueba
            //que necesito incluit en este contexto(ContextoLibreria)

            var dataPrueba = ObtenerDataPrueba().AsQueryable();

            var dbSet = new Mock<DbSet<LibreriaMaterial>>();

            //estamos indicando que la clase libreriamaterial tiene que ser una clase de tipo entidad para eso
            //debemos levantarla, darle la configuracion de Setup y darle un Provider darle un Expression un ElementType y GetEnumerator
            //estas son las propiedades que debe tener toda clase de entity framework
            //nosotros Al no estar trabajando con una configuracion de sql server o una instancia de persistencia 
            //debemos hacer manualmente este trabajo
            //DbSet Configurado y tambien eh configurado que LibreriaMaterial es una entidad de tipo entity framework que va a consumir la data
            //de pruena que e generado en el metodo ObtenerDataPrueba
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken())).Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));

            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);
            return contexto;
        }

        [Fact]
        public async void GetLibroPorId() {

            var mockContexto = CrearContexto();
            var mapConfig = new MapperConfiguration(cfg => {
                    cfg.AddProfile(new MappingTest());
                });
            var mapper = mapConfig.CreateMapper();

            var request = new ConsultaFiltro.LibroUnico();
            request.LibroId = Guid.Empty;

            var manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mapper);
            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(libro);
            Assert.True(libro.LibreriaMaterialId == Guid.Empty);
        }

        [Fact]
        public async void GetLibros() {
            //System.Diagnostics.Debugger.Launch();

            //Que metodo de mi microservice libro se esta encargando             
            //de realizar la consulta  de libros de la base de datos??

            //1. Emular a la instancia de entity framework core - ContextoLibreria
            //  para emular las acciones y evento de un objeto en un ambiente de Unit Test
            //  utilizamos objetos de tipo mock
            //Que es un Mock? Un mock es la representacion de un objeto que puede actuar como un componente
            //de software real pero que solo puede ser controlado en el codigo de Test
            //Resumen, un mock que puede representar cualquier elemento de tu codigo, se disfraza de cualquier actor
            //de tu codigo para conseguir la prueba que deseas, en nuestro caso que el objeto mock represente
            //o disfraze de ContextoLibreria

            var mockContexto = CrearContexto();

            //2. Emular al objeto mapping IMapper qie esta en la clase Manejador

            var mapConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();

            //3. Instancia a la clase Manejador y pasarle com parametros los mocks que eh creado
            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mapper);

            Consulta.Ejecuta request = new Consulta.Ejecuta();

            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            //Any pertenece a la libreria LinQ
            //Any me va a devolver true o false, si hay algun valor me da verdadero y el test pasa
            // sino hay valores significa que hay errores dentro de mi logica
            Assert.True(lista.Any());
        }

        [Fact]
        public async void GuardarLibro() {
            //Solo se debe usar en un metodo ala vez  no en dos metodos al mismo tiempo
            System.Diagnostics.Debugger.Launch();

            //Se Usa la libreria Microsoft.Entity.FrameworkCore 
            //que nos ayudara crear una base de datos en memoria 

            //Este objeto nos representa la configuracion que va a tener la base de datos en memoria
            var options = new DbContextOptionsBuilder<ContextoLibreria>()
                .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
                .Options;

            var contexto = new ContextoLibreria(options);

            var reques = new Nuevo.Ejecuta();
            reques.Titulo = "Libro de Microservice";
            reques.AutorLibro = Guid.Empty;
            reques.FechaPublicacion = DateTime.Now;

            var manejador = new Nuevo.Manejador(contexto);
            var libro = await manejador.Handle(reques, new System.Threading.CancellationToken());

            Assert.True(libro != null);
        }
    }
}
