using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Usuarios.Modelos;

namespace Usuarios.Modelos;

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

    public int IdRol { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaUltimaModificacion { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpira { get; set; }

    [JsonIgnore]
    public virtual Roles IdRolNavigation { get; set; } = null!;
}