using Microsoft.AspNetCore.Mvc;
using Usuarios.Abstraccion.Servicios;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiasController : ControllerBase
    {
        private readonly IServicioNoticias _servicioNoticias;
        public NoticiasController(IServicioNoticias servicioNoticias)
        {
            _servicioNoticias = servicioNoticias;
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerNoticias()
        {
            var noticias = await _servicioNoticias.ObtenerNoticias();
            if (noticias == null || noticias.Count == 0)
            {
                return NotFound("No se encontraron noticias.");
            }
            return Ok(noticias);
        }
    }
}
