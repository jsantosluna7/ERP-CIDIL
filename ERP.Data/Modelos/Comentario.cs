using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Data.Modelos
{
    [Table("comentarios")]
    public class Comentario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("texto")]
        [MaxLength(500)]
        public string Texto { get; set; } = string.Empty;

        // 🔗 Relación con UsuarioPublico (clave foránea)
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public UsuarioPublico Usuario { get; set; } = null!;

        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // 🔗 Relación con Anuncio (clave foránea)
        [Column("anuncio_id")]
        public int AnuncioId { get; set; }

        [ForeignKey(nameof(AnuncioId))]
        public Anuncio Anuncio { get; set; } = null!;
    }
}
