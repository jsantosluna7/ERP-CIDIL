using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ERP.Data.Modelos;

public partial class SolicitudPrestamosDeEquipo
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdInventario { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFinal { get; set; }

    public string Motivo { get; set; } = null!;

    public DateTime? FechaSolicitud { get; set; }

    public int? IdEstado { get; set; }
    [JsonIgnore]
    public virtual Estado? IdEstadoNavigation { get; set; }
    [JsonIgnore]
    public virtual InventarioEquipo IdInventarioNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
