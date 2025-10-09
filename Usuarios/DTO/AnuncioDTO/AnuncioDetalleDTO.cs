using System;
using System.Collections.Generic;
using Usuarios.DTO.Comentarios;

namespace Usuarios.DTO.AnuncioDTO
{
    /// <summary>
    /// DTO que representa un anuncio con todos sus detalles,
    /// incluyendo comentarios y cantidad de likes.
    /// </summary>
    public class AnuncioDetalleDTO
    {
        /// <summary>
        /// Identificador único del anuncio.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título del anuncio.
        /// </summary>
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del anuncio.
        /// </summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// URL de la imagen asociada al anuncio.
        /// </summary>
        public string ImagenUrl { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de publicación del anuncio.
        /// </summary>
        public DateTime FechaPublicacion { get; set; }

        /// <summary>
        /// Cantidad total de likes del anuncio.
        /// </summary>
        public int CantidadLikes { get; set; }

        /// <summary>
        /// Lista de comentarios asociados al anuncio.
        /// </summary>
        public List<ComentarioDTO> Comentarios { get; set; } = new List<ComentarioDTO>();
    }
}
