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
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autors {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //Debemos incluirlo como parte de los servicios que se van a Instanciar al arrancar mi programa dentro esta clase (Startup.cs) dentron de Configure Service
        //Fluent Validation, ContextoAturo, Libreria MediaTr
        public void ConfigureServices(IServiceCollection services) {
            //si quiero editar y creo otra clase para editar autores? Se tendria que agregar una linea adicionar del Fluent?
            //no es necesario hacerlo porque desde el momento que arranque mi proyecyo el lfuent validation lo que hara es buscar 
            //todas las clases c# que esten herando de la clase abstranValidation y los va a instanciar automaticamente 
            //Base => services.AddControllers(); Modificando =>
            services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());// De esta forma es como se inicializa el Fluent Validation dentro del proyecto
                                                                                                                        //En este caso en concreto estoy indicandolo que incliya el fluent validation en la clase Nuevo
            //Porque bota error en esta en linea en Azure DevOps
            //introduciendo lineas para modificar en github y azure
            services.AddDbContext<ContextoAutor>(options => {
                //options.UseNpgsql(Configuration.GetConnectionString("ConexionDataBase"));
                options.UseNpgsql(Configuration.GetConnectionString("ConexionDataBase"));
            });

            services.AddMediatR(typeof(Nuevo.Manejador).Assembly); //MediaTR

            services.AddAutoMapper(typeof(Consulta.Manejador));
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
