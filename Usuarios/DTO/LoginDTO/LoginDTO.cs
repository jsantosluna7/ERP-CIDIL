using ERP.Data.Modelos;
using Usuarios.Modelos;

namespace Usuarios.DTO.LoginDTO
{
    public class LoginDTO
    {
        public int Id { get; set; }

        public int IdMatricula { get; set; }

        public string NombreUsuario { get; set; } = null!;

        public string ApellidoUsuario { get; set; } = null!;

        public string CorreoInstitucional { get; set; } = null!;

        public string? Telefono { get; set; }

        public string? Direccion { get; set; }

        public int? IdRol { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaUltimaModificacion { get; set; }

        public DateTime? UltimaSesion { get; set; }

    }

    public class GoogleAuthDTO
    {
        public string AccessToken { get; set; } = string.Empty;
    }

    public class GoogleUserInfoDTO
    {
        public string sub { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public bool email_verified { get; set; }
        public string given_name { get; set; } = string.Empty;
        public string family_name { get; set; } = string.Empty;
        public string picture { get; set; } = string.Empty;
    }

    public class GoogleAuthResultDTO
    {
        public bool Existe { get; set; }
        public Token? Token { get; set; }
        public string CorreoInstitucional { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string ApellidoUsuario { get; set; } = string.Empty;
        public string FotoPerfil { get; set; } = string.Empty;
    }

    public class CompletarRegistroGoogleDTO
    {
        // Se reenvía el mismo accessToken para volver a verificar contra Google
        // en vez de confiar en el email/nombre que mande el front por JSON.
        public string AccessToken { get; set; } = string.Empty;
        public int IdMatricula { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
    }
}
