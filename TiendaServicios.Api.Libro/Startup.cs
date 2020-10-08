using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libros.Persistencia;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.Implement;

namespace TiendaServicios.Api.Libro {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            //services.AddTransient<IRabbitEventBus, RabbitEventbus>();
            services.AddSingleton<IRabbitEventBus, RabbitEventbus>(sp => {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitEventbus(sp.GetService<IMediator>(), scopeFactory);
            });

            //Se modificara para poder iniciar el FluenteValidation
            services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            //La clase Configuration esta leyendo las propiedades del objeto json que se encuentra del archivo appsettings.json
            services.AddDbContext<ContextoLibreria>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("ConexionDB"));
            });

            //Agregare una linea para poder instanciar un objeto de tipo IMediator para que sea posible injectarlo en el Controller
            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);
            //Haciendo esto estoy inicilizando los servicios del MediaTr, ya la injeccion podra trabajar en cualquie tipo de consutrctor
            // que necesite utilizar este objeto

            services.AddAutoMapper(typeof(Consulta.Ejecuta));
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
