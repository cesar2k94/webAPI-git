using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace webAPI.Services
{
    public class WriteToFileHostedService : IHostedService//Al inico y al final del ciclo de vida de la API se va realizar una accion, en este caso escribir en un archivo de texto
    {
        private readonly IHostEnvironment environment;//Se usa IHostEnvironment para obtener el directorio donde se encuentra la aplicacion ejecutandose
        private readonly object fileName= "File1API.txt";

        public WriteToFileHostedService(IHostEnvironment environment)
        {
            this.environment= environment;
        }
        public Task StartAsync(CancellationToken cancellationToken)//Se ejecuta cuando se monte la aplicacion
        {
            WriteToFile("WriteToFileHostedService: Proceso en marcha");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)//Se ejecuta cuando se desmonte la aplicacion
        {
            WriteToFile("WriteToFileHostedService: Process Stopped");
            return Task.CompletedTask;
        }

        private void WriteToFile(string message)
        {
            var path = $@"{environment.ContentRootPath}\wwwroot\{fileName}";//Se obtiene la direccion de donde se esta ejecutando la aplicacion, donde eesta el archivo donde vamos a escribir
            using (StreamWriter writer = new StreamWriter(path, append: true))//Con StreamWriter se escribe en el fichero
            {
                writer.WriteLine(message);
            }
        }
    }
}