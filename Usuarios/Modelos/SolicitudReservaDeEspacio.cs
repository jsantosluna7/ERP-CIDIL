using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Usuarios.Modelos;

public partial class SolicitudReservaDeEspacio
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdLaboratorio { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFinal { get; set; }

    public string Motivo { get; set; } = null!;

    public DateTime? FechaSolicitud { get; set; }

    public int? IdEstado { get; set; }
    [JsonIgnore]
    public virtual Estado? IdEstadoNavigation { get; set; }
    [JsonIgnore]
    public virtual Laboratorio IdLaboratorioNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
