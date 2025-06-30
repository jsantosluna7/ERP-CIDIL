using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.DTO.LoginDTO;
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
        public async Task<Resultado<Usuario?>> IniciarSecion(Login login)
        {
            // Verificar si el usuario existe
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoInstitucional == login.CorreoInstitucional);

            if (usuario == null)
            {
                return Resultado<Usuario?>.Falla("El correo institucional no existe.");
            }

            if(login.Contrasena.Length < 8)
            {
                return Resultado<Usuario?>.Falla("La contraseña debe tener al menos 8 caracteres.");
            }

            bool esValido = VerificarHash(login.Contrasena, usuario.ContrasenaHash);

            if (!esValido)
            {
                return Resultado<Usuario?>.Falla("La contraseña no coincide, verifique.");
            }

            // Actualizar la fecha de la última sesión
            usuario.UltimaSesion = DateTime.UtcNow;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            // Devolver el usuario encontrado
            return Resultado<Usuario?>.Exito(usuario);
        }

        //Método para registrar un usuario
        public async Task<Resultado<Usuario?>> RegistrarUsuario(CrearRegistroDTO crearRegistroDTO)
        {
            // Verificar si el usuario ya existe
            var correoExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoInstitucional == crearRegistroDTO.CorreoInstitucional);
            var matriculaExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdMatricula == crearRegistroDTO.IdMatricula);
            if (correoExistente != null)
            {
                return Resultado<Usuario?>.Falla("El correo institucional ya está en uso.");
            }

            if(matriculaExistente != null)
            {
                return Resultado<Usuario?>.Falla("La matricula ya está en uso.");
            }

            if(crearRegistroDTO.ContrasenaHash.Length < 8)
            {
                return Resultado<Usuario?>.Falla("La contraseña debe tener al menos 8 caracteres.");
            }

            string correo = crearRegistroDTO.CorreoInstitucional;

            if (!string.IsNullOrEmpty(correo))
            {
                if (!correo.EndsWith("@ipl.edu.do", StringComparison.OrdinalIgnoreCase))
                {
                    // El correo no pertenece al dominio institucional
                    return Resultado<Usuario?>.Falla("El correo institucional debe terminar con @ipl.edu.do.");
                }

                char primerCaracter = correo[0];

                if (char.IsDigit(primerCaracter))
                {
                    // El correo empieza con número

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
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaModificacion = DateTime.UtcNow,
                        UltimaSesion = DateTime.UtcNow
                    };


                    // Guardar el nuevo usuario en la base de datos
                    await _context.Usuarios.AddAsync(usuario);
                    await _context.SaveChangesAsync();
                    return Resultado<Usuario?>.Exito(usuario);
                }
                else if (char.IsLetter(primerCaracter))
                {
                    // El correo empieza con letras

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
                        IdRol = 3,
                        FechaCreacion = DateTime.UtcNow,
                        FechaUltimaModificacion = DateTime.UtcNow,
                        UltimaSesion = DateTime.UtcNow
                    };


                    // Guardar el nuevo usuario en la base de datos
                    await _context.Usuarios.AddAsync(usuario);
                    await _context.SaveChangesAsync();
                    return Resultado<Usuario?>.Exito(usuario);
                }
                else
                {
                    // El correo no empieza con letra ni número
                    return Resultado<Usuario?>.Falla("El correo institucional debe empezar con una letra o un número.");
                }
            }
            else
            {
                return Resultado<Usuario?>.Falla("El correo institucional no puede estar vacío.");
            }
        }

        private bool VerificarHash(string contrasena, string contrasenaHashAlmacenada)
        {
            return BCrypt.Net.BCrypt.Verify(contrasena, contrasenaHashAlmacenada);
        }
    }
}
