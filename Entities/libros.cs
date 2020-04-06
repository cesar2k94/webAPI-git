using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace webAPI.Entities
{
    public class libros
    {
        public int Id{get; set;}
        [Required]
        public string Titulo{get; set;}
        [Required]
        public int AutorId{get; set;}
        public Autor Autor{get; set;}

    }
}