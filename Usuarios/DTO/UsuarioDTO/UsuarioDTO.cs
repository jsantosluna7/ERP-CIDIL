using Usuarios.Modelos;

namespace Usuarios.DTO.UsuarioDTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        public int? IdMatricula { get; set; }

        public string NombreUsuario { get; set; } = null!;

        public string ApellidoUsuario { get; set; } = null!;

        public string CorreoInstitucional { get; set; } = null!;

        public string? Telefono { get; set; }

        public string? Direccion { get; set; }

        public int? IdRol { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaUltimaModificacion { get; set; }
    }
}
