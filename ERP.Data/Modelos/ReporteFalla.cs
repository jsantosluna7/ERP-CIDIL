using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ERP.Data.Modelos;

public partial class ReporteFalla
{
    public int IdReporte { get; set; }

    public int? IdLaboratorio { get; set; }

    public string Descripcion { get; set; } = null!;

    public string NombreSolicitante { get; set; } = null!;

    public int IdEstado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaUltimaActualizacion { get; set; }

    public string? Lugar { get; set; }

    [JsonIgnore]
    public virtual Estado IdEstadoNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual Laboratorio? IdLaboratorioNavigation { get; set; }
}
