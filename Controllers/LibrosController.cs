using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webAPI.Contexts;
using webAPI.Entities;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController: ControllerBase
    {
        private ApplicationDbContext context;

        public LibrosController( ApplicationDbContext context)
        {
            this.context=context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<libros>> Get()
        {
            return context.Libros.Include(x=>x.Autor).ToList();
        }
        [HttpGet("{id}",Name= "Obtener libro")]
        public ActionResult <libros> Get(int id)
        {
            var libro = context.Libros.Include(x=>x.Autor).FirstOrDefault(x=>x.Id==id);
            if (libro == null)
            {
                return NotFound();
            }
            return libro;
        }
        [HttpPost]
        public ActionResult Post([FromBody] libros Libro)
        {
            context.Libros.Add(Libro);
            context.SaveChanges();
            return new CreatedAtRouteResult("Obtener libro", new {id = Libro.Id, Libro});
        }
        [HttpPut("{id}")]
        public ActionResult Put(int id,[FromBody] libros Libro)
        {
            if (id != Libro.Id)//no existe el id del libro
            {
                 return BadRequest();
            }
             context.Entry(Libro).State=EntityState.Modified;//actualiza el libro
             context.SaveChanges();//guarda los cambios
             return Ok();//Operacion realizada con exito  
        }
        [HttpDelete("{id}")]
        public ActionResult<libros> Delete (int id)
        {
            var libro =context.Libros.FirstOrDefault(x=> x.Id==id);//Selecciona el livro q se quiere borrar
            if(libro==null)
            {
                return NotFound();//No existe el libro
            }

            context.Libros.Remove(libro);//Se elimina el libro
            context.SaveChanges();
            return libro;
        }
    }
}