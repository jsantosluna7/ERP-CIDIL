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
                    IdEstado = solicitud.IdEstado
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
                    IdEstado = solicitud.IdEstado
                };
                // Agregar la solicitudDTO a la lista de solicitudesDTO
                solicitudesDTO.Add(solicitudDTO);
            }

            return solicitudesDTO;
        }

        // Método para obtener una solicitud de reserva por id
        public async Task<SolicitudDeReservaDTO?> ObtenerSolicitudReservaPorId(int id)
        {
            var solicitud = await _repositorioSolicitudDeReserva.ObtenerSolicitudReservaPorId(id);
            if (solicitud == null)
            {
                return null;
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
                IdEstado = solicitud.IdEstado
            };
            return solicitudDTO;
        }

        // Método para solicitar una reserva
        public async Task<CrearSolicitudDeReservaDTO?> SolicitarCrearReserva(CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO)
        {
            var solicitud = await _repositorioSolicitudDeReserva.SolicitarCrearReserva(crearSolicitudDeReservaDTO);
            if (solicitud == null)
            {
                return null;
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
                FechaSolicitud = solicitud.FechaSolicitud
            };
            return solicitudDTO;
        }

        // Método para editar una solicitud de reserva
        public async Task<ActualizarSolicitudDeReservaDTO?> EditarSolicitudReserva(int id, ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO)
        {
            var solicitud = await _repositorioSolicitudDeReserva.EditarSolicitudReserva(id, actualizarSolicitudDeReservaDTO);
            if (solicitud == null)
            {
                return null;
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
                IdEstado = solicitud.IdEstado
            };
            return solicitudDTO;
        }

        // Método para cancelar una solicitud de reserva
        public async Task<bool?> CancelarSolicitudReserva(int id)
        {
            var resultado = await _repositorioSolicitudDeReserva.CancelarSolicitudReserva(id);
            if (resultado == null)
            {
                return null;
            }
            return resultado;
        }
    }
}
