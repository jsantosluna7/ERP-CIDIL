using Compras.Abstraccion.Repositorios;
using Compras.Abstraccion.Servicios;
using Compras.DTO.OrdenesDTO;
using ERP.Data.Modelos;

namespace Compras.Implementaciones.Servicios
{
    public class ServicioOrdenes : IServicioOrdenes
    {
        private readonly IRepositorioOrdenes _repositorioOrdenes;

        public ServicioOrdenes(IRepositorioOrdenes repositorioOrdenes)
        {
            _repositorioOrdenes = repositorioOrdenes;
        }

        public async Task<Resultado<List<OrdenesDTO>>> OrdenesAll()
        {
            var ordenesTodos = await _repositorioOrdenes.OrdenesAll();
            var ordenes = ordenesTodos.Valor;

            if (ordenes == null || ordenes.Count == 0)
            {
                return Resultado<List<OrdenesDTO>>.Falla(ordenesTodos.MensajeError);
            }

            var ordenesDTO = new List<OrdenesDTO>();

            foreach (Ordene ordene in ordenes)
            {
                var ordeneDTO = new OrdenesDTO
                {
                    Codigo = ordene.Codigo,
                    Nombre = ordene.Nombre,
                    Departamento = ordene.Departamento,
                    UnidadNegocio = ordene.UnidadNegocio,
                    SolicitadoPor = ordene.SolicitadoPor,
                    FechaSolicitud = ordene?.FechaSolicitud,
                    FechaSubida = ordene?.FechaSubida,
                    Moneda = ordene?.Moneda,
                    ImporteTotal = ordene?.ImporteTotal,
                    Comentario = ordene?.Comentario,
                    EstadoTimelineId = ordene?.EstadoTimelineId,
                    CreadoPor = ordene?.CreadoPor,
                    ActualizadoEn = ordene?.ActualizadoEn
                };
                ordenesDTO.Add(ordeneDTO);
            }
            return Resultado<List<OrdenesDTO>>.Exito(ordenesDTO);
        }
    }
}
