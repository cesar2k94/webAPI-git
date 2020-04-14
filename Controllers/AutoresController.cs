using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webAPI.Contexts;
using webAPI.Entities;
using webAPI.Helpers;
using webAPI.Models;
using webAPI.Services;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IClaseB claseB;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IClaseB claseB, IMapper mapper)//AL agregar ICLaseB claseB se esta realizando inyencion de dependencia, de bajo acoplamiento
        {
            this.context = context;
            this.claseB= claseB;
            this.mapper= mapper; //al poner IMapper es para poder usar el mapeo
        }
        [HttpGet]
        //[ServiceFilter(typeof(MiFiltrodAccion))]//Aqui pongo como atributo el filtro q creé, tenemos q usar ServiceFilter xq estoy usando inyencion de dependencia,sino no tuviera q ponerlo
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get()
        {
           /* claseB.HacerAlgo();
            return context.Autores.Include(x=>x.Libros).ToList();*/
           //Si quiero retornar una lista de AutorDTO:
            var autores= await context.Autores.ToListAsync();
            var autoresDTO=mapper.Map<List<AutorDTO>>(autores);
            return autoresDTO;
        }
        [HttpGet("Primer")]
        public ActionResult <Autor> GetPrimerAutor()
        {
            return context.Autores.FirstOrDefault();
        }
        [HttpGet("{id}", Name= "Obtener Autor")]
        public async Task<ActionResult<AutorDTO>> Get (int id)// el async el Task es para usar prog asincrona
        {
            var autor =  await context.Autores.Include(x=>x.Libros).FirstOrDefaultAsync(x=> x.Id == id);//con el await es para q sea programacion asincrona
            if (autor == null)
            {
                return NotFound();
            }
           
           var autorDTO = mapper.Map<AutorDTO>(autor);// Se mapea autor a un tipo autorDTO
           return autorDTO;
            /*return new AutorDTO()
            {
                Id = autor.Id,
                Nombre=autor.Nombre
            };//se muestran los datos de la clase AutorDTO*/
        }
        [HttpPost]
        public ActionResult Post([FromBody] Autor autor)//se recibe un autor del cliente y se incorpora a la BD, ese autor viene en el cuerpo de la peticion http por eso se pone FromBody
        {
            context.Autores.Add(autor);//se agrega autor en la BD
            //context.SaveChanges();
            return new CreatedAtRouteResult("Obtener Autor", new {id = autor.Id},autor);//primero se pone la ruta donde el cliente va localizar el recurso, despues se pone los parametros de la accion y espues se pone en el cuerpo de la rspuesta autor  
        }
        [HttpPut("{id}")]//el Put es para actualizar le recurso de autor
        public ActionResult Put(int id, [FromBody] Autor value)
        {
            if (id!=value.Id)
            {
                return BadRequest();
            }
            context.Entry(value).State= EntityState.Modified;// se actualiza el registro
            context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public ActionResult<Autor> Delete(int id)
        {
            var autor= context.Autores.FirstOrDefault(x=>x.Id==id);
            if (autor == null)
            {
                return NotFound();
            }
            context.Autores.Remove(autor);
            context.SaveChanges();
            return autor;
        }
    }
}