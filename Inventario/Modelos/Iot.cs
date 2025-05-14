using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Inventario.Modelos;

public partial class Iot
{
    public int Id { get; set; }

    public int IdPlaca { get; set; }

    public int IdLaboratorio { get; set; }

    public float? Sensor1 { get; set; }

    public float? Sensor2 { get; set; }

    public float? Sensor3 { get; set; }

    public float? Sensor4 { get; set; }

    public float? Sensor5 { get; set; }

    public bool? Actuador { get; set; }
    [JsonIgnore]
    public virtual Laboratorio IdLaboratorioNavigation { get; set; } = null!;
}
