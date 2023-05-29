using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Source.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Source : ControllerBase
    {


        private readonly IWebHostEnvironment _environment;

        public Source(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // Obtener la dirección IP del cliente
            var forwardedForHeader = HttpContext.Request.Headers["X-Forwarded-For"];
            var ipAddress = string.IsNullOrEmpty(forwardedForHeader) ? HttpContext.Connection.RemoteIpAddress : IPAddress.Parse(forwardedForHeader);
           // var ipAddress = HttpContext.Connection.RemoteIpAddress;

            // Verificar si la dirección IP es IPv4 o IPv6
            if (ipAddress != null)
            {
                if (ipAddress.IsIPv4MappedToIPv6)
                {
                    ipAddress = ipAddress.MapToIPv4();
                }

                // Obtener la hora actual
                var currentTime = DateTime.Now;

                // Construir la ruta del archivo de texto
                var filePath = Path.Combine(_environment.ContentRootPath, "log.txt");

                // Crear el contenido a escribir en el archivo
                var content = $"{ipAddress} - {currentTime}\n";

                // Escribir el contenido en el archivo
                Console.WriteLine(content);

                System.IO.File.AppendAllText(filePath, content);

                // Resto de la lógica de tu endpoint

                var filePathPage = Path.Combine(_environment.WebRootPath, "htmlpage.html");

                if (System.IO.File.Exists(filePath))
                {
                    return PhysicalFile(filePathPage, "text/html");
                }
            }

            return BadRequest();
        }
    
    }
}