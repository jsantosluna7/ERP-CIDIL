using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sprache;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.DTO.LoginDTO;
using Usuarios.Implementaciones.Servicios;
using Usuarios.Modelos;

namespace Usuarios.Implementaciones.Repositorios
{
    public class RepositorioLogin : IRepositorioLogin
    {
        private readonly DbErpContext _context;
        private readonly ServicioOtp _servicioOtp;
        private readonly ServicioEmailUsuarios _email;

        public RepositorioLogin(DbErpContext context, ServicioOtp servicioOtp, ServicioEmailUsuarios emailUsuarios)
        {
            _context = context;
            _servicioOtp = servicioOtp;
            _email = emailUsuarios;

        }

        //Método para iniciar seción
        public async Task<Resultado<Token?>> IniciarSecion(Login login)
        {
            // Verificar si el usuario existe
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoInstitucional == login.CorreoInstitucional);

            if (usuario == null)
            {
                return Resultado<Token?>.Falla("El correo institucional no existe.");
            }

            if(login.Contrasena.Length < 8)
            {
                return Resultado<Token?>.Falla("La contraseña debe tener al menos 8 caracteres.");
            }

            bool esValido = VerificarHash(login.Contrasena, usuario.ContrasenaHash);

            if (!esValido)
            {
                return Resultado<Token?>.Falla("La contraseña no coincide, verifique.");
            }

            // Actualizar la fecha de la última sesión
            usuario.UltimaSesion = DateTime.UtcNow;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim("idRol", usuario.IdRol.ToString()),
                new Claim("nombreUsuario", usuario.NombreUsuario),
                new Claim("apellidoUsuario", usuario.ApellidoUsuario),
                new Claim("correoInstitucional", usuario.CorreoInstitucional),
                new Claim("idMatricula", usuario.IdMatricula.ToString()),
                new Claim("telefono", usuario.Telefono),
                new Claim("direccion", usuario.Direccion),
                new Claim("fechaCreacion", usuario.FechaCreacion.ToString()),
                new Claim("fechaUltimaModificacion", usuario.FechaUltimaModificacion.ToString()),
                new Claim("ultimaSesion", usuario.UltimaSesion.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8aSX$jhE6WX2&jW9XaZUT4LiEP#TK!VyC^wt3ZqdRWJYtcv75J%cCRZd867JjXqtAAZgL%")); // Clave secreta para firmar el token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "cidilipl.online",
                audience: "cidilipl.online",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            // Crear el token
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Crear el objeto Token con el token generado
            var tokenResult = new Token
            {
                TokenId = tokenString,
            };

            // Devolver el usuario encontrado
            return Resultado<Token?>.Exito(tokenResult);
        }

        //Método para verificar el otp
        public async Task<Resultado<Token?>> verificarOtp(VerificarOtpDTO verificarOtp)
        {
            var usuarioPendiente = _context.UsuariosPendientes.FirstOrDefault(u => u.Id == verificarOtp.PendingUserId);

            if (usuarioPendiente == null)
            {
                return Resultado<Token?>.Falla("Usuario pendiente no encontrado.");
            }

            // Verificar si el OTP ha expirado
            if (usuarioPendiente.OtpExpira < DateTime.UtcNow)
            {
                return Resultado<Token?>.Falla("El OTP ha expirado. Por favor, solicite uno nuevo.");
            }

            // Verificar si el OTP es correcto
            usuarioPendiente.OtpIntentos++;
            await _context.SaveChangesAsync(); // Guardar incremento

            if (usuarioPendiente.OtpIntentos > 3)
            {
                return Resultado<Token?>.Falla("Ha superado el número máximo de intentos. Por favor, solicite un nuevo OTP.");
            }

            var providedHash = _servicioOtp.HashOtp(verificarOtp.Otp);
            if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(usuarioPendiente.OtpHash),
                Encoding.UTF8.GetBytes(providedHash)
            ))
            {
                return Resultado<Token?>.Falla("El OTP es incorrecto. Por favor, verifique e intente nuevamente.");
            }

            // Si el OTP es correcto, crear el usuario en la tabla Usuarios
            var nuevoUsuario = new Usuario
            {
                IdMatricula = usuarioPendiente.IdMatricula,
                NombreUsuario = usuarioPendiente.NombreUsuario,
                ApellidoUsuario = usuarioPendiente.ApellidoUsuario,
                CorreoInstitucional = usuarioPendiente.CorreoInstitucional,
                ContrasenaHash = usuarioPendiente.ContrasenaHash,
                Telefono = usuarioPendiente.Telefono,
                Direccion = usuarioPendiente.Direccion,
                FechaCreacion = DateTime.UtcNow,
                IdRol = usuarioPendiente.IdRol,
                UltimaSesion = DateTime.UtcNow,
                FechaUltimaModificacion = DateTime.UtcNow
            };

            // Guardar el nuevo usuario en la base de datos
            using var transaction = await _context.Database.BeginTransactionAsync();

            await _context.Usuarios.AddAsync(nuevoUsuario);
            await _context.SaveChangesAsync();

            await _context.UsuariosPendientes.Where(u => u.CorreoInstitucional == usuarioPendiente.CorreoInstitucional).ExecuteDeleteAsync();
            await transaction.CommitAsync();

            var claims = new[]
{
                new Claim(JwtRegisteredClaimNames.Sub, nuevoUsuario.Id.ToString()),
                new Claim("idRol", nuevoUsuario.IdRol.ToString()),
                new Claim("nombreUsuario", nuevoUsuario.NombreUsuario),
                new Claim("apellidoUsuario", nuevoUsuario.ApellidoUsuario),
                new Claim("correoInstitucional", nuevoUsuario.CorreoInstitucional),
                new Claim("idMatricula", nuevoUsuario.IdMatricula.ToString()),
                new Claim("telefono", nuevoUsuario.Telefono),
                new Claim("direccion", nuevoUsuario.Direccion),
                new Claim("fechaCreacion", nuevoUsuario.FechaCreacion.ToString()),
                new Claim("fechaUltimaModificacion", nuevoUsuario.FechaUltimaModificacion.ToString()),
                new Claim("ultimaSesion", nuevoUsuario.UltimaSesion.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8aSX$jhE6WX2&jW9XaZUT4LiEP#TK!VyC^wt3ZqdRWJYtcv75J%cCRZd867JjXqtAAZgL%")); // Clave secreta para firmar el token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "cidilipl.online",
                audience: "cidilipl.online",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            // Crear el token
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Crear el objeto Token con el token generado
            var tokenResult = new Token
            {
                TokenId = tokenString,
            };

            return Resultado<Token?>.Exito(tokenResult);
        }

        //Método para registrar un usuario
        public async Task<Resultado<Token?>> RegistrarUsuario(CrearRegistroDTO crearRegistroDTO)
        {
            try
            {
                // OTP
                var otp = _servicioOtp.GenerarOtp();
                var hashOtp = _servicioOtp.HashOtp(otp);

                // Verificar si el usuario ya existe
                var correoExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.CorreoInstitucional == crearRegistroDTO.CorreoInstitucional);
                var matriculaExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdMatricula == crearRegistroDTO.IdMatricula);
                if (correoExistente != null)
                {
                    return Resultado<Token?>.Falla("El correo institucional ya está en uso.");
                }

                if (matriculaExistente != null)
                {
                    return Resultado<Token?>.Falla("La matricula ya está en uso.");
                }

                if (string.IsNullOrEmpty(crearRegistroDTO.ContrasenaHash) || crearRegistroDTO.ContrasenaHash.Length < 8)
                {
                    return Resultado<Token?>.Falla("La contraseña debe tener al menos 8 caracteres.");
                }

                string correo = crearRegistroDTO.CorreoInstitucional;

                if (!string.IsNullOrEmpty(correo))
                {
                    if (!correo.EndsWith("@ipl.edu.do", StringComparison.OrdinalIgnoreCase))
                    {
                        // El correo no pertenece al dominio institucional
                        return Resultado<Token?>.Falla("El correo institucional debe terminar con @ipl.edu.do.");
                    }

                    char primerCaracter = correo[0];

                    if (char.IsDigit(primerCaracter))
                    {
                        // El correo empieza con número

                        //Creamos el hash de la contraseña
                        string hash = BCrypt.Net.BCrypt.HashPassword(crearRegistroDTO.ContrasenaHash);

                        // Generar un ID único para el usuario pendiente
                        var pendingUserId = Guid.NewGuid();

                        // Guardar usuario en PendingUsers
                        var usuario = new UsuariosPendiente
                        {
                            Id = pendingUserId,
                            IdMatricula = crearRegistroDTO.IdMatricula,
                            NombreUsuario = crearRegistroDTO.NombreUsuario,
                            ApellidoUsuario = crearRegistroDTO.ApellidoUsuario,
                            CorreoInstitucional = crearRegistroDTO.CorreoInstitucional,
                            ContrasenaHash = hash,
                            Telefono = crearRegistroDTO.Telefono,
                            Direccion = crearRegistroDTO.Direccion,
                            FechaCreacion = DateTime.UtcNow,
                            IdRol = 4,
                            OtpHash = hashOtp,
                            OtpExpira = DateTime.UtcNow.AddMinutes(5), // El OTP expira en 5 minutos
                            OtpIntentos = 0 // Inicializar intentos a 0
                        };


                        // Guardar el nuevo usuario en la base de datos
                        await _context.UsuariosPendientes.AddAsync(usuario);
                        await _context.SaveChangesAsync();

                        try
                        {
                            await _email.EnviarCorreoOtp(crearRegistroDTO.CorreoInstitucional, otp);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error enviando correo: " + ex.Message);
                            return Resultado<Token?>.Falla("No se pudo enviar el correo de verificación.");
                        }

                        var tokenString = SeguridadJwt(usuario);

                        return Resultado<Token?>.Exito(tokenString);
                    }
                    else if (char.IsLetter(primerCaracter))
                    {
                        // El correo empieza con letras

                        //Creamos el hash de la contraseña
                        string hash = BCrypt.Net.BCrypt.HashPassword(crearRegistroDTO.ContrasenaHash);

                        // Generar un ID único para el usuario pendiente
                        var pendingUserId = Guid.NewGuid();

                        // Crear un nuevo usuario
                        var usuario = new UsuariosPendiente
                        {
                            Id = pendingUserId,
                            IdMatricula = crearRegistroDTO.IdMatricula,
                            NombreUsuario = crearRegistroDTO.NombreUsuario,
                            ApellidoUsuario = crearRegistroDTO.ApellidoUsuario,
                            CorreoInstitucional = crearRegistroDTO.CorreoInstitucional.Trim(),
                            ContrasenaHash = hash,
                            Telefono = crearRegistroDTO.Telefono,
                            Direccion = crearRegistroDTO.Direccion,
                            FechaCreacion = DateTime.UtcNow,
                            IdRol = 3,
                            OtpHash = hashOtp,
                            OtpExpira = DateTime.UtcNow.AddMinutes(5), // El OTP expira en 5 minutos
                            OtpIntentos = 0 // Inicializar intentos a 0
                        };


                        // Guardar el nuevo usuario en la base de datos
                        await _context.UsuariosPendientes.AddAsync(usuario);
                        await _context.SaveChangesAsync();

                        try
                        {
                            await _email.EnviarCorreoOtp(crearRegistroDTO.CorreoInstitucional, otp);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error enviando correo: " + ex.Message);
                            return Resultado<Token?>.Falla("No se pudo enviar el correo de verificación.");
                        }

                        var tokenString = SeguridadJwt(usuario);

                        return Resultado<Token?>.Exito(tokenString);
                    }
                    else
                    {
                        // El correo no empieza con letra ni número
                        return Resultado<Token?>.Falla("El correo institucional debe empezar con una letra o un número.");
                    }
                }
                else
                {
                    return Resultado<Token?>.Falla("El correo institucional no puede estar vacío.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en RegistrarUsuario: {ex}");
                return Resultado<Token?>.Falla("Error inesperado en el servidor: " + ex.Message);
            }
        }

        private bool VerificarHash(string contrasena, string contrasenaHashAlmacenada)
        {
            return BCrypt.Net.BCrypt.Verify(contrasena, contrasenaHashAlmacenada);
        }

        private Token SeguridadJwt(UsuariosPendiente usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim("correoInstitucional", usuario.CorreoInstitucional),
                new Claim("OtpHash", usuario.OtpHash),
                new Claim("OtpExpira", usuario.OtpExpira.ToString()),
                new Claim("OtpIntentos", usuario.OtpIntentos.ToString())

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8aSX$jhE6WX2&jW9XaZUT4LiEP#TK!VyC^wt3ZqdRWJYtcv75J%cCRZd867JjXqtAAZgL%")); // Clave secreta para firmar el token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "cidilipl.online",
                audience: "cidilipl.online",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            // Crear el token
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Crear el objeto Token con el token generado
            var tokenResult = new Token
            {
                TokenId = tokenString,
            };

            return tokenResult;
        }
    }
}
