using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using webAPI.Helpers;

namespace webAPI.Entities
{
    public class Autor
    {
        public int Id { get; set; }
        [Required]//esto hace q la propiedad Nombre sea obligada llenarle,sino da error
        [PrimeraLetraMayuscula]
        [StringLength(10, ErrorMessage="El campo nombre debe tener {1} caracteres o menos")]
        public string Nombre { get; set; }
        public List<libros> Libros{get; set;}
    }
}