using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace webAPI.Services
{
    public class WriteToFileHostedService : IHostedService, IDisposable//Al inico y al final del ciclo de vida de la API se va realizar una acción, en este caso escribir en un archivo de texto, Se pone IDisposable para limpiar los recursos del timer
    {
        private readonly IHostEnvironment environment;//Se usa IHostEnvironment para obtener el directorio donde se encuentra la aplicación ejecutándose
        private readonly object fileName= "File1API.txt";
        private Timer tiempo;

        public WriteToFileHostedService(IHostEnvironment environment)
        {
            this.environment= environment;
        }
        public Task StartAsync(CancellationToken cancellationToken)//Se ejecuta cuando se monte la aplicación
        {
            WriteToFile("WriteToFileHostedService: Proceso en marcha");
            tiempo = new Timer(Trabajando, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));//Inicializar el Timer, a state se le pasa null en este caso,el tiempo de inicio es 0, y el período 5 segundos, la funcion Trabajando se va ejecutar cada 5 segundos
            return Task.CompletedTask;
        }

        private void Trabajando (object state)
        {
            WriteToFile("WriteToFileHostedService: Trabajando a las "+ DateTime.Now.ToString("dd/MM//yyyy hh:mm:ss"));//Esta función va escribir en in archivo
        }

        public Task StopAsync(CancellationToken cancellationToken)//Se ejecuta cuando se desmonte la aplicación
        {
            WriteToFile("WriteToFileHostedService: Process Stopped");
            tiempo?.Change(Timeout.Infinite, 0);//desactivamos el timer
            return Task.CompletedTask;
        }

        private void WriteToFile(string message)
        {
            var path = $@"{environment.ContentRootPath}\wwwroot\{fileName}";//Se obtiene la dirección de donde se esta ejecutando la aplicación, donde está el archivo donde vamos a escribir
            using (StreamWriter writer = new StreamWriter(path, append: true))//Con StreamWriter se escribe en el fichero
            {
                writer.WriteLine(message);
            }
        }

        public void Dispose()
        {
            tiempo?.Dispose();//Limpiamos los recursos del Timer, libera los recursos usados por el timer, se pone en signo de interrogación para que solo se ejecute si el timer no es nulo
        }
    }
}