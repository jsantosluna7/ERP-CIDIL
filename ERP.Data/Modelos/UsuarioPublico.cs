using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Data.Modelos
{
    [Table("usuarios_publicos")]
    public class UsuarioPublico
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nombre")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Column("correo")]
        [MaxLength(100)]
        public string? Correo { get; set; }

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // 🔗 Relaciones con otras entidades
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
