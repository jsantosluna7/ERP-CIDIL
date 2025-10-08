using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Data.Modelos
{
    [Table("curriculums")] // Asegúrate de que el nombre de la tabla sea correcto
    public class Curriculum
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("archivo_url")]
        public string ArchivoUrl { get; set; } = string.Empty;

        [Required]
        [Column("fecha_envio")]
        public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;
    }
}
