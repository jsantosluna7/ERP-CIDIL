using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.LoginDTO;
using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Implementaciones.Servicios
{
    public class ServicioLogin : IServicioLogin
    {
        private readonly IRepositorioLogin _repositorioLogin;

        public ServicioLogin(IRepositorioLogin repositorioLogin)
        {
            _repositorioLogin = repositorioLogin;
        }

        //El servicio Login se encarga de realizar la lógica de negocio y de interactuar con el repositorio para obtener los datos necesarios.

        //Método para iniciar seción
        public async Task<LoginDTO?> IniciarSecion(Login login)
        {
            var usuario = await _repositorioLogin.IniciarSecion(login);

            if (usuario == null)
            {
                return null;
            }

            var loginDTO = new LoginDTO
            {
                Id = usuario.Id,
                IdMatricula = usuario.IdMatricula,
                NombreUsuario = usuario.NombreUsuario,
                ApellidoUsuario = usuario.ApellidoUsuario,
                CorreoInstitucional = usuario.CorreoInstitucional,
                Telefono = usuario.Telefono,
                Direccion = usuario.Direccion,
                IdRol = usuario.IdRol,
                FechaCreacion = usuario.FechaCreacion,
                FechaUltimaModificacion = usuario.FechaUltimaModificacion
            };

            return loginDTO;
        }

        //Método para registrar un usuario
        public async Task<CrearRegistroDTO?> RegistrarUsuario(CrearRegistroDTO crearRegistroDTO) //El task es para que sea asincrono, y es de tipo CrearLoginDTO, esto es para que retorne el DTO de la clase CrearLoginDTO y que el usuario pueda ver el resultado de la creación del usuario que es ese DTO.
        {
            var usuario = await _repositorioLogin.RegistrarUsuario(crearRegistroDTO);
            if (usuario == null)
            {
                return null;
            }
            var crearRegistroDTORetorno = new CrearRegistroDTO
            {
                IdMatricula = usuario.IdMatricula,
                NombreUsuario = usuario.NombreUsuario,
                ApellidoUsuario = usuario.ApellidoUsuario,
                CorreoInstitucional = usuario.CorreoInstitucional,
                Telefono = usuario.Telefono,
                Direccion = usuario.Direccion,
                IdRol = usuario.IdRol,
                FechaCreacion = usuario.FechaCreacion,
                FechaUltimaModificacion = usuario.FechaUltimaModificacion
            };
            return crearRegistroDTORetorno;
        }

    }
}
