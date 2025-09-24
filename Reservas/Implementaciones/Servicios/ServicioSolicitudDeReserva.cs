using ERP.Data.Modelos;
using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOSolicitudDeReserva;
using Reservas.Implementaciones.Repositorios;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioSolicitudDeReserva : IServicioSolicitudDeReserva
    {
        private readonly IRepositorioSolicitudDeReserva _repositorioSolicitudDeReserva;
        public ServicioSolicitudDeReserva(IRepositorioSolicitudDeReserva repositorioSolicitudDeReserva)
        {
            _repositorioSolicitudDeReserva = repositorioSolicitudDeReserva;
        }

        // Método para obtener las solicitudes de reserva
        public async Task<List<SolicitudDeReservaDTO>?> ObtenerSolicitudesReservas(int pagina, int tamanoPagina)
        {
            var solicitudes = await _repositorioSolicitudDeReserva.ObtenerSolicitudesReservas(pagina, tamanoPagina);

            if (solicitudes == null || solicitudes.Count == 0)
            {
                return null;
            }

            var solicitudesDTO = new List<SolicitudDeReservaDTO>();

            // Recorrer la lista de solicitudes y convertir cada una a SolicitudDeReservaDTO
            foreach (var solicitud in solicitudes)
            {
                var solicitudDTO = new SolicitudDeReservaDTO()
                {
                    Id = solicitud.Id,
                    IdUsuario = solicitud.IdUsuario,
                    IdLaboratorio = solicitud.IdLaboratorio,
                    HoraInicio = solicitud.HoraInicio,
                    HoraFinal = solicitud.HoraFinal,
                    FechaInicio = solicitud.FechaInicio,
                    FechaFinal = solicitud.FechaFinal,
                    Motivo = solicitud.Motivo,
                    FechaSolicitud = solicitud.FechaSolicitud,
                    IdEstado = solicitud.IdEstado,
                    PersonasCantidad = solicitud.PersonasCantidad
                };
                // Agregar la solicitudDTO a la lista de solicitudesDTO
                solicitudesDTO.Add(solicitudDTO);
            }

            return solicitudesDTO;
        }

        //Método para obtener todas las solicitudes de reserva por piso
        public async Task<List<SolicitudDeReservaDTO>?> ObtenerSolicitudesReservasPorPiso(int piso)
        {
            var solicitudes = await _repositorioSolicitudDeReserva.ObtenerSolicitudesReservasPorPiso(piso);

            if (solicitudes == null || solicitudes.Count == 0)
            {
                return null;
            }

            var solicitudesDTO = new List<SolicitudDeReservaDTO>();

            // Recorrer la lista de solicitudes y convertir cada una a SolicitudDeReservaDTO
            foreach (var solicitud in solicitudes)
            {
                var solicitudDTO = new SolicitudDeReservaDTO()
                {
                    Id = solicitud.Id,
                    IdUsuario = solicitud.IdUsuario,
                    IdLaboratorio = solicitud.IdLaboratorio,
                    HoraInicio = solicitud.HoraInicio,
                    HoraFinal = solicitud.HoraFinal,
                    FechaInicio = solicitud.FechaInicio,
                    FechaFinal = solicitud.FechaFinal,
                    Motivo = solicitud.Motivo,
                    FechaSolicitud = solicitud.FechaSolicitud,
                    IdEstado = solicitud.IdEstado,
                    PersonasCantidad = solicitud.PersonasCantidad
                };
                // Agregar la solicitudDTO a la lista de solicitudesDTO
                solicitudesDTO.Add(solicitudDTO);
            }

            return solicitudesDTO;
        }

        // Método para obtener una solicitud de reserva por id
        public async Task<Resultado<SolicitudDeReservaDTO?>> ObtenerSolicitudReservaPorId(int id)
        {
            var solicitudPorId = await _repositorioSolicitudDeReserva.ObtenerSolicitudReservaPorId(id);
            var solicitud = solicitudPorId.Valor;

            if (!solicitudPorId.esExitoso)
            {
                return Resultado<SolicitudDeReservaDTO?>.Falla(solicitudPorId.MensajeError ?? "No se obtuvieron las solicitudes.");
            }
            var solicitudDTO = new SolicitudDeReservaDTO()
            {
                Id = solicitud.Id,
                IdUsuario = solicitud.IdUsuario,
                IdLaboratorio = solicitud.IdLaboratorio,
                HoraInicio = solicitud.HoraInicio,
                HoraFinal = solicitud.HoraFinal,
                FechaInicio = solicitud.FechaInicio,
                FechaFinal = solicitud.FechaFinal,
                Motivo = solicitud.Motivo,
                FechaSolicitud = solicitud.FechaSolicitud,
                IdEstado = solicitud.IdEstado,
                PersonasCantidad = solicitud.PersonasCantidad
            };
            return Resultado<SolicitudDeReservaDTO?>.Exito(solicitudDTO);
        }

        // Método para solicitar una reserva
        public async Task<Resultado<CrearSolicitudDeReservaDTO?>> SolicitarCrearReserva(CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO)
        {
            var solicitudPorId = await _repositorioSolicitudDeReserva.SolicitarCrearReserva(crearSolicitudDeReservaDTO);
            var solicitud = solicitudPorId.Valor;

            if (!solicitudPorId.esExitoso)
            {
                return Resultado<CrearSolicitudDeReservaDTO?>.Falla(solicitudPorId.MensajeError ?? "No se pudo crear la solicitud de espacio.");
            }

            var solicitudDTO = new CrearSolicitudDeReservaDTO()
            {
                IdUsuario = solicitud.IdUsuario,
                IdLaboratorio = solicitud.IdLaboratorio,
                HoraInicio = solicitud.HoraInicio,
                HoraFinal = solicitud.HoraFinal,
                FechaInicio = solicitud.FechaInicio,
                FechaFinal = solicitud.FechaFinal,
                Motivo = solicitud.Motivo,
                FechaSolicitud = solicitud.FechaSolicitud,
                PersonasCantidad = solicitud.PersonasCantidad
            };
            return Resultado<CrearSolicitudDeReservaDTO?>.Exito(solicitudDTO);
        }

        // Método para editar una solicitud de reserva
        public async Task<Resultado<ActualizarSolicitudDeReservaDTO?>> EditarSolicitudReserva(int id, ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO)
        {
            var solicitudPorId = await _repositorioSolicitudDeReserva.EditarSolicitudReserva(id, actualizarSolicitudDeReservaDTO);
            var solicitud = solicitudPorId.Valor;

            if (!solicitudPorId.esExitoso)
            {
                return Resultado<ActualizarSolicitudDeReservaDTO?>.Falla(solicitudPorId.MensajeError ?? "No se pudo editar la solicitud de espacio.");
            }

            var solicitudDTO = new ActualizarSolicitudDeReservaDTO()
            {
                IdLaboratorio = solicitud.IdLaboratorio,
                HoraInicio = solicitud.HoraInicio,
                HoraFinal = solicitud.HoraFinal,
                FechaInicio = solicitud.FechaInicio,
                FechaFinal = solicitud.FechaFinal,
                Motivo = solicitud.Motivo,
                FechaSolicitud = solicitud.FechaSolicitud,
                IdEstado = solicitud.IdEstado,
                PersonasCantidad = solicitud.PersonasCantidad
            };

            return Resultado<ActualizarSolicitudDeReservaDTO?>.Exito(solicitudDTO);
        }

        // Método para cancelar una solicitud de reserva
        public async Task<Resultado<bool?>> CancelarSolicitudReserva(int id)
        {
            var resultadoPorId = await _repositorioSolicitudDeReserva.CancelarSolicitudReserva(id);
            var resultado = resultadoPorId.Valor;

            if (!resultadoPorId.esExitoso)
            {
                return Resultado<bool?>.Falla(resultadoPorId.MensajeError ?? "No se pudo cancelar la solicitud de espacio.");

            }
            return Resultado<bool?>.Exito(resultado);
        }
    }
}
