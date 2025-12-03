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

        public async Task<Resultado<OrdenesDTO>> ObtenerPorId(int id)
        {
            var resultado = await _repositorioOrdenes.ObtenerPorId(id);
            var orden = resultado.Valor!;

            if(!resultado.esExitoso)
            {
                return Resultado<OrdenesDTO>.Falla(resultado.MensajeError);
            }

            var ordenDTO = new OrdenesDTO
            {
                Codigo = orden.Codigo,
                Nombre = orden.Nombre,
                Departamento = orden.Departamento,
                UnidadNegocio = orden.UnidadNegocio,
                SolicitadoPor = orden.SolicitadoPor,
                FechaSolicitud = orden.FechaSolicitud,
                FechaSubida = orden.FechaSubida,
                Moneda = orden.Moneda,
                ImporteTotal = orden.ImporteTotal,
                Comentario = orden.Comentario,
                EstadoTimelineId = orden.EstadoTimelineId,
                CreadoPor = orden.CreadoPor,
                ActualizadoEn = orden.ActualizadoEn
            };

            return Resultado<OrdenesDTO>.Exito(ordenDTO);
        }

        public async Task<Resultado<OrdenesDTO>> CrearOrdenes(OrdenesDTO ordene)
        {
            var resultado = await _repositorioOrdenes.CrearOrdenes(ordene);
            var orden = resultado.Valor!;

            if(!resultado.esExitoso)
            {
                return Resultado<OrdenesDTO>.Falla(resultado.MensajeError);
            }

            var ordenDTO = new OrdenesDTO
            {
                Codigo = orden.Codigo,
                Nombre = orden.Nombre,
                Departamento = orden.Departamento,
                UnidadNegocio = orden.UnidadNegocio,
                SolicitadoPor = orden.SolicitadoPor,
                FechaSolicitud = orden.FechaSolicitud,
                FechaSubida = orden.FechaSubida,
                Moneda = orden.Moneda,
                ImporteTotal = orden.ImporteTotal,
                Comentario = orden.Comentario,
                EstadoTimelineId = orden.EstadoTimelineId,
                CreadoPor = orden.CreadoPor,
                ActualizadoEn = orden.ActualizadoEn
            };

            return Resultado<OrdenesDTO>.Exito(ordenDTO);
        }

        public async Task<Resultado<OrdenesDTO>> ActualizarOrdenes(int id, OrdenesDTO ordenesDTO)
        {
            var resultado = await _repositorioOrdenes.ActualizarOrdenes(id, ordenesDTO);
            var orden = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<OrdenesDTO>.Falla(resultado.MensajeError);
            }

            var ordenDTO = new OrdenesDTO
            {
                Codigo = orden.Codigo,
                Nombre = orden.Nombre,
                Departamento = orden.Departamento,
                UnidadNegocio = orden.UnidadNegocio,
                SolicitadoPor = orden.SolicitadoPor,
                FechaSolicitud = orden.FechaSolicitud,
                FechaSubida = orden.FechaSubida,
                Moneda = orden.Moneda,
                ImporteTotal = orden.ImporteTotal,
                Comentario = orden.Comentario,
                EstadoTimelineId = orden.EstadoTimelineId,
                CreadoPor = orden.CreadoPor,
                ActualizadoEn = orden.ActualizadoEn
            };

            return Resultado<OrdenesDTO>.Exito(ordenDTO);
        }

        public async Task<Resultado<bool?>> Eliminar(int id)
        {
            var resultado = await _repositorioOrdenes.Eliminar(id);
            var orden = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<bool?>.Falla(resultado.MensajeError);
            }

            return Resultado<bool?>.Exito(orden);
        }

        // Hacer la parte de desactivar una orden.
    }
}
