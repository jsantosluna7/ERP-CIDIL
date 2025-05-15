using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOEstado;
using Reservas.Implementaciones.Repositorios;
using Reservas.Modelos;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioEstado : IServicioEstado
    {

        private readonly IRepositorioEstado _repositorioEstado;

        public ServicioEstado(IRepositorioEstado repositorioEstado)
        {
            this._repositorioEstado = repositorioEstado;
        }

        public async Task<Estado?> GetById(int id)
        {
            var es = await _repositorioEstado.GetById(id);
            return es = new Estado
            {   
                Id = es.Id,
                Estado1 = es.Estado1,
                PrestamosEquipos = es.PrestamosEquipos,
                ReservaDeEspacios = es.ReservaDeEspacios,
                SolicitudPrestamosDeEquipos = es.SolicitudPrestamosDeEquipos,
                SolicitudReservaDeEspacios = es.SolicitudReservaDeEspacios
            };
        }

        public async Task<List<EstadoDTO>?> GetEstado()
        {
           var estado = await _repositorioEstado.GetEstado();
            if (estado == null)
            {
                return null;
            }
            var estadoDTO = new List<EstadoDTO>();
            foreach(Estado estado1 in estado)
            {
                var nuevoEstado = new EstadoDTO
                {
                    Id = estado1.Id,
                    Estado1 = estado1.Estado1,
                };
                 estadoDTO.Add(nuevoEstado);
            }
                return estadoDTO;
        }
    }
}
