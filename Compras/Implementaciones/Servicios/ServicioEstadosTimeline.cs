using Compras.Abstraccion.Repositorios;
using Compras.Abstraccion.Servicios;
using Compras.DTO.EstadosTimelineDTO;
using Compras.DTO.OrdenesDTO;
using Compras.Implementaciones.Repositorios;
using ERP.Data.Modelos;

namespace Compras.Implementaciones.Servicios
{
    public class ServicioEstadosTimeline : IServicioEstadosTimeline
    {
        private readonly IRepositorioEstadosTimeline _repositorioEstadosTimeline;

        public ServicioEstadosTimeline(IRepositorioEstadosTimeline repositorioEstadosTimeline)
        {
            _repositorioEstadosTimeline = repositorioEstadosTimeline;
        }

        public async Task<Resultado<List<EstadosTimeline>>> EstadosTimeline()
        {
            var estadosTodos = await _repositorioEstadosTimeline.EstadosTimeline();
            var estados = estadosTodos.Valor;

            if (estados == null || estados.Count == 0)
            {
                return Resultado<List<EstadosTimeline>>.Falla(estadosTodos.MensajeError);
            }

            var estadosDTO = new List<EstadosTimeline>();

            foreach (EstadosTimeline estado in estados)
            {
                var estadoDTO = new EstadosTimeline
                {
                    Id = estado.Id,
                    Codigo = estado.Codigo,
                    Nombre = estado.Nombre,
                    Color = estado.Color,
                    Icono = estado.Icono,
                    Activo = estado.Activo
                };
                estadosDTO.Add(estadoDTO);
            }
            return Resultado<List<EstadosTimeline>>.Exito(estadosDTO);
        }

        public async Task<Resultado<EstadosTimelineDTO>> EstadosTimelineId(int id)
        {
            var resultado = await _repositorioEstadosTimeline.EstadosTimelineId(id);
            var estado = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<EstadosTimelineDTO>.Falla(resultado.MensajeError);
            }

            var estadoDTO = new EstadosTimelineDTO
            {
                Codigo = estado.Codigo,
                Nombre = estado.Nombre,
                Color = estado.Color,
                Icono = estado.Icono,
                Activo = estado.Activo
            };

            return Resultado<EstadosTimelineDTO>.Exito(estadoDTO);
        }

        public async Task<Resultado<EstadosTimelineDTO>> CrearEstadosTimeline(EstadosTimelineDTO estados)
        {
            var resultado = await _repositorioEstadosTimeline.CrearEstadosTimeline(estados);
            var estado = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<EstadosTimelineDTO>.Falla(resultado.MensajeError);
            }

            var estadoDTO = new EstadosTimelineDTO
            {
                Codigo = estado.Codigo,
                Nombre = estado.Nombre,
                Color = estado.Color,
                Icono = estado.Icono,
                Activo = estado.Activo
            };

            return Resultado<EstadosTimelineDTO>.Exito(estadoDTO);
        }

        public async Task<Resultado<EstadosTimelineDTO>> ActualizarEstadosTimeline(int id, EstadosTimelineDTO estadosDTO)
        {
            var resultado = await _repositorioEstadosTimeline.ActualizarEstadosTimeline(id, estadosDTO);
            var estado = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<EstadosTimelineDTO>.Falla(resultado.MensajeError);
            }

            var estadoDTO = new EstadosTimelineDTO
            {
                Codigo = estado.Codigo,
                Nombre = estado.Nombre,
                Color = estado.Color,
                Icono = estado.Icono,
                Activo = estado.Activo
            };

            return Resultado<EstadosTimelineDTO>.Exito(estadoDTO);
        }

        public async Task<Resultado<bool?>> Eliminar(int id)
        {
            var resultado = await _repositorioEstadosTimeline.Eliminar(id);
            var estado = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<bool?>.Falla(resultado.MensajeError);
            }

            return Resultado<bool?>.Exito(estado);
        }
    }
}
