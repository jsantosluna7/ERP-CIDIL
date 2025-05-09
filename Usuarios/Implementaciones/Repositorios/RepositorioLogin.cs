using Microsoft.EntityFrameworkCore;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.DTO.LoginDTO;
using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Implementaciones.Repositorios
{
    public class RepositorioLogin : IRepositorioLogin
    {
        private readonly DbErpContext _context;

        public RepositorioLogin(DbErpContext context)
        {
            _context = context;
        }

        //Método para iniciar seción
        public async Task<Usuario?> IniciarSecion(Login login)
        {
            // Verificar si el usuario existe
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoInstitucional == login.CorreoInstitucional);

            if (usuario == null)
            {
                return null;
            }

            if(login.Contrasena.Length < 8)
            {
                return null;
            }

            bool esValido = VerificarHash(login.Contrasena, usuario.ContrasenaHash);

            if (!esValido)
            {
                return null;
            }

            // Devolver el usuario encontrado
            return usuario;
        }

        //Método para registrar un usuario
        public async Task<Usuario?> RegistrarUsuario(CrearRegistroDTO crearRegistroDTO)
        {
            // Verificar si el usuario ya existe
            var correoExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoInstitucional == crearRegistroDTO.CorreoInstitucional);
            var matriculaExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdMatricula == crearRegistroDTO.IdMatricula);
            if (correoExistente != null || matriculaExistente != null)
            {
                return null;
            }

            if(crearRegistroDTO.ContrasenaHash.Length < 8)
            {
                return null;
            }

            //Creamos el hash de la contraseña
            string hash = BCrypt.Net.BCrypt.HashPassword(crearRegistroDTO.ContrasenaHash);

            // Crear un nuevo usuario
            var usuario = new Usuario
            {
                IdMatricula = crearRegistroDTO.IdMatricula,
                NombreUsuario = crearRegistroDTO.NombreUsuario,
                ApellidoUsuario = crearRegistroDTO.ApellidoUsuario,
                CorreoInstitucional = crearRegistroDTO.CorreoInstitucional,
                ContrasenaHash = hash,
                Telefono = crearRegistroDTO.Telefono,
                Direccion = crearRegistroDTO.Direccion,
                IdRol = crearRegistroDTO.IdRol,
                FechaCreacion = DateTime.UtcNow,
                FechaUltimaModificacion = DateTime.UtcNow
            };


            // Guardar el nuevo usuario en la base de datos
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        private bool VerificarHash(string contrasena, string contrasenaHashAlmacenada)
        {
            return BCrypt.Net.BCrypt.Verify(contrasena, contrasenaHashAlmacenada);
        }
    }
}
