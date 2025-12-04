using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class Curriculum
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? ArchivoUrl { get; set; }

    public DateTime? Fecha { get; set; }

    public DateTime FechaEnvio { get; set; }

    public int? UsuarioId { get; set; }

    public bool? EsExterno { get; set; }

    public int? AnuncioId { get; set; }

    public virtual Anuncio? Anuncio { get; set; }
}
