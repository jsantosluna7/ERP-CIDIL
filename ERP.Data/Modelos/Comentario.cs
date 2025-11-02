using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Data.Modelos
{
    // Representa un comentario realizado por un usuario institucional en un anuncio.
    [Table("comentarios")]
    public class Comentario
    {
    
        // Identificador único del comentario.
        
        [Key]
        [Column("id")]
        public int Id { get; set; }

       
        // Texto del comentario.
     
        [Required(ErrorMessage = "El texto del comentario es obligatorio.")]
        [MaxLength(500, ErrorMessage = "El comentario no puede superar los 500 caracteres.")]
        [Column("texto")]
        public string Texto { get; set; } = string.Empty;

        
        // Fecha y hora en que se creó el comentario.
        
        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        
        
        /// Mapea a la columna 'usuario' en la base de datos.
        
        [MaxLength(150)]
        [Column("usuario")]
        public string? NombreUsuario { get; set; }

        
        /// Identificador del usuario asociado al comentario.
        
        [Column("usuario_id")]
        [ForeignKey(nameof(Usuario))]
        public int UsuarioId { get; set; }

      
          /// Usuario institucional que realizó el comentario.
        
        public virtual Usuario Usuario { get; set; } = null!;

    
        /// Identificador del anuncio al que pertenece el comentario.
     
        [Column("anuncio_id")]
        [ForeignKey(nameof(Anuncio))]
        public int AnuncioId { get; set; }

        
        /// Anuncio asociado al comentario.
        
        public virtual Anuncio Anuncio { get; set; } = null!;
    }
}
