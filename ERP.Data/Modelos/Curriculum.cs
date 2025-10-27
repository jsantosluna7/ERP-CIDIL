using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Data.Modelos
{
    [Table("curriculums")]
    public class Curriculum
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        [Column("archivo_url")]
        public string ArchivoUrl { get; set; } = string.Empty;

        [Column("fecha_envio")]
        public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;

        [Column("es_externo")]
        public bool EsExterno { get; set; } = false;

        // 🔗 Usuario institucional opcional
        [Column("usuario_id")]
        public int? UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }

        // 🔗 Anuncio relacionado (opcional)
        [Column("anuncio_id")]
        public int? AnuncioId { get; set; }

        [ForeignKey(nameof(AnuncioId))]
        public Anuncio? Anuncio { get; set; }
    }
}
