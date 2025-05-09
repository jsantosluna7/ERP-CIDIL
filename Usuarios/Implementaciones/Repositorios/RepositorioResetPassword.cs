using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Implementaciones.Servicios;
using Usuarios.Modelos;

namespace Usuarios.Implementaciones.Repositorios
{
    public class RepositorioResetPassword : IRepositorioResetPassword
    {
        private readonly DbErpContext _context;
        private readonly ServicioEmail _servicioEmail;

        public RepositorioResetPassword(DbErpContext context, ServicioEmail servicioEmail)
        {
            _context = context;
            _servicioEmail = servicioEmail;
        }

        //Método para obtener el token de restablecimiento de contraseña, y revisar si el usuario existe
        public async Task<bool?> OlvideContrasena(OlvideContrasena olvideContrasena)
        {
            // Verificar si el usuario existe
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoInstitucional == olvideContrasena.CorreoInstitucional);

            //Si el usuario no existe, devolvemos null
            if (usuario == null)
            {
                return null;
            }

            string token = Guid.NewGuid().ToString();
            usuario.ResetToken = token;
            usuario.ResetTokenExpira = DateTime.UtcNow.AddHours(1);

            await _context.SaveChangesAsync();

            // Aquí puedes enviar el token al correo del usuario
            await _servicioEmail.EnviarCorreoRecuperacion(usuario.CorreoInstitucional, token);

            // Devolvemos true si el token se ha generado correctamente
            return true;
        }

        //Método para restablecer la contraseña
        public async Task<bool?> RestablecerContrasena(ResetearContrasena resetearContrasena)
        {
            // Verificar si el token es válido
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.ResetToken == resetearContrasena.Token && u.ResetTokenExpira > DateTime.UtcNow);
            if (usuario == null)
            {
                return null; // El token no es válido o ha expirado
            }

            // Crear un nuevo hash de la contraseña

            string hash = BCrypt.Net.BCrypt.HashPassword(resetearContrasena.NuevaContrasena);

            // Verificar si la nueva contraseña es igual a la anterior
            if (BCrypt.Net.BCrypt.Verify(resetearContrasena.NuevaContrasena, usuario.ContrasenaHash))
            {
                return null; // La nueva contraseña no puede ser igual a la anterior
            }

            // Verificar si la nueva contraseña es válida
            if (resetearContrasena.NuevaContrasena.Length < 8)
            {
                return null; // La nueva contraseña no es válida
            }

            // Actualizar la contraseña del usuario
            usuario.ContrasenaHash = hash;
            usuario.ResetToken = null;
            usuario.ResetTokenExpira = null;
            await _context.SaveChangesAsync();

            // Aquí puedes enviar un correo al usuario informando que su contraseña ha sido restablecida
            await _servicioEmail.CorreoCambioExitoso(usuario.CorreoInstitucional);

            // Devolvemos true si la contraseña se ha restablecido correctamente
            return true;
        }
    }
}
