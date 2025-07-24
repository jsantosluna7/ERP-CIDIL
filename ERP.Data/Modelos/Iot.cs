using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class Iot
{
    public int Id { get; set; }

    public string IdPlaca { get; set; } = null!;

    public int IdLaboratorio { get; set; }

    public float? Sensor1 { get; set; }

    public float? Sensor2 { get; set; }

    public float? Sensor3 { get; set; }

    public float? Sensor4 { get; set; }

    public float? Sensor5 { get; set; }

    public bool? Actuador { get; set; }

    public bool? Activado { get; set; }

    public DateTime? HoraEntrada { get; set; }

    public virtual Laboratorio IdLaboratorioNavigation { get; set; } = null!;
}
