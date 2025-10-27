using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Reservas.Abstraccion.Repositorio;
using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.DTO.DTOSolicitudDeReserva;
using Reservas.Implementaciones.Servicios;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioReservaDeEspacio : IRepositorioReservaDeEspacio
    {
        private readonly DbErpContext _context;
        private readonly ServicioEmailReservas _servicioEmail;
        private readonly ServicioConflictos _servicioConflictos;
        private readonly ServicioCantidadPersonas _servicioCantidadPersonas;

        public RepositorioReservaDeEspacio(DbErpContext context, ServicioEmailReservas servicioEmail, ServicioConflictos servicioConflictos, ServicioCantidadPersonas servicioCantidadPersonas)
        {
            _context = context;
            _servicioEmail = servicioEmail;
            _servicioConflictos = servicioConflictos;
            _servicioCantidadPersonas = servicioCantidadPersonas;
        }

        //Método para obtener todas las reservas
        public async Task<List<ReservaDeEspacio>> ObtenerReservas(int pagina, int tamanoPagina)
        {
            if (pagina <= 0) pagina = 1;
            if (tamanoPagina <= 0) tamanoPagina = 20;

            return await _context.ReservaDeEspacios
                .Where(r => r.Activado == true)
                .OrderBy(i => i.Id)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();
        }

        //Método para obtener todas las reservas
        public async Task<List<ReservaDeEspacio>> ObtenerReservasTodo()
        {

            return await _context.ReservaDeEspacios
                .Where(r => r.Activado == true && r.IdEstado == 1)
                .OrderBy(i => i.Id).ToListAsync();
        }

        //Método para obtener todas las reservas por id
        public async Task<Resultado<ReservaDeEspacio?>> ObtenerReservaPorId(int id)
        {
            var reserva = await _context.ReservaDeEspacios.FirstOrDefaultAsync(r => r.Id == id);
            if (reserva == null)
            {
                return Resultado<ReservaDeEspacio?>.Falla("No se encontraron reservas de espacio con ese id.");
            }

            return Resultado<ReservaDeEspacio?>.Exito(reserva);
        }

        //Método para obtener todas las solicitudes de reserva
        public async Task<List<ReservaDeEspacio>> ObtenerReservasDeEspacioPorPiso(int piso)
        {
            try
            {
                // Obtener IDs de laboratorios que pertenecen al piso
                var idsLaboratorios = await _context.Laboratorios
                    .Where(p => p.Piso == piso)
                    .Select(l => l.Id)
                    .ToListAsync();

                // Obtener todas las solicitudes de esos laboratorios
                var reservas = await _context.ReservaDeEspacios
                    .Where(r => idsLaboratorios.Contains(r.IdLaboratorio) && r.IdEstado == 1)
                    .ToListAsync();

                return reservas;
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error al obtener las solicitudes", ex);
            }
        }

        public async Task<Resultado<List<ReservaDeEspacio>>> ObtenerEspaciosUsuario(int id)
        {
            var reserva = await _context.ReservaDeEspacios.Where(e => e.IdUsuario == id).ToListAsync();
            if (reserva == null || reserva.Count == 0)
            {
                return Resultado<List<ReservaDeEspacio>>.Falla("No haz reservado ningún espacio.");
            }

            return Resultado<List<ReservaDeEspacio>>.Exito(reserva);
        }

        //Método para que el usuario apruebe una reserva
        public async Task<Resultado<ReservaDeEspacio?>> CrearReserva(CrearReservaDeEspacioDTO crearReservaDeEspacioDTO)
        {
            //verificar si la reserva ya existe
            var conflicto = await _servicioConflictos.conflictoReserva(crearReservaDeEspacioDTO.IdLaboratorio, crearReservaDeEspacioDTO.HoraInicio, crearReservaDeEspacioDTO.HoraFinal, crearReservaDeEspacioDTO.FechaSolicitud);
            Resultado<bool> cantidadPersonas = await _servicioCantidadPersonas.VerificarCantidad(crearReservaDeEspacioDTO.IdLaboratorio, crearReservaDeEspacioDTO.PersonasCantidad);

            if (!cantidadPersonas.esExitoso)
            {
                return Resultado<ReservaDeEspacio?>.Falla(cantidadPersonas.MensajeError ?? "La cantidad de personas digitada no se permite.");
            }

            if (!conflicto)
            {
                return Resultado<ReservaDeEspacio?>.Falla("Ya existe una reserva de espacio en el tiempo y laboratorio establecido.");
            }

            //Crear la reserva
            var crearReserva = new ReservaDeEspacio
            {
                IdUsuario = crearReservaDeEspacioDTO.IdUsuario,
                IdLaboratorio = crearReservaDeEspacioDTO.IdLaboratorio,
                HoraInicio = crearReservaDeEspacioDTO.HoraInicio,
                HoraFinal = crearReservaDeEspacioDTO.HoraFinal,
                FechaInicio = crearReservaDeEspacioDTO.FechaInicio,
                FechaFinal = crearReservaDeEspacioDTO.FechaFinal,
                IdEstado = crearReservaDeEspacioDTO.IdEstado,
                Motivo = crearReservaDeEspacioDTO.Motivo,
                FechaSolicitud = NormalizarUtc(crearReservaDeEspacioDTO.FechaSolicitud),
                IdUsuarioAprobador = crearReservaDeEspacioDTO.IdUsuarioAprobador,
                FechaAprobacion = NormalizarUtc(DateTime.UtcNow),
                ComentarioAprobacion = crearReservaDeEspacioDTO.ComentarioAprobacion,
                PersonasCantidad = crearReservaDeEspacioDTO.PersonasCantidad,
            };

            //Convertirmos la fecha UTC a OFFSET
            string fechaSolicitud = crearReservaDeEspacioDTO.FechaSolicitud.ToString();
            string fechaInicio = crearReservaDeEspacioDTO.FechaInicio.ToString();
            string horaInicio = crearReservaDeEspacioDTO.HoraInicio.ToString();
            string horaFinal = crearReservaDeEspacioDTO.HoraFinal.ToString();

            //Se parcea la fecha para que incluya la zona  horaria
            DateTimeOffset dtoSolicitud = DateTimeOffset.Parse(fechaSolicitud);
            DateTimeOffset dtoInicio = DateTimeOffset.Parse(fechaInicio);
            DateTimeOffset dtoHoraInicio = DateTimeOffset.Parse(horaInicio);
            DateTimeOffset dtoHoraFinal = DateTimeOffset.Parse(horaFinal);

            //Ahora la hora local
            DateTime fechaLocalSolicitud = dtoSolicitud.LocalDateTime;
            DateTime fechaLocalInicio = dtoInicio.LocalDateTime;
            DateTime fechaLocalHoraInicio = dtoHoraInicio.LocalDateTime;
            DateTime fechaLocalHoraFinal = dtoHoraFinal.LocalDateTime;

            //Formateamos personalizadamente

            string fechaFormateadaSolicitud = fechaLocalSolicitud.ToString("dd/MM/yyyy h:mm tt");
            string fechaFormateadaInicio = fechaLocalInicio.ToString("dd/MM/yyyy");
            string fechaFormateadaHoraInicio = fechaLocalHoraInicio.ToString("h:mm tt");
            string fechaFormateadaHoraFinal = fechaLocalHoraFinal.ToString("h:mm tt");

            _context.ReservaDeEspacios.Add(crearReserva);

            await _context.SaveChangesAsync();

            if (crearReservaDeEspacioDTO.IdEstado == 1) // Si el estado es "Aprobado"
            {
                var usuario = await _context.Usuarios.Where(u => u.Id == crearReservaDeEspacioDTO.IdUsuario).FirstOrDefaultAsync();
                var laboratorio = await _context.Laboratorios.Where(l => l.Id == crearReservaDeEspacioDTO.IdLaboratorio).FirstOrDefaultAsync();
                if (usuario != null || laboratorio != null)
                {
                    await _servicioEmail.EnviarCorreoAprobacion(usuario.CorreoInstitucional, usuario.NombreUsuario, usuario.ApellidoUsuario, laboratorio.Nombre, fechaFormateadaSolicitud, fechaFormateadaInicio, fechaFormateadaHoraInicio,fechaFormateadaHoraFinal);
                }
            } else if (crearReservaDeEspacioDTO.IdEstado == 3) // Si el estado es "Rechazado"
            {
                var usuario = await _context.Usuarios.Where(u => u.Id == crearReservaDeEspacioDTO.IdUsuario).FirstOrDefaultAsync();
                var laboratorio = await _context.Laboratorios.Where(l => l.Id == crearReservaDeEspacioDTO.IdLaboratorio).FirstOrDefaultAsync();
                if (usuario != null || laboratorio != null)
                {
                   await _servicioEmail.EnviarCorreoRechazo(usuario.CorreoInstitucional, usuario.NombreUsuario, usuario.ApellidoUsuario, laboratorio.Nombre, fechaFormateadaSolicitud, fechaFormateadaInicio, crearReservaDeEspacioDTO.ComentarioAprobacion, fechaFormateadaHoraInicio, fechaFormateadaHoraFinal);
                }
            }

            return Resultado<ReservaDeEspacio?>.Exito(crearReserva);
        }

        //Cambiar el tipo de hora
        private DateTime? NormalizarUtc(DateTime? date)
        {
            if (!date.HasValue) return null;
            return DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);
        }

        //Método para que el usuario cancele una reserva
        public async Task<Resultado<bool?>> CancelarReserva(int id)
        {
            //verificar si hay una reserva con ese id
            var reserva = await ObtenerReservaPorId(id);

            //si la reserva es null, significa que no existe
            if (reserva == null)
            {
                return Resultado<bool?>.Falla("No hay reservas de espacio por eliminar");
            }

            var resReserva = reserva.Valor;

            //Eliminar la reserva
            _context.ReservaDeEspacios.Remove(resReserva);

            //guardar los cambios
            await _context.SaveChangesAsync();

            //si la reserva fue eliminada, retornar true
            return Resultado<bool?>.Exito(true);
        }

        //Método para que el usuario edite una reserva
        public async Task<Resultado<ReservaDeEspacio?>> EditarReserva(int id, ActualizarReservaDeEspacioDTO actualizarReservaDeEspacioDTO)
        {
            //verificar si hay una reserva con ese id
            var reservaPorId = await ObtenerReservaPorId(id);
            Resultado<bool> cantidadPersonas = await _servicioCantidadPersonas.VerificarCantidad(actualizarReservaDeEspacioDTO.IdLaboratorio ?? 1, actualizarReservaDeEspacioDTO.PersonasCantidad);

            if (!cantidadPersonas.esExitoso)
            {
                return Resultado<ReservaDeEspacio?>.Falla(cantidadPersonas.MensajeError ?? "La cantidad de personas digitada no se permite.");
            }

            if (reservaPorId == null)
            {
                return Resultado<ReservaDeEspacio?>.Falla("No hay reservas de espacio con ese id.");
            }

            var reservaExiste = reservaPorId.Valor;

            reservaExiste.IdUsuario = actualizarReservaDeEspacioDTO.IdUsuario ?? reservaExiste.IdUsuario;
            reservaExiste.HoraInicio = actualizarReservaDeEspacioDTO.HoraInicio;
            reservaExiste.HoraFinal = actualizarReservaDeEspacioDTO.HoraFinal;
            reservaExiste.Motivo = actualizarReservaDeEspacioDTO.Motivo ?? reservaExiste.Motivo;
            reservaExiste.FechaSolicitud = actualizarReservaDeEspacioDTO.FechaSolicitud ?? reservaExiste.FechaSolicitud;
            reservaExiste.IdEstado = actualizarReservaDeEspacioDTO.IdEstado ?? reservaExiste.IdEstado;
            reservaExiste.IdUsuarioAprobador = actualizarReservaDeEspacioDTO.IdUsuarioAprobador ?? reservaExiste.IdUsuarioAprobador;
            reservaExiste.FechaAprobacion = DateTime.UtcNow;
            reservaExiste.ComentarioAprobacion = actualizarReservaDeEspacioDTO.ComentarioAprobacion ?? reservaExiste.ComentarioAprobacion;
            reservaExiste.PersonasCantidad = actualizarReservaDeEspacioDTO.PersonasCantidad;

            //verificar si la reserva ya existe
            bool conflicto = await _servicioConflictos.conflictoReservaActualizar(reservaExiste.Id, reservaExiste.IdLaboratorio, reservaExiste.HoraInicio, reservaExiste.HoraFinal, reservaExiste.FechaSolicitud);

            if (!conflicto)
            {
                return Resultado<ReservaDeEspacio?>.Falla("Ya existe una reserva de espacio en el tiempo y laboratorio establecido.");
            }

            //Actualizar la reserva
            _context.Update(reservaExiste);
            await _context.SaveChangesAsync();
            var reservaActualizada = await ObtenerReservaPorId(id);

            if (reservaActualizada == null)
            {
                return Resultado<ReservaDeEspacio?>.Falla("No hay reservas de espacio con ese id.");
            }
            return reservaActualizada;

        }

        //Método para desactivar un espacio
        public async Task<Resultado<bool?>> desactivarReservaDeEspacio(int id)
        {
            // Verificar si el espacio existe
            var espacioPorId = await ObtenerReservaPorId(id);
            if (espacioPorId == null)
            {
                return Resultado<bool?>.Falla("No hay reservas de espacio con ese id.");
            }

            var espacio = espacioPorId.Valor;
            // Desactivar el espacio
            espacio.Activado = false;
            // Guardar los cambios en la base de datos
            _context.Update(espacio);
            await _context.SaveChangesAsync();
            return Resultado<bool?>.Exito(true);
        }
    }
}
