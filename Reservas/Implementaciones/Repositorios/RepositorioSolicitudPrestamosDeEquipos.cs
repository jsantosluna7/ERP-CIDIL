using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;
using Reservas.DTO.DTOPrestamosEquipo;
using Reservas.DTO.DTOSolicitudDeEquipos;
using Reservas.Implementaciones.Servicios;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioSolicitudPrestamosDeEquipos : IRepositorioSolicitudPrestamosDeEquipos
    {
        private readonly DbErpContext _context;
        private readonly ServicioEmailReservas _servicioEmail;

        public RepositorioSolicitudPrestamosDeEquipos(DbErpContext context, ServicioEmailReservas servicioEmail)
        {
            _context = context;
            _servicioEmail = servicioEmail;
        }

        //Optener las solicitudes de los pretamos de equipos
        public async Task<List<SolicitudPrestamosDeEquipo>> GetSolicitudPrestamos(int pagina, int tamanoPagina)
        {
            return await _context.SolicitudPrestamosDeEquipos.ToListAsync();

        }

        //Método para obtener todas las Prestamos por id
        public async Task<SolicitudPrestamosDeEquipo> GetByIdSolicitudPEquipos(int id)
        {
            var reserva = await _context.SolicitudPrestamosDeEquipos.FirstOrDefaultAsync(s => s.Id == id);
            if (reserva == null)
            {
                return null;
            }
            return reserva;
        }

        public async Task<Resultado<List<SolicitudPrestamosDeEquipo>>> ObtenerSolicitudEquiposUsuario(int id)
        {
            var reserva = await _context.SolicitudPrestamosDeEquipos.Where(e => e.IdUsuario == id).ToListAsync();
            if (reserva == null || reserva.Count == 0)
            {
                return Resultado<List<SolicitudPrestamosDeEquipo>>.Falla("No se encontraron solicitudes de equipos para el usuario especificado.");
            }

            return Resultado<List<SolicitudPrestamosDeEquipo>>.Exito(reserva);
        }

        public async Task<SolicitudPrestamosDeEquipo?> CrearSolicitudPEquipos(CrearSolicitudPrestamosDeEquiposDTO crearSolicitudPrestamosDeEquiposDTO)
        {
            bool conflitoDeSolicitud = await conflictoPrestamos(crearSolicitudPrestamosDeEquiposDTO.IdUsuario, crearSolicitudPrestamosDeEquiposDTO.IdInventario, crearSolicitudPrestamosDeEquiposDTO.FechaInicio, crearSolicitudPrestamosDeEquiposDTO.FechaFinal, crearSolicitudPrestamosDeEquiposDTO.FechaSolicitud);
            if (!conflitoDeSolicitud)
            {
                return null;
            }

            var crearReservas = new SolicitudPrestamosDeEquipo
            {
                IdUsuario = crearSolicitudPrestamosDeEquiposDTO.IdUsuario,
                IdInventario = crearSolicitudPrestamosDeEquiposDTO.IdInventario,
                FechaInicio = crearSolicitudPrestamosDeEquiposDTO.FechaInicio,
                FechaFinal = crearSolicitudPrestamosDeEquiposDTO.FechaFinal,
                Motivo = crearSolicitudPrestamosDeEquiposDTO.Motivo,
                FechaSolicitud = crearSolicitudPrestamosDeEquiposDTO.FechaSolicitud,
                Cantidad = crearSolicitudPrestamosDeEquiposDTO.Cantidad,
            };

            //Convertirmos la fecha UTC a OFFSET
            string fechaInicio = crearReservas.FechaInicio.ToString();
            string fechaFinal = crearReservas.FechaFinal.ToString();

            //Se parcea la fecha para que incluya la zona  horaria
            DateTimeOffset dtoInicio = DateTimeOffset.Parse(fechaInicio);
            DateTimeOffset dtoFinal = DateTimeOffset.Parse(fechaFinal);

            //Ahora la hora local
            DateTime fechaLocalInicio = dtoInicio.LocalDateTime;
            DateTime fechaLocalFinal = dtoFinal.LocalDateTime;

            //Formateamos personalizadamente

            string fechaFormateadaInicio = fechaLocalInicio.ToString("dd/MM/yyyy h:mm tt");
            string fechaFormateadaFinal = fechaLocalFinal.ToString("dd/MM/yyyy h:mm tt");

            var roles = new int?[] { 1, 2 }; //Roles de administrador y superusuario.

            var usuario = await _context.Usuarios.Where(u => roles.Contains(u.IdRol)).ToListAsync();
            var inventario = await _context.InventarioEquipos.Where(i => i.Id == crearReservas.IdInventario).FirstOrDefaultAsync();


            foreach (var usuarios in usuario)
            {
                await _servicioEmail.EnviarCorreoReservaEquipos(usuarios.CorreoInstitucional, inventario.Nombre, crearReservas.Cantidad.ToString(),fechaFormateadaInicio, fechaFormateadaFinal); //Agregar la url que porque el usuario aprobador podra acceder al id de la solicitud
            }

            _context.SolicitudPrestamosDeEquipos.Add(crearReservas);
            await _context.SaveChangesAsync();
            return crearReservas;
        }

        //Método para que el usuario Actualize una solicitud de Prestamos

        public async Task<SolicitudPrestamosDeEquipo?> ActualizarSolicitudPEquipos(int id, ActualizarSolicitudPrestamosDeEquiposDTO actualizarSolicitudPrestamosDeEquiposDTO)
        {
            var prestamoEquipo = await GetByIdSolicitudPEquipos(id);
            if (prestamoEquipo == null)
            {
                return null;
            }

            prestamoEquipo.IdUsuario = actualizarSolicitudPrestamosDeEquiposDTO.IdUsuario;
            prestamoEquipo.IdInventario = actualizarSolicitudPrestamosDeEquiposDTO.IdInventario;
            prestamoEquipo.FechaInicio = actualizarSolicitudPrestamosDeEquiposDTO.FechaInicio;
            prestamoEquipo.FechaFinal = actualizarSolicitudPrestamosDeEquiposDTO.FechaFinal;
            prestamoEquipo.Motivo = actualizarSolicitudPrestamosDeEquiposDTO.Motivo;
            prestamoEquipo.FechaSolicitud = actualizarSolicitudPrestamosDeEquiposDTO.FechaSolicitud;
            prestamoEquipo.Cantidad = actualizarSolicitudPrestamosDeEquiposDTO.Cantidad;
            prestamoEquipo.IdEstado = actualizarSolicitudPrestamosDeEquiposDTO.idEstado;




            //verificar si la reserva ya existe
            bool conflicto = await conflictoReservaActualizar(prestamoEquipo.IdUsuario, prestamoEquipo.IdInventario, prestamoEquipo.FechaInicio, prestamoEquipo.FechaFinal, prestamoEquipo.FechaSolicitud);

            if (!conflicto)
            {
                return null; //si la reserva ya existe retorna null
            }

            var usuario = await _context.Usuarios.Where(u => u.IdRol == 2).ToListAsync();

            var usuarioSolicitante = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == prestamoEquipo.IdUsuario);

            foreach (var usuarios in usuario)
            {
                await _servicioEmail.EnviarCorreoReservaMoficación(usuarios.CorreoInstitucional, usuarioSolicitante.NombreUsuario, usuarioSolicitante.ApellidoUsuario);
            }

            //Actualizar la reserva
            _context.Update(prestamoEquipo);
            await _context.SaveChangesAsync();

            // Obtener la solicitud de reserva actualizada
            var solicitudActualizada = await GetByIdSolicitudPEquipos(id);
            return solicitudActualizada;


        }

        //Método para que el usuario cancele una solicitud de reserva
        public async Task<bool?> CancelarSolicitudReserva(int id)
        {
            var solicitudEquipo = await GetByIdSolicitudPEquipos(id);
            if (solicitudEquipo == null)
            {
                return null;
            }
            _context.SolicitudPrestamosDeEquipos.Remove(solicitudEquipo);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> conflictoPrestamos(int IdUsuario, int IdInventario, DateTime? FechaInicio, DateTime? FechaFinal, DateTime? FechaSolicitud)
        {
            var diaSemana = FechaSolicitud?.DayOfWeek.ToString();
            var dia = diaSemana switch
            {
                "Monday" => "Lunes",
                "Tuesday" => "Martes",
                "Wednesday" => "Miércoles",
                "Thursday" => "Jueves",
                "Friday" => "Viernes",
                "Saturday" => "Sábado",
                "Sunday" => "Domingo",
                _ => throw new ArgumentOutOfRangeException()
            };


            bool conflictoEquipo = await _context.PrestamosEquipos
                .AnyAsync(h => h.IdInventario == IdInventario &&
                               h.FechaInicio == FechaInicio &&
                               h.FechaInicio < FechaFinal &&
                               h.FechaFinal > FechaInicio);



            return !(conflictoEquipo); // Si no hay conflicto, se puede crear la reserva
        }

        public async Task<bool> conflictoReservaActualizar(int IdUsuario, int IdInventario, DateTime? FechaInicio, DateTime? FechaFinal, DateTime? FechaSolicitud)
        {
            var diaSemana = FechaSolicitud?.DayOfWeek.ToString();
            var dia = diaSemana switch
            {
                "Monday" => "Lunes",
                "Tuesday" => "Martes",
                "Wednesday" => "Miércoles",
                "Thursday" => "Jueves",
                "Friday" => "Viernes",
                "Saturday" => "Sábado",
                "Sunday" => "Domingo",
                _ => throw new ArgumentOutOfRangeException()
            };


            bool conflictoHorario = await _context.PrestamosEquipos
                .Where(h => h.IdInventario != IdInventario) // Excluir la reserva actual
                .AnyAsync(h => h.IdInventario == IdInventario &&
                               h.FechaInicio == FechaInicio &&
                               h.FechaInicio < FechaFinal &&
                               h.FechaFinal > FechaInicio);



            return !(conflictoHorario); // Si no hay conflicto, se puede crear la reserva
        }
    }
}
