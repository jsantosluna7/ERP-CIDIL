using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ERP.Data.Modelos;

public partial class ReporteFalla
{
    public int IdReporte { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaUltimaActualizacion { get; set; }

    public string? Lugar { get; set; }

    public int Estado { get; set; }

    public int IdUsuario { get; set; }

    [JsonIgnore]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}