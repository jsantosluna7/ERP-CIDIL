using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Data.Modelos
{
    [Table("likes")]
    public class Like
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("anuncio_id")]
        public int AnuncioId { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; } // FK a UsuarioPublico

        [ForeignKey(nameof(UsuarioId))]
        public UsuarioPublico? Usuario { get; set; }

        [Column("ip_usuario")]
        [MaxLength(50)]
        public string IpUsuario { get; set; } = string.Empty;

        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(AnuncioId))]
        public Anuncio? Anuncio { get; set; }
    }
}
