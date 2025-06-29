using ERP.Data.Modelos;
using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOEstado;
using Reservas.Implementaciones.Repositorios;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioEstado : IServicioEstado
    {

        private readonly IRepositorioEstado _repositorioEstado;

        public ServicioEstado(IRepositorioEstado repositorioEstado)
        {
            this._repositorioEstado = repositorioEstado;
        }

        public async Task<Resultado<Estado?>> GetById(int id)
        {
            var resultado = await _repositorioEstado.GetById(id);

            if (!resultado.esExitoso)
            {
                return Resultado<Estado?>.Falla(resultado.MensajeError ?? "El estado no existe o no se encontró.");
            }

            var estado = resultado.Valor!;

            var es = new Estado
            {
                Id = estado.Id,
                Estado1 = estado.Estado1,
            };

            return Resultado<Estado?>.Exito(es);
        }

        public async Task<Resultado<List<EstadoDTO>?>> GetEstado()
        {
           var estado = await _repositorioEstado.GetEstado();
            if (!estado.Any())
            {
                return Resultado<List<EstadoDTO>?>.Falla("No se encontraron registros."); // Retorna null si no hay estados
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
                return Resultado<List<EstadoDTO>?>.Exito(estadoDTO);
        }
    }
}
