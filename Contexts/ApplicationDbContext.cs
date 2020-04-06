using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webAPI.Entities;

namespace webAPI.Contexts
{
    public class ApplicationDbContext: DbContext
    {
        //Configurar ENtityFramework Core
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<Autor> Autores {get; set; }//la base de datos va tener una tabla llamada Autores del formato de la clase Autor
        public DbSet<libros> Libros{get; set;}
    }
}