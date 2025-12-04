using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class Usuario
{
    public int Id { get; set; }

    public int IdMatricula { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string ApellidoUsuario { get; set; } = null!;

    public string CorreoInstitucional { get; set; } = null!;

    public string ContrasenaHash { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public int? IdRol { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaUltimaModificacion { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpira { get; set; }

    public bool? Activado { get; set; }

    public DateTime? UltimaSesion { get; set; }

    public virtual ICollection<ComentariosOrden> ComentariosOrdens { get; set; } = new List<ComentariosOrden>();

    public virtual Role? IdRolNavigation { get; set; }

    public virtual ICollection<OrdenTimeline> OrdenTimelines { get; set; } = new List<OrdenTimeline>();

    public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();

    public virtual ICollection<PrestamosEquipo> PrestamosEquipoIdUsuarioAprobadorNavigations { get; set; } = new List<PrestamosEquipo>();

    public virtual ICollection<PrestamosEquipo> PrestamosEquipoIdUsuarioNavigations { get; set; } = new List<PrestamosEquipo>();

    public virtual ICollection<ReporteFalla> ReporteFallas { get; set; } = new List<ReporteFalla>();

    public virtual ICollection<ReservaDeEspacio> ReservaDeEspacioIdUsuarioAprobadorNavigations { get; set; } = new List<ReservaDeEspacio>();

    public virtual ICollection<ReservaDeEspacio> ReservaDeEspacioIdUsuarioNavigations { get; set; } = new List<ReservaDeEspacio>();

    public virtual ICollection<SolicitudPrestamosDeEquipo> SolicitudPrestamosDeEquipos { get; set; } = new List<SolicitudPrestamosDeEquipo>();

    public virtual ICollection<SolicitudReservaDeEspacio> SolicitudReservaDeEspacios { get; set; } = new List<SolicitudReservaDeEspacio>();
}
