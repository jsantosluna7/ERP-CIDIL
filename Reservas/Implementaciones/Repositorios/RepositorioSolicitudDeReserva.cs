using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Reservas.Abstraccion.Repositorio;
using Reservas.DTO.DTOSolicitudDeReserva;
using Reservas.Implementaciones.Servicios;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioSolicitudDeReserva : IRepositorioSolicitudDeReserva
    {
        private readonly DbErpContext _context;
        private readonly ServicioEmailReservas _servicioEmail;
        private readonly ServicioConflictos _servicioConflictos;
        private readonly ServicioCantidadPersonas _servicioCantidadPersonas;
        public RepositorioSolicitudDeReserva(DbErpContext context, ServicioEmailReservas servicioEmail, ServicioConflictos servicioConflictos, ServicioCantidadPersonas servicioCantidadPersonas)
        {
            _context = context;
            _servicioEmail = servicioEmail;
            _servicioConflictos = servicioConflictos;
            _servicioCantidadPersonas = servicioCantidadPersonas;
        }

        //Método para obtener todas las solicitudes de reserva
        public async Task<List<SolicitudReservaDeEspacio>> ObtenerSolicitudesReservas(int pagina, int tamanoPagina)
        {

            if (pagina <= 0) pagina = 1;
            if (tamanoPagina <= 0) tamanoPagina = 20;

            return await _context.SolicitudReservaDeEspacios
                .OrderBy(i => i.Id).Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();
        }

        //Método para obtener todas las solicitudes de reserva
        public async Task<List<SolicitudReservaDeEspacio>> ObtenerSolicitudesReservasPorPiso(int piso)
        {
            try
            {
                // Obtener IDs de laboratorios que pertenecen al piso
                var idsLaboratorios = await _context.Laboratorios
                    .Where(p => p.Piso == piso)
                    .Select(l => l.Id)
                    .ToListAsync();

                // Obtener todas las solicitudes de esos laboratorios
                var solicitudes = await _context.SolicitudReservaDeEspacios
                    .Where(r => idsLaboratorios.Contains(r.IdLaboratorio))
                    .ToListAsync();

                return solicitudes;
            }
            catch (Exception ex) {
                throw new Exception("Hubo un error al obtener las solicitudes", ex);
            }
        }

        //Método para obtener todas las solicitudes de reserva por id
        public async Task<Resultado<SolicitudReservaDeEspacio?>> ObtenerSolicitudReservaPorId(int id)
        {
            var solicitudReserva = await _context.SolicitudReservaDeEspacios.FirstOrDefaultAsync(r => r.Id == id);
            if (solicitudReserva == null)
            {
                return Resultado<SolicitudReservaDeEspacio?>.Falla("No existe una solicitud de reserva con ese id.");
            }
            return Resultado<SolicitudReservaDeEspacio?>.Exito(solicitudReserva);
        }

        //Método para que el usuario solicite una reserva
        public async Task<Resultado<SolicitudReservaDeEspacio?>> SolicitarCrearReserva(CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO)
        {
            //verificar si la reserva ya existe
            bool conflictoCompleto = await _servicioConflictos.conflictoReserva(crearSolicitudDeReservaDTO.IdLaboratorio, crearSolicitudDeReservaDTO.HoraInicio, crearSolicitudDeReservaDTO.HoraFinal, crearSolicitudDeReservaDTO.FechaSolicitud);
            Resultado<bool> cantidadPersonas = await _servicioCantidadPersonas.VerificarCantidad(crearSolicitudDeReservaDTO.IdLaboratorio, crearSolicitudDeReservaDTO.PersonasCantidad);

            if (!cantidadPersonas.esExitoso)
            {
                return Resultado<SolicitudReservaDeEspacio?>.Falla(cantidadPersonas.MensajeError ?? "La cantidad de personas digitada no se permite.");
            }

            if (!conflictoCompleto)
            {
                return Resultado<SolicitudReservaDeEspacio?>.Falla("Ya existe una reserva de espacio en el tiempo y laboratorio establecido.");
            }

            //Crear la reserva
            var solicitudReservaCrear = new SolicitudReservaDeEspacio
            {
                IdUsuario = crearSolicitudDeReservaDTO.IdUsuario,
                IdLaboratorio = crearSolicitudDeReservaDTO.IdLaboratorio,
                HoraInicio = crearSolicitudDeReservaDTO.HoraInicio,
                HoraFinal = crearSolicitudDeReservaDTO.HoraFinal,
                FechaInicio = crearSolicitudDeReservaDTO.FechaInicio,
                FechaFinal = crearSolicitudDeReservaDTO.FechaFinal,
                Motivo = crearSolicitudDeReservaDTO.Motivo,
                FechaSolicitud = DateTime.UtcNow,
                PersonasCantidad = crearSolicitudDeReservaDTO.PersonasCantidad
            };

            var roles = new int?[] { 1, 2 }; //Roles de administrador y superusuario.

            var usuario = await _context.Usuarios.Where(u => roles.Contains(u.IdRol)).ToListAsync();

            foreach (var usuarios in usuario)
            {
                await _servicioEmail.EnviarCorreoReserva(usuarios.CorreoInstitucional); //Agregar la url que porque el usuario aprobador podra acceder al id de la solicitud
            }

            _context.SolicitudReservaDeEspacios.Add(solicitudReservaCrear);
            await _context.SaveChangesAsync();

            return Resultado<SolicitudReservaDeEspacio?>.Exito(solicitudReservaCrear);
        }

        //Método para que el usuario cancele una solicitud de reserva
        public async Task<Resultado<bool?>> CancelarSolicitudReserva(int id)
        {
            var solicitudReserva = await ObtenerSolicitudReservaPorId(id);

            if (solicitudReserva == null)
            {
                return Resultado<bool?>.Falla(solicitudReserva.MensajeError ?? "No exite una solicitud de reserva con ese id.");
            }

            var valor = solicitudReserva.Valor;

            _context.SolicitudReservaDeEspacios.Remove(valor);

            await _context.SaveChangesAsync();

            return Resultado<bool?>.Exito(true);
        }

        //Método para que el usuario edite una solicitud de reserva
        public async Task<Resultado<SolicitudReservaDeEspacio?>> EditarSolicitudReserva(int id, ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO)
        {
            var solicitudReservaPorId = await ObtenerSolicitudReservaPorId(id);
            var solicitudReserva = solicitudReservaPorId.Valor;
            Resultado<bool> cantidadPersonas = await _servicioCantidadPersonas.VerificarCantidad(actualizarSolicitudDeReservaDTO.IdLaboratorio ?? 1, actualizarSolicitudDeReservaDTO.PersonasCantidad);

            if (!cantidadPersonas.esExitoso)
            {
                return Resultado<SolicitudReservaDeEspacio?>.Falla(cantidadPersonas.MensajeError ?? "La cantidad de personas digitada no se permite.");
            }

            if (solicitudReserva == null)
            {
                return Resultado<SolicitudReservaDeEspacio?>.Falla("No se encuentra la solicitud de espacio.");
            }

            solicitudReserva.IdLaboratorio = actualizarSolicitudDeReservaDTO.IdLaboratorio ?? solicitudReserva.IdLaboratorio;
            solicitudReserva.HoraInicio = actualizarSolicitudDeReservaDTO.HoraInicio;
            solicitudReserva.HoraFinal = actualizarSolicitudDeReservaDTO.HoraFinal;
            solicitudReserva.FechaInicio = actualizarSolicitudDeReservaDTO.FechaInicio;
            solicitudReserva.FechaFinal = actualizarSolicitudDeReservaDTO.FechaFinal;
            solicitudReserva.Motivo = actualizarSolicitudDeReservaDTO.Motivo ?? solicitudReserva.Motivo;
            solicitudReserva.FechaSolicitud = DateTime.UtcNow;
            solicitudReserva.IdEstado = actualizarSolicitudDeReservaDTO.IdEstado; // Asignar el estado de "Pendiente" (2) al editar la solicitud
            solicitudReserva.PersonasCantidad = actualizarSolicitudDeReservaDTO.PersonasCantidad;

            //verificar si la reserva ya existe
            bool conflicto = await _servicioConflictos.conflictoReservaActualizar(solicitudReserva.Id, solicitudReserva.IdLaboratorio, solicitudReserva.HoraInicio, solicitudReserva.HoraFinal, solicitudReserva.FechaSolicitud);

            if (!conflicto)
            {
                return Resultado<SolicitudReservaDeEspacio?>.Falla("Ya existe una reserva de espacio en el tiempo y laboratorio establecido."); //si la reserva ya existe retorna null
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
            var solicitudActualizadaId = await ObtenerSolicitudReservaPorId(id);
            var solicitudActualizada = solicitudActualizadaId.Valor;

            return Resultado<SolicitudReservaDeEspacio?>.Exito(solicitudActualizada);
        }
    }
}
