using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOSolicitudDeEquipos;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioSolicitudPrestamosDeEquipos : IServicioSolicitudPrestamosDeEquipos
    {
        private readonly IRepositorioSolicitudPrestamosDeEquipos _repositorioSolicitudPrestamosDeEquipos;

        public ServicioSolicitudPrestamosDeEquipos(IRepositorioSolicitudPrestamosDeEquipos repositorioSolicitudPrestamosDeEquipos)
        {
            _repositorioSolicitudPrestamosDeEquipos = repositorioSolicitudPrestamosDeEquipos;
        }

        public async Task<List<SolicitudPrestamosDeEquiposDTO>?> GetSolicitudPrestamos(int pagina, int tamanoPagina)
        {
            var prestamo = await _repositorioSolicitudPrestamosDeEquipos.GetSolicitudPrestamos(pagina, tamanoPagina);
            if (prestamo == null)
            {
                return null;
            }

            // Recorrer la lista de solicitudes y convertir cada una a SolicitudDeReservaDTO
            var prestamosDTO = new List<SolicitudPrestamosDeEquiposDTO>();

            foreach (var prestamos in prestamo)
            {
                var prestamoDTO = new SolicitudPrestamosDeEquiposDTO()
                {
                    Id=prestamos.Id,
                    IdUsuario = prestamos.IdUsuario,
                    IdInventario = prestamos.IdInventario,
                    FechaInicio = prestamos.FechaInicio,
                    FechaFinal = prestamos.FechaFinal,
                    Motivo = prestamos.Motivo,
                    FechaSolicitud = prestamos.FechaSolicitud,
                    
                };
                // Agregar la solicitudDTO a la lista de solicitudesDTO
                prestamosDTO.Add(prestamoDTO);
            }
            return prestamosDTO;
        }

        public async Task<SolicitudPrestamosDeEquiposDTO?> GetByIdSolicitudPEquipos(int id)
        {
            var prestamos = await _repositorioSolicitudPrestamosDeEquipos.GetByIdSolicitudPEquipos(id);
            if (prestamos == null)
            {
                return null;
            }

            var prestamoDTO = new SolicitudPrestamosDeEquiposDTO()
            {
                Id=prestamos.Id,
                IdUsuario = prestamos.IdUsuario,
                IdInventario = prestamos.IdInventario,
                FechaInicio = prestamos.FechaInicio,
                FechaFinal = prestamos.FechaFinal,
                Motivo = prestamos.Motivo,
                FechaSolicitud = prestamos.FechaSolicitud,
            };
            return prestamoDTO;
        }

        public async Task<CrearSolicitudPrestamosDeEquiposDTO?> CrearSolicitudPEquipos(CrearSolicitudPrestamosDeEquiposDTO crearSolicitudPrestamosDeEquiposDTO)
        {
            var prestamos = await _repositorioSolicitudPrestamosDeEquipos.CrearSolicitudPEquipos(crearSolicitudPrestamosDeEquiposDTO);
            if (prestamos == null)
            {
                return null;
            }

            var prestamoDTO = new CrearSolicitudPrestamosDeEquiposDTO()
            {
                IdUsuario = prestamos.IdUsuario,
                IdInventario = prestamos.IdInventario,
                FechaInicio = prestamos.FechaInicio,
                FechaFinal = prestamos.FechaFinal,
                Motivo = prestamos.Motivo,
                FechaSolicitud = prestamos.FechaSolicitud,
            };
            return prestamoDTO;
        }

        public async Task<ActualizarSolicitudPrestamosDeEquiposDTO?> ActualizarSolicitudPEquipos(int id, ActualizarSolicitudPrestamosDeEquiposDTO actualizarSolicitudPrestamosDeEquiposDTO)
        {
            var prestamos = await _repositorioSolicitudPrestamosDeEquipos.ActualizarSolicitudPEquipos(id, actualizarSolicitudPrestamosDeEquiposDTO);
            if (prestamos == null)
            {
                return null;
            }

            var prestamoDTO = new ActualizarSolicitudPrestamosDeEquiposDTO()
            {
                IdUsuario = prestamos.IdUsuario,
                IdInventario = prestamos.IdInventario,
                FechaInicio = prestamos.FechaInicio,
                FechaFinal = prestamos.FechaFinal,
                Motivo = prestamos.Motivo,
                FechaSolicitud = prestamos.FechaSolicitud,
            };
            return prestamoDTO;
        }


        public async Task<bool?> CancelarSolicitudReserva(int id)
        {
            var prestamo = await _repositorioSolicitudPrestamosDeEquipos.CancelarSolicitudReserva(id);
            if (prestamo == null)
            {
                return null;
            }
            return prestamo;
        }
    }
}
