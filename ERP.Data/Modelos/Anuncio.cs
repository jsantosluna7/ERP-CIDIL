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
        [Column("Id")] //  así, con mayúscula como en la BD
        public int Id { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Column("imagen_url")]
        public string ImagenUrl { get; set; } = string.Empty;

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Column("fecha_publicacion")]
        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        public List<Comentario> Comentarios { get; set; } = new();
        public List<Like> Likes { get; set; } = new();
    }
}
