using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;
using Reservas.DTO.DTOPrestamosEquipo;
using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.Implementaciones.Servicios;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioPrestamosEquipo : IRepositorioPrestamosEquipo
    {


        private readonly DbErpContext _context;
        private readonly ServicioEmailReservas _servicioEmail;

        public RepositorioPrestamosEquipo(DbErpContext context, ServicioEmailReservas servicioEmail)
        {
            _context = context;
            _servicioEmail = servicioEmail;
        }


        

        public async Task<PrestamosEquipo?> Actualizar(int id, ActualizarPrestamosEquipoDTO actualizarPrestamosEquipoDTO)
        {
           var pEquipoExiste = await GetById(id);

            if (pEquipoExiste == null)
            {
                return null;
            }

            pEquipoExiste.IdUsuario = actualizarPrestamosEquipoDTO.IdUsuario;
            pEquipoExiste.IdInventario = actualizarPrestamosEquipoDTO.IdInventario;
            pEquipoExiste.IdEstado = actualizarPrestamosEquipoDTO.IdEstado;
            pEquipoExiste.FechaInicio = actualizarPrestamosEquipoDTO.FechaInicio;
            pEquipoExiste.FechaFinal = actualizarPrestamosEquipoDTO.FechaFinal;
            pEquipoExiste.FechaEntrega = actualizarPrestamosEquipoDTO.FechaEntrega;
            pEquipoExiste.IdUsuarioAprobador = actualizarPrestamosEquipoDTO.IdUsuarioAprobador;
            pEquipoExiste.Motivo = actualizarPrestamosEquipoDTO.Motivo;
            pEquipoExiste.ComentarioAprobacion = actualizarPrestamosEquipoDTO.ComentarioAprobacion;
            pEquipoExiste.Cantidad =actualizarPrestamosEquipoDTO.Cantidad;
            


            _context.Update(pEquipoExiste);
            await _context.SaveChangesAsync();
            var pEquipoActualizado = await GetById(id);
            return pEquipoActualizado;


        }

        public async Task<PrestamosEquipo?> Crear(CrearPrestamosEquipoDTO crearPrestamosEquipoDTO)
        {
            var pEquipo = new PrestamosEquipo
            {
                IdUsuario = crearPrestamosEquipoDTO.IdUsuario,
                IdInventario = crearPrestamosEquipoDTO.IdInventario,
                IdEstado = crearPrestamosEquipoDTO.IdEstado,
                FechaInicio = crearPrestamosEquipoDTO.FechaInicio,
                FechaFinal = crearPrestamosEquipoDTO.FechaFinal,
                FechaEntrega = crearPrestamosEquipoDTO.FechaEntrega,
                IdUsuarioAprobador = crearPrestamosEquipoDTO.IdUsuarioAprobador,
                Motivo = crearPrestamosEquipoDTO.Motivo,
                ComentarioAprobacion = crearPrestamosEquipoDTO.ComentarioAprobacion,
                Cantidad = crearPrestamosEquipoDTO.Cantidad,
                

            };

            //Convertirmos la fecha UTC a OFFSET
            string fechaInicio = crearPrestamosEquipoDTO.FechaInicio.ToString();
            string fechaEntrega = crearPrestamosEquipoDTO.FechaEntrega.ToString() ?? "" ;
            string fechaFinal = crearPrestamosEquipoDTO.FechaFinal.ToString();

            //Se parcea la fecha para que incluya la zona  horaria
            DateTimeOffset dtoInicio = DateTimeOffset.Parse(fechaInicio);
            DateTimeOffset dtofechaEntrega = DateTimeOffset.Parse(fechaEntrega);
            DateTimeOffset dtofechaFinal = DateTimeOffset.Parse(fechaFinal);

            //Ahora la hora local
            DateTime fechaLocalInicio = dtoInicio.LocalDateTime;
            DateTime fechaLocalfechaEntrega = dtofechaEntrega.LocalDateTime;
            DateTime fechaLocalfechaFinal = dtofechaFinal.LocalDateTime;

            //Formateamos personalizadamente

            string fechaFormateadaInicio = fechaLocalInicio.ToString("dd/MM/yyyy");
            string fechaFormateadafechaEntrega = fechaLocalfechaEntrega.ToString("h:mm tt");
            string fechaFormateadafechaFinal = fechaLocalfechaFinal.ToString("h:mm tt");

            _context.PrestamosEquipos.Add(pEquipo);
            await _context.SaveChangesAsync();

            if (crearPrestamosEquipoDTO.IdEstado == 1) // Si el estado es "Aprobado"
            {
                var usuario = await _context.Usuarios.Where(u => u.Id == crearPrestamosEquipoDTO.IdUsuario).FirstOrDefaultAsync();
                if (usuario != null)
                {
                    await _servicioEmail.EnviarCorreoAprobacionEquipos(usuario.CorreoInstitucional, usuario.NombreUsuario, usuario.ApellidoUsuario, fechaFormateadaInicio, fechaFormateadaHoraInicio, fechaFormateadaHoraFinal);
                }
            }
            else if (crearPrestamosEquipoDTO.IdEstado == 3) // Si el estado es "Rechazado"
            {
                var usuario = await _context.Usuarios.Where(u => u.Id == crearPrestamosEquipoDTO.IdUsuario).FirstOrDefaultAsync();
                if (usuario != null)
                {
                    await _servicioEmail.EnviarCorreoRechazoEquipos(usuario.CorreoInstitucional, usuario.NombreUsuario, usuario.ApellidoUsuario, fechaFormateadaInicio, crearPrestamosEquipoDTO.ComentarioAprobacion, fechaFormateadaHoraInicio, fechaFormateadaHoraFinal);
                }
            }
            return pEquipo;
        }

        public async Task<bool?> Eliminar(int id)
        {
           var prestamos =   await GetById(id);

            if (prestamos == null)
            {
                return null;
            }
            _context.Remove(prestamos);
            await _context.SaveChangesAsync();
            return true;
        }

        //Método para desactivar un equipo
        public async Task<bool?> desactivarPrestamoEquipos(int id)
        {
            // Verificar si el equipo existe
            var equipo = await GetById(id);
            if (equipo == null)
            {
                return null;
            }
            // Desactivar el equipo
            equipo.Activado = false;
            // Guardar los cambios en la base de datos
            _context.Update(equipo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PrestamosEquipo?> GetById(int id)
        {
            return await _context.PrestamosEquipos.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        

        public async Task<List<PrestamosEquipo>?> GetPrestamosEquipo(int pagina, int tamanoPagina)
        {
            if (pagina <= 0) pagina = 1;
            if (tamanoPagina <= 0) tamanoPagina = 20;

            return await _context.PrestamosEquipos
                .Where(p => p.Activado == true)
                .OrderBy(i => i.Id)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();
        }


       
    }
}
