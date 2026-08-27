using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ERP.Data.Modelos;

public partial class ExtensionPrestamosEquipo
{
    public int Id { get; set; }

    public int IdPrestamos { get; set; }

    public DateTime FechaExtensionSolicitada { get; set; }

    public DateTime FechaSolicitud { get; set; }

    public int IdEstado { get; set; }

    public string? Motivo { get; set; }

    public string? ComentarioAprobacion { get; set; }

    public int? IdUsuarioAprobador { get; set; }

    [JsonIgnore]
    public virtual Estado IdEstadoNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual PrestamosEquipo IdPrestamosNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual Usuario? IdUsuarioAprobadorNavigation { get; set; }
}
