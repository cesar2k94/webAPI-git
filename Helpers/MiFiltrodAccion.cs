using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace webAPI.Helpers
{
    public class MiFiltrodAccion : IActionFilter//Creando mi propio filtro
    {
        private readonly ILogger<MiFiltrodAccion> logger;

        public MiFiltrodAccion(ILogger<MiFiltrodAccion> logger)//COn el ILogger estamos realizando inyeccion de dependencia
        {
            this.logger=logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)//Se ejecuta antes de la accion 
        {
            logger.LogError("OnActionExecuting");
        }
        public void OnActionExecuted(ActionExecutedContext context)//Se ejecuta despues de la accion
        {
           logger.LogError("OnActionExecuted");
        }

        
    }
}