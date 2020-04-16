using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
        [HttpGet("{id}", Name= "ObtenerAutor")]//con Name le pongo un nombre a la regla de routeo
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
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacion)//se recibe un autor del cliente y se incorpora a la BD, ese autor viene en el cuerpo de la peticion http por eso se pone FromBody
        {
            var autor =  mapper.Map<Autor>(autorCreacion);
            context.Add(autor);//se agrega autor en la BD
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);
            return new CreatedAtRouteResult("ObtenerAutor", new {id = autor.Id},autorDTO);//primero se pone la ruta donde el cliente va localizar el recurso, despues se pone los parametros de la accion y espues se pone en el cuerpo de la rspuesta autor  
        }
        [HttpPut("{id}")]//el Put es para actualizar le recurso de autor
        public ActionResult Put(int id, [FromBody] AutorCreacionDTO valueactualizacion)
        {
            var autor = mapper.Map<Autor>(valueactualizacion);
            autor.Id = id;
            context.Entry(autor).State= EntityState.Modified;// se actualiza el registro
            context.SaveChanges();
            return Ok();
        }

        [HttpPatch("{id}")]//el Patch para actualizaciones parciales del recurso autor 
        public async Task<ActionResult> Patch(int id, [FromBody]JsonPatchDocument<AutorCreacionDTO> patchDocument)//se pone JsonPatchDocument xq en el curpo de la peticion se reciben varias operaciones
        {
            if(patchDocument==null)
            {
                return BadRequest();
            }
            var autorDeLaBD = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);//Buscando autor de la BD
            if(autorDeLaBD==null)
            {
                return NotFound();
            }

            var autorDTO = mapper.Map<AutorCreacionDTO>(autorDeLaBD);
            patchDocument.ApplyTo(autorDTO, ModelState);//Se pasa ModelState para saber si el modelo es valido. Esta linea de codigo se usa para aplicar las operaciones q hemos recibido al autor De la Base de datos

            mapper.Map(autorDTO, autorDeLaBD);

            var IsValid = TryValidateModel(autorDeLaBD);
            if(!IsValid)//para ver si el modelo es valido
            {
                return BadRequest(ModelState);//EN el ModelState se guardan los errores de validacion
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Autor>> Delete (int id)
        {
            var autorId = await context.Autores.Select(x=>x.Id).FirstOrDefaultAsync(x=>x==id);//con Select selecciono las columnas en la tabla q quiero leer
            if (autorId == default(int))//por si no existe el autor con ese id
            {
                return NotFound();
            }
            context.Remove(new Autor {Id = autorId });//se borra el autor q tenga ese ID
            await context.SaveChangesAsync();
            return Ok(autorId);
        }
    }
}