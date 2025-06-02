using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Reservas.Modelos;

public partial class ReservaDeEspacio
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdLaboratorio { get; set; }

    public int IdEstado { get; set; }

    public string Motivo { get; set; } = null!;

    public DateTime? FechaSolicitud { get; set; }

    public int? IdUsuarioAprobador { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public string? ComentarioAprobacion { get; set; }

    public bool? Activado { get; set; }

    public DateTime? HoraInicio { get; set; }

    public DateTime? HoraFinal { get; set; }
    [JsonIgnore]
    public virtual Estado IdEstadoNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual Laboratorio IdLaboratorioNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual Usuario? IdUsuarioAprobadorNavigation { get; set; }
    [JsonIgnore]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
