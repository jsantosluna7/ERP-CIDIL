using Compras.Abstraccion.Repositorios;
using Compras.DTO.ComentariosOrdenDTO;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Compras.Implementaciones.Repositorios
{
    public class RepositorioComentariosOrden : IRepositorioComentariosOrden
    {
        private readonly DbErpContext _context;

        public RepositorioComentariosOrden(DbErpContext context)
        {
            _context = context;
        }

        public async Task<Resultado<List<ComentariosOrden>>> ComentariosOrden()
        {
            var resultado = await _context.ComentariosOrdens.ToListAsync();

            if (resultado == null || resultado.Count == 0)
            {
                return Resultado<List<ComentariosOrden>>.Falla("No se encontraron comentarios de la orden.");
            }

            return Resultado<List<ComentariosOrden>>.Exito(resultado);
        }

        public async Task<Resultado<ComentariosOrden>> ComentariosOrdenId(int id)
        {
            var resultado = await _context.ComentariosOrdens.FirstOrDefaultAsync(e => e.Id == id);

            if (resultado == null)
            {
                return Resultado<ComentariosOrden>.Falla("No se encontró un comentario de una orden con el ID");
            }

            return Resultado<ComentariosOrden>.Exito(resultado);
        }

        public async Task<Resultado<List<ComentariosOrden>>> ComentariosPorItemId(int itemId)
        {
            if(itemId <= 0)
            {
                return Resultado<List<ComentariosOrden>>.Falla("El ID del item proporcionado no es válido.");
            }

            var resultado = await _context.ComentariosOrdens
                .Where(e => e.ItemId == itemId)
                .ToListAsync();
            if (resultado == null || resultado.Count == 0)
            {
                return Resultado<List<ComentariosOrden>>.Falla("No se encontraron comentarios para el item proporcionado.");
            }
            return Resultado<List<ComentariosOrden>>.Exito(resultado);
        }

        public async Task<Resultado<List<ComentariosOrden>>> ComentariosPorOrdenId(int ordenId)
        {
            if(ordenId <= 0)
            {
                return Resultado<List<ComentariosOrden>>.Falla("El ID de la orden proporcionado no es válido.");
            }

            var resultado = await _context.ComentariosOrdens
                .Where(e => e.OrdenId == ordenId)
                .ToListAsync();
            if (resultado == null || resultado.Count == 0)
            {
                return Resultado<List<ComentariosOrden>>.Falla("No se encontraron comentarios para la orden proporcionada.");
            }
            return Resultado<List<ComentariosOrden>>.Exito(resultado);
        }

        public async Task<Resultado<List<ComentariosOrden>>> ComentariosPorUsuarioId(int usuarioId)
        {
            if(usuarioId <= 0)
            {
                return Resultado<List<ComentariosOrden>>.Falla("El ID del usuario proporcionado no es válido.");
            }

            var resultado = await _context.ComentariosOrdens
                .Where(e => e.UsuarioId == usuarioId)
                .ToListAsync();
            if (resultado == null || resultado.Count == 0)
            {
                return Resultado<List<ComentariosOrden>>.Falla("No se encontraron comentarios hechas por el usuario proporcionado.");
            }
            return Resultado<List<ComentariosOrden>>.Exito(resultado);
        }

        public async Task<Resultado<ComentariosOrden>> CrearComentariosOrden(CrearComentariosOrdenDTO comentario)
        {
            if (comentario == null)
            {
                Resultado<ComentariosOrden>.Falla("No se pueden dejar campos vacios.");
            }

            var comentarios = new ComentariosOrden
            {
                OrdenId = comentario.OrdenId,
                ItemId = comentario.ItemId,
                UsuarioId = comentario.UsuarioId,
                Comentario = comentario.Comentario
            };

            _context.ComentariosOrdens.Add(comentarios);
            await _context.SaveChangesAsync();
            return Resultado<ComentariosOrden>.Exito(comentarios);
        }

        public async Task<Resultado<ComentariosOrden>> ActualizarComentariosOrden(int id, CrearComentariosOrdenDTO comentarioDTO)
        {
            var existeComentario = await ComentariosOrdenId(id);
            var comentario = existeComentario.Valor;

            if (comentario == null)
            {
                return Resultado<ComentariosOrden>.Falla(existeComentario.MensajeError);
            }

            var comentarios = new ComentariosOrden
            {
                Id = comentario.Id,
                OrdenId = comentarioDTO.OrdenId ?? comentario.OrdenId,
                ItemId = comentarioDTO.ItemId ?? comentario.ItemId,
                UsuarioId = comentarioDTO.UsuarioId ?? comentario.UsuarioId,
                Comentario = comentarioDTO.Comentario ?? comentario.Comentario,
                CreadoEn = comentario.CreadoEn
            };

            _context.Update(comentarios);
            _context.SaveChanges();
            var comentariosActualizados = await ComentariosOrdenId(id);
            var comentariosAct = comentariosActualizados.Valor!;
            return Resultado<ComentariosOrden>.Exito(comentariosAct);
        }

        public async Task<Resultado<bool?>> Eliminar(int id)
        {
            var comentariosPorId = await ComentariosOrdenId(id);
            var comentarios = comentariosPorId.Valor!;

            if (comentarios == null)
            {
                return Resultado<bool?>.Falla(comentariosPorId.MensajeError);
            }

            _context.Remove(comentariosPorId);
            _context.SaveChanges();
            return Resultado<bool?>.Exito(true);
        }
    }
}
