using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Data.Modelos
{
    [Table("anuncios")]
    public class Anuncio
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("titulo")]
        [MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Column("imagen_url")]
        public string ImagenUrl { get; set; } = string.Empty;

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Column("fecha_publicacion")]
        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;

        // 🔗 Relación con UsuarioPublico (quien creó el anuncio)
        [ForeignKey(nameof(Usuario))]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        public UsuarioPublico? Usuario { get; set; }

        // Campo para diferenciar si el anuncio es pasantía
        [Column("es_pasantia")]
        public bool EsPasantia { get; set; } = false;

        // Relaciones con Comentarios y Likes
        public List<Comentario> Comentarios { get; set; } = new();
        public List<Like> Likes { get; set; } = new();
    }
}
