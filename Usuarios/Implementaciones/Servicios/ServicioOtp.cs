using System.Security.Cryptography;
using System.Text;

namespace Usuarios.Implementaciones.Servicios
{
    public class ServicioOtp
    {
        public string GenerarOtp()
        {
            // Lógica para generar un OTP (One-Time Password)
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        public string HashOtp(string otp)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(otp));
            return Convert.ToBase64String(bytes);
        }
    }
}
