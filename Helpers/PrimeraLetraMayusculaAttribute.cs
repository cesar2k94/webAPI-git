using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webAPI.Helpers
{
    public class PrimeraLetraMayusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)//en value esta el valor de la propiedad donde se ha creado el atributo
        {
           if (value == null|| string.IsNullOrEmpty(value.ToString()))//si el valor es nulo o vacio
           {
               return ValidationResult.Success;//validación exitosa
           }
           var firstLetter= value.ToString()[0].ToString();//obtenemos el valor de la primera letra
           if (firstLetter != firstLetter.ToUpper())//Comparamos a ver si la primera letra es mayuscula
           {
               return new ValidationResult("La primera letra debe ser mayuscula");
           }
           return ValidationResult.Success;//validacion exitosa
        }   
    }
}