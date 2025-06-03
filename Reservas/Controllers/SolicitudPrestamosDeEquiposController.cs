using ERP.Data.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOSolicitudDeEquipos;

namespace Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudPrestamosDeEquiposController : ControllerBase
    {
        private readonly IServicioSolicitudPrestamosDeEquipos _servicioSolicitudPrestamosDeEquipos;
        private readonly DbErpContext _dbErpContext;

        public SolicitudPrestamosDeEquiposController(IServicioSolicitudPrestamosDeEquipos servicioSolicitudPrestamosDeEquipos, DbErpContext dbErpContext)
        {
            _servicioSolicitudPrestamosDeEquipos = servicioSolicitudPrestamosDeEquipos;
            _dbErpContext = dbErpContext;
        }

        [HttpGet]
        public async Task<IActionResult?> GetSolicitudPrestamos([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20)
        {
            var prestamo = await _servicioSolicitudPrestamosDeEquipos.GetSolicitudPrestamos(pagina, tamanoPagina);
            if (prestamo == null)
            {
                return NotFound("Su solicitud no se encuentra");
            }
           

            var totalInventario = await _dbErpContext.SolicitudPrestamosDeEquipos.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalInventario / (double)tamanoPagina);

            var respuesta = new
            {
                paginacion = new
                {
                    paginaActual = pagina,
                    tamanoPagina,
                    totalInventario,
                    totalPaginas
                },
                datos = prestamo
            };
            return Ok(respuesta);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdSolicitudPEquipos(int id)
        {
            var prestamo = await _servicioSolicitudPrestamosDeEquipos.GetByIdSolicitudPEquipos(id);
            if (prestamo == null)
            {
                return NotFound($"Su solicitud con el ID: {id} no se encuentra");
            }
            return Ok(prestamo);
        }

        [HttpPost]
        public async Task <IActionResult?> CrearSolicitudPEquipo(CrearSolicitudPrestamosDeEquiposDTO crearSolicitudPrestamosDeEquiposDTO)
        {
            var prestamo = await _servicioSolicitudPrestamosDeEquipos.CrearSolicitudPEquipos(crearSolicitudPrestamosDeEquiposDTO);
            if (prestamo == null)
            {
                return NotFound($"Su Solicitud no se pudo crear");
            }
            return Ok(prestamo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult?> ActualizarSolicitudPEquipos(int id,ActualizarSolicitudPrestamosDeEquiposDTO actualizarSolicitudPrestamosDeEquiposDTO)
        {
            var prestamo = await _servicioSolicitudPrestamosDeEquipos.ActualizarSolicitudPEquipos(id,actualizarSolicitudPrestamosDeEquiposDTO);
            if (prestamo == null)
            {
                return NotFound($"Su solicitud con el ID: {id} no se pudo actualizar");
            }
            return Ok(prestamo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarSolicitudReserva(int id)
        {
            var prestamo = await _servicioSolicitudPrestamosDeEquipos.CancelarSolicitudReserva(id);
            if (prestamo == null)
            {
                return NotFound($"Su solicitud con el ID: {id} no se encuantra");
            }
            return Ok(prestamo);
        }

    }
    
}
