using System;

namespace ERP.Data.Modelos
{
    public class Like
    {
        public int Id { get; set; }

        public int AnuncioId { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public string IpUsuario { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public Anuncio? Anuncio { get; set; }
    }
}
