using System;

namespace ERP.Data.Modelos
{
    public class Like
    {
        public int Id { get; set; }
        public int AnuncioId { get; set; }

        // Cambiado de UsuarioId → Usuario (para mantener consistencia con el DTO y servicio)
        public string Usuario { get; set; } = string.Empty;

        public string IpUsuario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public Anuncio? Anuncio { get; set; }
    }
}
