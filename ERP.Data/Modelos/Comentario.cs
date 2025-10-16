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

        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // 🔑 CORRECCIÓN CRÍTICA: Se añade la propiedad para guardar el nombre de usuario
        // Mapea al campo 'usuario' (o similar) que debe existir en la tabla 'comentarios'
        [Column("usuario")] // Ajusta este nombre si la columna se llama diferente, ej: 'nombre_usuario'
        [MaxLength(150)]
        public string? NombreUsuario { get; set; }

        // ✅ Relación con UsuarioPublico (no debe ser null si existe)
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public UsuarioPublico Usuario { get; set; } = null!;

        // ✅ Relación con Anuncio
        [Column("anuncio_id")]
        public int AnuncioId { get; set; }

        [ForeignKey(nameof(AnuncioId))]
        public Anuncio Anuncio { get; set; } = null!;
    }
}
