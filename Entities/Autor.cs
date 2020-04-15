using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using webAPI.Helpers;

namespace webAPI.Entities
{
    public class Autor : IValidatableObject //Validacion personalizada por modelos, heredamos de esta clase si vamos a realizar validacion por objetos
    {
        public int Id { get; set; }
        [Required]//esto hace q la propiedad Nombre sea obligada llenarle,sino da error
        [PrimeraLetraMayuscula] //Validacion por atributos
        [StringLength(10, ErrorMessage="El campo nombre debe tener {1} caracteres o menos")]
        public string Nombre { get; set; }
        public List<libros> Libros{get; set;}

        public string Identification {get; set;}
       public DateTime FechaNacimiento {get; set;}
        //public List<libros> Books {get;set;}    
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)//Validacion por modelos, en este caso ya uso la validacion por atributos(PrimeraLetraMayuscula) pero realice la validacion por modelos en modo de prueba
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeLetra = Nombre[0].ToString();
                if (primeLetra != primeLetra.ToUpper())
                {
                    yield return new ValidationResult("La 1ra letra debe ser mayúscula",new string[] {nameof(Nombre)});//uso yield xq retorno un elemento de un IEnumerable
                }                
            }
        }
        
    }
} 