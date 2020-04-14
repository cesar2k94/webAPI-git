using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using webAPI.Contexts;
using webAPI.Entities;
using webAPI.Helpers;
using webAPI.Models;
using webAPI.Services;

namespace webAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(configuration => //este servicio se usa para configurar el mapeo, primero hay q agregar el paquete AutoMapper.Extensions.Microsoft.DependencyInjection
            {
                configuration.CreateMap<Autor, AutorDTO>();//Autor sería la fuente y AutorDTO el destino, los datos de Autor se mapean en AutorDTO, para eso tienen q tener el mismo nombre las variables en la fuente y en el destino 
            }, typeof(Startup));
            services.AddTransient<IHostedService, WriteToFileHostedService>();//la inyeccion de dependencia en la clase WriteToFileHostedSeervice
            services.AddScoped<MiFiltrodAccion>();//Habilitamos el Filtro q creé
            services.AddResponseCaching();//Habilitamos un conjunto de servicios para la funcionabilidad de guardar en CACHE//Filtros
            services.AddTransient<IClaseB, ClaseB>();//Para configurar la relacion de dependencia, configurar servicios, la interfaz está implementada como está en claseB
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers()
            .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //Esto es necesario para permitir las relaciones ciclicas entre clases
            services.AddMvc(Options => Options.Filters.Add(new MiFiltrodExcepcion()));//Aqui se ponen los filtros globales
                                                                                      //Si tuviera inyeccion de dependencia se pone:
                                                                                      // Options=>Options.Filters.Add(typeof(MiFiltrodExcepcion));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)//Middleware
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseResponseCaching();//Middleware de CACHE
                                     // app.UseMvc();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
