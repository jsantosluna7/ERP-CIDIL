using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Reservas.Abstraccion.Repositorio;
using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.DTO.DTOSolicitudDeReserva;
using Reservas.Implementaciones.Servicios;
using Reservas.Modelos;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioReservaDeEspacio : IRepositorioReservaDeEspacio
    {
        private readonly DbErpContext _context;
        private readonly ServicioEmail _servicioEmail;
        public RepositorioReservaDeEspacio(DbErpContext context, ServicioEmail servicioEmail)
        {
            _context = context;
            _servicioEmail = servicioEmail;
        }

        //Método para obtener todas las reservas
        public async Task<List<ReservaDeEspacio>> ObtenerReservas()
        {
            return await _context.ReservaDeEspacios.ToListAsync();
        }

        //Método para obtener todas las solicitudes de reserva
        public async Task<List<SolicitudReservaDeEspacio>> ObtenerSolicitudesReservas()
        {
            return await _context.SolicitudReservaDeEspacios.ToListAsync();
        }

        //Método para obtener todas las reservas por id
        public async Task<ReservaDeEspacio?> ObtenerReservaPorId(int id)
        {
            var reserva = await _context.ReservaDeEspacios.FirstOrDefaultAsync(r => r.Id == id);
            if (reserva == null)
            {
                return null;
            }

            return reserva;
        }

        //Método para obtener todas las solicitudes de reserva por id
        public async Task<SolicitudReservaDeEspacio?> ObtenerSolicitudReservaPorId(int id)
        {
            var solicitudReserva = await _context.SolicitudReservaDeEspacios.FirstOrDefaultAsync(r => r.Id == id);
            if (solicitudReserva == null)
            {
                return null;
            }
            return solicitudReserva;
        }

        //Método para que el usuario solicite una reserva
        public async Task<SolicitudReservaDeEspacio?> SolicitarCrearReserva(CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO)
        {
            //verificar si la reserva ya existe
            bool conflictoCompleto = await conflictoReserva(crearSolicitudDeReservaDTO.IdLaboratorio, crearSolicitudDeReservaDTO.HoraInicio, crearSolicitudDeReservaDTO.HoraFinal, crearSolicitudDeReservaDTO.FechaSolicitud);

            if (!conflictoCompleto)
            {
                return null;
            }

            //Crear la reserva
            var solicitudReservaCrear = new SolicitudReservaDeEspacio
            {
                IdUsuario = crearSolicitudDeReservaDTO.IdUsuario,
                IdLaboratorio = crearSolicitudDeReservaDTO.IdLaboratorio,
                HoraInicio = crearSolicitudDeReservaDTO.HoraInicio,
                HoraFinal = crearSolicitudDeReservaDTO.HoraFinal,
                Motivo = crearSolicitudDeReservaDTO.Motivo,
                FechaSolicitud = DateTime.UtcNow
            };

            var usuario = await _context.Usuarios.Where(u => u.IdRol == 2).ToListAsync();

            foreach (var usuarios in usuario)
            {
                await _servicioEmail.EnviarCorreoReserva(usuarios.CorreoInstitucional); //Agregar la url que porque el usuario aprobador podra acceder al id de la solicitud
            }

            _context.SolicitudReservaDeEspacios.Add(solicitudReservaCrear);
            await _context.SaveChangesAsync();

            return solicitudReservaCrear;
        }

        //Método para que el usuario apruebe una reserva
        public async Task<ReservaDeEspacio?> CrearReserva(CrearReservaDeEspacioDTO crearReservaDeEspacioDTO)
        {
            //verificar si la reserva ya existe
            var conflicto = await conflictoReserva(crearReservaDeEspacioDTO.IdLaboratorio, crearReservaDeEspacioDTO.HoraInicio, crearReservaDeEspacioDTO.HoraFinal, crearReservaDeEspacioDTO.FechaSolicitud);

            if (!conflicto)
            {
                return null;
            }

            //Crear la reserva
            var crearReserva = new ReservaDeEspacio
            {
                IdUsuario = crearReservaDeEspacioDTO.IdUsuario,
                IdLaboratorio = crearReservaDeEspacioDTO.IdLaboratorio,
                HoraInicio = crearReservaDeEspacioDTO.HoraInicio,
                HoraFinal = crearReservaDeEspacioDTO.HoraFinal,
                IdEstado = crearReservaDeEspacioDTO.IdEstado,
                Motivo = crearReservaDeEspacioDTO.Motivo,
                FechaSolicitud = crearReservaDeEspacioDTO.FechaSolicitud,
                IdUsuarioAprobador = crearReservaDeEspacioDTO.IdUsuarioAprobador,
                FechaAprobacion = DateTime.UtcNow,
                ComentarioAprobacion = crearReservaDeEspacioDTO.ComentarioAprobacion,
            };

            _context.ReservaDeEspacios.Add(crearReserva);
            await _context.SaveChangesAsync();

            return crearReserva;
        }

        //Método para que el usuario cancele una reserva
        public async Task<bool?> CancelarReserva(int id)
        {
            //verificar si hay una reserva con ese id
            var reserva = await ObtenerReservaPorId(id);

            //si la reserva es null, significa que no existe
            if (reserva == null)
            {
                return null;
            }

            //Eliminar la reserva
            _context.ReservaDeEspacios.Remove(reserva);

            //guardar los cambios
            await _context.SaveChangesAsync();

            //si la reserva fue eliminada, retornar true
            return true;
        }

        //Método para que el usuario cancele una solicitud de reserva
        public async Task<bool?> CancelarSolicitudReserva(int id)
        {
            var solicitudReserva = await ObtenerSolicitudReservaPorId(id);
            if (solicitudReserva == null)
            {
                return null;
            }
            _context.SolicitudReservaDeEspacios.Remove(solicitudReserva);
            await _context.SaveChangesAsync();
            return true;
        }

        //Método para que el usuario edite una reserva
        public async Task<ReservaDeEspacio?> EditarReserva(int id, ActualizarReservaDeEspacioDTO actualizarReservaDeEspacioDTO)
        {
            //verificar si hay una reserva con ese id
            var reservaExiste = await ObtenerReservaPorId(id);
            if (reservaExiste == null)
            {
                return null;
            }


            reservaExiste.IdUsuario = actualizarReservaDeEspacioDTO.IdUsuario ?? reservaExiste.IdUsuario;
            reservaExiste.HoraInicio = actualizarReservaDeEspacioDTO.HoraInicio ?? reservaExiste.HoraInicio;
            reservaExiste.HoraFinal = actualizarReservaDeEspacioDTO.HoraFinal ?? reservaExiste.HoraFinal;
            reservaExiste.Motivo = actualizarReservaDeEspacioDTO.Motivo ?? reservaExiste.Motivo;
            reservaExiste.FechaSolicitud = actualizarReservaDeEspacioDTO.FechaSolicitud ?? reservaExiste.FechaSolicitud;
            reservaExiste.IdEstado = actualizarReservaDeEspacioDTO.IdEstado ?? reservaExiste.IdEstado;
            reservaExiste.IdUsuarioAprobador = actualizarReservaDeEspacioDTO.IdUsuarioAprobador ?? reservaExiste.IdUsuarioAprobador;
            reservaExiste.FechaAprobacion = DateTime.UtcNow;
            reservaExiste.ComentarioAprobacion = actualizarReservaDeEspacioDTO.ComentarioAprobacion ?? reservaExiste.ComentarioAprobacion;

            //verificar si la reserva ya existe
            bool conflicto = await conflictoReservaActualizar(reservaExiste.Id, reservaExiste.IdLaboratorio, reservaExiste.HoraInicio, reservaExiste.HoraFinal, reservaExiste.FechaSolicitud);

            if (!conflicto)
            {
                return null; //si la reserva ya existe retorna null
            }

            //Actualizar la reserva
            _context.Update(reservaExiste);
            await _context.SaveChangesAsync();
            var reservaActualizada = await ObtenerReservaPorId(id);

            if (reservaActualizada == null)
            {
                return null;
            }
            return reservaActualizada;

        }

        //Método para que el usuario edite una solicitud de reserva
        public async Task<SolicitudReservaDeEspacio?> EditarSolicitudReserva(int id, ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO)
        {
            var solicitudReserva = await ObtenerSolicitudReservaPorId(id);
            if (solicitudReserva == null)
            {
                return null;
            }

            solicitudReserva.IdLaboratorio = actualizarSolicitudDeReservaDTO.IdLaboratorio ?? solicitudReserva.IdLaboratorio;
            solicitudReserva.HoraInicio = actualizarSolicitudDeReservaDTO.HoraInicio ?? solicitudReserva.HoraInicio;
            solicitudReserva.HoraFinal = actualizarSolicitudDeReservaDTO.HoraFinal ?? solicitudReserva.HoraFinal;
            solicitudReserva.Motivo = actualizarSolicitudDeReservaDTO.Motivo ?? solicitudReserva.Motivo;
            solicitudReserva.FechaSolicitud = DateTime.UtcNow;

            //verificar si la reserva ya existe
            bool conflicto = await conflictoReservaActualizar(solicitudReserva.Id, solicitudReserva.IdLaboratorio, solicitudReserva.HoraInicio, solicitudReserva.HoraFinal, solicitudReserva.FechaSolicitud);

            if (!conflicto)
            {
                return null; //si la reserva ya existe retorna null
            }

            var usuario = await _context.Usuarios.Where(u => u.IdRol == 2).ToListAsync();

            var usuarioSolicitante = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == solicitudReserva.IdUsuario);

            foreach (var usuarios in usuario)
            {
                await _servicioEmail.EnviarCorreoReservaMoficación(usuarios.CorreoInstitucional, usuarioSolicitante.NombreUsuario, usuarioSolicitante.ApellidoUsuario);
            }

            //Actualizar la reserva
            _context.Update(solicitudReserva);
            await _context.SaveChangesAsync();

            // Obtener la solicitud de reserva actualizada
            var solicitudActualizada = await ObtenerSolicitudReservaPorId(id);
            return solicitudActualizada;
        }

        public async Task<bool> conflictoReserva(int IdLaboratorio, TimeOnly? HoraInicio, TimeOnly? HoraFinal, DateTime? FechaSolicitud)
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


            bool conflictoHorario = await _context.Horarios
                .AnyAsync(h => h.IdLaboratorio == IdLaboratorio &&
                               h.Dia == dia &&
                               h.HoraInicio < HoraFinal &&
                               h.HoraFinal > HoraInicio);

            bool conflictoReserva = await _context.ReservaDeEspacios
                .AnyAsync(r => r.IdLaboratorio == IdLaboratorio &&
                               r.FechaSolicitud == FechaSolicitud &&
                               r.HoraInicio < HoraFinal &&
                               r.HoraFinal > HoraInicio);

            return !(conflictoHorario || conflictoReserva); // Si no hay conflicto, se puede crear la reserva
        }

        public async Task<bool> conflictoReservaActualizar(int id, int IdLaboratorio, TimeOnly? HoraInicio, TimeOnly? HoraFinal, DateTime? FechaSolicitud)
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


            bool conflictoHorario = await _context.Horarios
                .Where(h => h.IdLaboratorio != IdLaboratorio) // Excluir la reserva actual
                .AnyAsync(h => h.IdLaboratorio == IdLaboratorio &&
                               h.Dia == dia &&
                               h.HoraInicio < HoraFinal &&
                               h.HoraFinal > HoraInicio);

            bool conflictoReserva = await _context.ReservaDeEspacios
                .Where(r => r.Id != id) // Excluir la reserva actual
                .AnyAsync(r => r.IdLaboratorio == IdLaboratorio &&
                               r.FechaSolicitud == FechaSolicitud &&
                               r.HoraInicio < HoraFinal &&
                               r.HoraFinal > HoraInicio);

            return !(conflictoHorario || conflictoReserva); // Si no hay conflicto, se puede crear la reserva
        }
    }
}
