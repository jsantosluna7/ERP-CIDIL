using ERP.Data.Modelos;
using Inventario.Abstraccion.Repositorio;
using Inventario.Abstraccion.Servicios;
using Inventario.DTO.EstadoFisicoDTO;


namespace Inventario.Implementaciones.Servicios
{
    public class ServicioEstadoFisico : IServicioEstadoFisico
    {
        private readonly IRepositorioEstadoFisico _repositorioEstadoFisico;
        public ServicioEstadoFisico(IRepositorioEstadoFisico repositorioEstadoFisico)
        {
            _repositorioEstadoFisico = repositorioEstadoFisico;
        }
        public async Task<EstadoFisico?> GetById(int id)
        {
          var est = await _repositorioEstadoFisico.GetById(id);
            return new EstadoFisico
            {
                Id =est.Id,
                EstadoFisico1=est.EstadoFisico1,
                InventarioEquipos=est.InventarioEquipos,
            };
          
        }

        public async Task<List<EstadoFisicoDTO>?> GetEstadoFisico()
        {
            var eF = await _repositorioEstadoFisico.GetEstadoFisico();
            if (eF == null)
            {
                return null;
            }
            var efDTO = new List<EstadoFisicoDTO>();
            foreach(EstadoFisico estadoFisico in eF)
            {
                var nuevoEF = new EstadoFisicoDTO
                {
                    Id = estadoFisico.Id,
                    EstadoFisico1 =estadoFisico.EstadoFisico1,
                };
                efDTO.Add(nuevoEF);

            }
            return efDTO;
        }
    }
}
