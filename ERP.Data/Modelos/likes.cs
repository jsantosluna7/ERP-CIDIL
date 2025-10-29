using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Data.Modelos
{
    
    // Representa un "Like" realizado por un usuario institucional en un anuncio.
    // Cada registro indica qué usuario dio like a qué anuncio y cuándo.
   
    [Table("likes")]
    public class Like
    {
        // 🔑 Clave primaria
        [Key]
        [Column("id")]
        public int Id { get; set; }

        // 📢 Anuncio al que pertenece el Like
        [Required]
        [Column("anuncio_id")]
        public int AnuncioId { get; set; }

        // 🔗 Usuario que dio el Like
        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario Usuario { get; set; } = null!;

        // 🌐 IP del usuario (opcional, útil para auditoría)
        [Column("ip_usuario")]
        [MaxLength(50)]
        public string IpUsuario { get; set; } = string.Empty;

        // 🕒 Fecha de creación del Like
        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // 🔗 Relación con el anuncio
        [ForeignKey(nameof(AnuncioId))]
        public Anuncio Anuncio { get; set; } = null!;
    }
}
