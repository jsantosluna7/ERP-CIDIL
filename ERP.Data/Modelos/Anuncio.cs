using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class Anuncio
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string? ImagenUrl { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int UsuarioId { get; set; }

    public DateTime FechaPublicacion { get; set; }

    public bool? EsPasantia { get; set; }

    public bool EsCarrusel { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<Curriculum> Curricula { get; set; } = new List<Curriculum>();

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
}
