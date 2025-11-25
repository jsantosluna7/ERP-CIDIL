using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Asegúrate de que tienes el using necesario para acceder al modelo 'Usuario',
// si no está en el mismo namespace ERP.Data.Modelos.

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

        //Clave Foránea con Usuario
        [ForeignKey(nameof(Usuario))]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        //Propiedad de navegación para cargar los datos del creador.
        // Asumo que tu modelo Usuario no acepta valores nulos en la base de datos (por eso el 'null!').
        public Usuario Usuario { get; set; } = null!;

        // Campo para diferenciar si el anuncio es pasantía
        [Column("es_pasantia")]
        public bool EsPasantia { get; set; } = false;

        // Relaciones con Comentarios y Likes
        public List<Comentario> Comentarios { get; set; } = new();
        public List<Like> Likes { get; set; } = new();

        //public bool EsCarrusel { get; set; } = false;



    }
}