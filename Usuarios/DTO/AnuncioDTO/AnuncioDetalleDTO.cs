using System;
using System.Collections.Generic;
using Usuarios.DTO.Comentarios; // <-- Debe apuntar exactamente al namespace

namespace Usuarios.DTO.AnuncioDTO
{
    public class AnuncioDetalleDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public DateTime FechaPublicacion { get; set; }
        public int CantidadLikes { get; set; }
        public List<ComentarioDTO> Comentarios { get; set; } = new List<ComentarioDTO>();
    }
}
