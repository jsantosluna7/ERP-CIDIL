using Compras.Abstraccion.Repositorios;
using Compras.Abstraccion.Servicios;
using Compras.DTO.ComentariosOrdenDTO;
using Compras.DTO.EstadosTimelineDTO;
using Compras.Implementaciones.Repositorios;
using ERP.Data.Modelos;

namespace Compras.Implementaciones.Servicios
{
    public class ServicioComentariosOrden : IServicioComentariosOrden
    {
        private readonly IRepositorioComentariosOrden _repositorioComentariosOrden;

        public ServicioComentariosOrden(IRepositorioComentariosOrden repositorioComentariosOrden)
        {
            _repositorioComentariosOrden = repositorioComentariosOrden;
        }

        public async Task<Resultado<List<ComentariosOrden>>> ComentariosOrden()
        {
            var comentariosTodos = await _repositorioComentariosOrden.ComentariosOrden();
            var comentarios = comentariosTodos.Valor;

            if (comentarios == null || comentarios.Count == 0)
            {
                return Resultado<List<ComentariosOrden>>.Falla(comentariosTodos.MensajeError);
            }

            var comentariosDTO = new List<ComentariosOrden>();

            foreach (ComentariosOrden comentario in comentarios)
            {
                var comentarioDTO = new ComentariosOrden
                {
                    Id = comentario.Id,
                    OrdenId = comentario.OrdenId,
                    ItemId = comentario.ItemId,
                    UsuarioId = comentario.UsuarioId,
                    Comentario = comentario.Comentario,
                    CreadoEn = comentario.CreadoEn
                };
                comentariosDTO.Add(comentarioDTO);
            }
            return Resultado<List<ComentariosOrden>>.Exito(comentariosDTO);
        }

        public async Task<Resultado<ComentariosOrden>> ComentariosOrdenId(int id)
        {
            var resultado = await _repositorioComentariosOrden.ComentariosOrdenId(id);
            var comentario = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<ComentariosOrden>.Falla(resultado.MensajeError);
            }

            var estadoDTO = new ComentariosOrden
            {
                Id = comentario.Id,
                OrdenId = comentario.OrdenId,
                ItemId = comentario.ItemId,
                UsuarioId = comentario.UsuarioId,
                Comentario = comentario.Comentario,
                CreadoEn = comentario.CreadoEn
            };

            return Resultado<ComentariosOrden>.Exito(estadoDTO);
        }

        public async Task<Resultado<List<ComentariosOrden>>> ComentariosOrdenPorOrdenId(int ordenId)
        {
            var resultado = await _repositorioComentariosOrden.ComentariosPorOrdenId(ordenId);
            var comentarios = resultado.Valor!;
            if (!resultado.esExitoso)
            {
                return Resultado<List<ComentariosOrden>>.Falla(resultado.MensajeError);
            }
            var comentariosDTO = new List<ComentariosOrden>();
            foreach (var comentario in comentarios)
            {
                var comentarioDTO = new ComentariosOrden
                {
                    Id = comentario.Id,
                    OrdenId = comentario.OrdenId,
                    ItemId = comentario.ItemId,
                    UsuarioId = comentario.UsuarioId,
                    Comentario = comentario.Comentario,
                    CreadoEn = comentario.CreadoEn
                };
                comentariosDTO.Add(comentarioDTO);
            }
            return Resultado<List<ComentariosOrden>>.Exito(comentariosDTO);
        }

        public async Task<Resultado<List<ComentariosOrden>>> ComentariosOrdenPorItemId(int itemId)
        {
            var resultado = await _repositorioComentariosOrden.ComentariosPorItemId(itemId);
            var comentarios = resultado.Valor!;
            if (!resultado.esExitoso)
            {
                return Resultado<List<ComentariosOrden>>.Falla(resultado.MensajeError);
            }
            var comentariosDTO = new List<ComentariosOrden>();
            foreach (var comentario in comentarios)
            {
                var comentarioDTO = new ComentariosOrden
                {
                    Id = comentario.Id,
                    OrdenId = comentario.OrdenId,
                    ItemId = comentario.ItemId,
                    UsuarioId = comentario.UsuarioId,
                    Comentario = comentario.Comentario,
                    CreadoEn = comentario.CreadoEn
                };
                comentariosDTO.Add(comentarioDTO);
            }
            return Resultado<List<ComentariosOrden>>.Exito(comentariosDTO);
        }

        public async Task<Resultado<List<ComentariosOrden>>> ComentariosOrdenPorUsuarioId(int usuarioId)
        {
            var resultado = await _repositorioComentariosOrden.ComentariosPorUsuarioId(usuarioId);
            var comentarios = resultado.Valor!;
            if (!resultado.esExitoso)
            {
                return Resultado<List<ComentariosOrden>>.Falla(resultado.MensajeError);
            }
            var comentariosDTO = new List<ComentariosOrden>();
            foreach (var comentario in comentarios)
            {
                var comentarioDTO = new ComentariosOrden
                {
                    Id = comentario.Id,
                    OrdenId = comentario.OrdenId,
                    ItemId = comentario.ItemId,
                    UsuarioId = comentario.UsuarioId,
                    Comentario = comentario.Comentario,
                    CreadoEn = comentario.CreadoEn
                };
                comentariosDTO.Add(comentarioDTO);
            }
            return Resultado<List<ComentariosOrden>>.Exito(comentariosDTO);
        }

        public async Task<Resultado<ComentariosOrdenDTO>> CrearComentariosOrden(CrearComentariosOrdenDTO comentarios)
        {
            var resultado = await _repositorioComentariosOrden.CrearComentariosOrden(comentarios);
            var comentario = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<ComentariosOrdenDTO>.Falla(resultado.MensajeError);
            }

            var estadoDTO = new ComentariosOrdenDTO
            {
                Id = comentario.Id,
                OrdenId = comentario.OrdenId,
                ItemId = comentario.ItemId,
                UsuarioId = comentario.UsuarioId,
                Comentario = comentario.Comentario,
                CreadoEn = comentario.CreadoEn
            };

            return Resultado<ComentariosOrdenDTO>.Exito(estadoDTO);
        }

        public async Task<Resultado<ComentariosOrdenDTO>> ActualizarComentariosOrden(int id, CrearComentariosOrdenDTO comentariosDTO)
        {
            var resultado = await _repositorioComentariosOrden.ActualizarComentariosOrden(id, comentariosDTO);
            var comentario = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<ComentariosOrdenDTO>.Falla(resultado.MensajeError);
            }

            var estadoDTO = new ComentariosOrdenDTO
            {
                Id = comentario.Id,
                OrdenId = comentario.OrdenId,
                ItemId = comentario.ItemId,
                UsuarioId = comentario.UsuarioId,
                Comentario = comentario.Comentario,
                CreadoEn = comentario.CreadoEn
            };

            return Resultado<ComentariosOrdenDTO>.Exito(estadoDTO);
        }

        public async Task<Resultado<bool?>> Eliminar(int id)
        {
            var resultado = await _repositorioComentariosOrden.Eliminar(id);
            var estado = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<bool?>.Falla(resultado.MensajeError);
            }

            return Resultado<bool?>.Exito(estado);
        }
    }
}
