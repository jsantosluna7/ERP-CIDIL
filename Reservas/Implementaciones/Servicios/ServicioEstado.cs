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

        public Estado GetById(int id)
        {
            return _repositorioEstado.GetById(id);
        }

        public List<EstadoDTO> GetEstado()
        {
           var estado = _repositorioEstado.GetEstado();
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
