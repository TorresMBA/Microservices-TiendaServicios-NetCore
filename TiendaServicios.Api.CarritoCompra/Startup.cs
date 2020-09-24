using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TiendaServicios.Api.CarritoCompra.Aplicacion;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteService;

namespace TiendaServicios.Api.CarritoCompra {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddScoped<ILibroService, LibroService>();
            services.AddControllers();

            //Configuration tiene acceso a las propiedades json que tiene appsettings.json
            services.AddDbContext<CarritoContexto>(opt => {
                opt.UseMySQL(Configuration.GetConnectionString("ConexionDatabase"));
            });

            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);

            //Estamos indicandole que agregue dentro del HttpClient el servicio Libros con el endPoint de Service: Libros que esta dentro de appsetting.json
            services.AddHttpClient("Libros", config => {
                config.BaseAddress = new Uri(Configuration["Services:Libros"]); 
             });

            //
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
