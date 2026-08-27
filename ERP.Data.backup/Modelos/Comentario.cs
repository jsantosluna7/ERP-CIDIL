using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class Comentario
{
    public int Id { get; set; }

    public int AnuncioId { get; set; }

    public int? UsuarioId { get; set; }

    public string? Usuario { get; set; }

    public string Texto { get; set; } = null!;

    public DateTime? Fecha { get; set; }

    public virtual Anuncio Anuncio { get; set; } = null!;
}
