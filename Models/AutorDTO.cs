using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webAPI.Models
{
    public class AutorDTO //se usa AutorDTO para mostrar solo los datos q se quieren y no todos los datos, a clase Autor es para uso interno mientra q AutorDTO es lo q vamos a mostrar
    {
        public int Id {get; set;}
        [Required]
        public string Nombre {get; set;}
       // public DateTime FechaNacimiento {get;set;}
        public List<LibroDTO>Libros{get; set;}
       // public List<LibroDTO> Books {get;set;}

    }
}