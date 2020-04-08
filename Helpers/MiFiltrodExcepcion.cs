using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace webAPI.Helpers
{
    public class MiFiltrodExcepcion: ExceptionFilterAttribute//Filtro de Excepcion
    {
        public override void OnException(ExceptionContext context)
        {

        }
    }
}