using System;
using System.Collections.Generic;

namespace IoT.Modelos;

public partial class InventarioEquipo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? NombreCorto { get; set; }

    public string? Perfil { get; set; }

    public int IdLaboratorio { get; set; }

    public string? Fabricante { get; set; }

    public string? Modelo { get; set; }

    public string Serial { get; set; } = null!;

    public string? DescripcionLarga { get; set; }

    public DateTime? FechaTransaccion { get; set; }

    public string? Departamento { get; set; }

    public decimal? ImporteActivo { get; set; }

    public byte[]? ImagenEquipo { get; set; }

    public bool? Disponible { get; set; }

    public int IdEstadoFisico { get; set; }

    public bool? ValidacionPrestamo { get; set; }

    public int? Cantidad { get; set; }

    public bool? Activado { get; set; }

    public virtual EstadoFisico IdEstadoFisicoNavigation { get; set; } = null!;

    public virtual Laboratorio IdLaboratorioNavigation { get; set; } = null!;

    public virtual ICollection<SolicitudPrestamosDeEquipo> SolicitudPrestamosDeEquipos { get; set; } = new List<SolicitudPrestamosDeEquipo>();
}
