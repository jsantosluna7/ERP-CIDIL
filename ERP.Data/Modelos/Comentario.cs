using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Data.Modelos
{
    /// <summary>
    /// Representa un comentario realizado por un usuario institucional en un anuncio.
    /// Cada comentario pertenece a un anuncio y a un usuario institucional.
    /// </summary>
    [Table("comentarios")]
    public class Comentario
    {
        /// <summary>
        /// Identificador único del comentario.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Texto del comentario.
        /// </summary>
        [Required(ErrorMessage = "El texto del comentario es obligatorio.")]
        [MaxLength(500, ErrorMessage = "El comentario no puede superar los 500 caracteres.")]
        [Column("texto")]
        public string Texto { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora en que se creó el comentario.
        /// </summary>
        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Nombre visible del usuario que realizó el comentario.
        /// Mapea a la columna 'usuario' en la base de datos.
        /// </summary>
        [MaxLength(150)]
        [Column("usuario")]
        public string? NombreUsuario { get; set; }

        /// <summary>
        /// Identificador del usuario asociado al comentario.
        /// </summary>
        [Column("usuario_id")]
        [ForeignKey(nameof(Usuario))]
        public int UsuarioId { get; set; }

        /// <summary>
        /// Usuario institucional que realizó el comentario.
        /// </summary>
        public virtual Usuario Usuario { get; set; } = null!;

        /// <summary>
        /// Identificador del anuncio al que pertenece el comentario.
        /// </summary>
        [Column("anuncio_id")]
        [ForeignKey(nameof(Anuncio))]
        public int AnuncioId { get; set; }

        /// <summary>
        /// Anuncio asociado al comentario.
        /// </summary>
        public virtual Anuncio Anuncio { get; set; } = null!;
    }
}
