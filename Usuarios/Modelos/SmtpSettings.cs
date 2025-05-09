using System.ComponentModel.DataAnnotations;

namespace Usuarios.Modelos
{
    public class SmtpSettings
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required bool EnableSsl { get; set; }
        public required string User { get; set; }
        public required string Password { get; set; }
    }
}
