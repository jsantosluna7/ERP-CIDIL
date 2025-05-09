using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Implementaciones.Servicios
{
    public class ServicioUsuarios : IServicioUsuarios
    {
        private readonly IRepositorioUsuario _repositorioUsuario;

        public ServicioUsuarios(IRepositorioUsuario repositorioUsuario)
        {
            _repositorioUsuario = repositorioUsuario;
        }

        // Método para obetener todos los usuarios
        public List<UsuarioDTO> ObtenerUsuarios()
        {
            // Obtener todos los usuarios
            var usuarios = _repositorioUsuario.obtenerUsuarios();
            if (usuarios == null || !usuarios.Any())
            {
                throw new Exception("No se encontraron usuarios");
            }

            //Inicializar la lista de usuarios DTO
            var usuariosDTO = new List<UsuarioDTO>();

            // Mapear los usuarios a DTOs
            foreach (Usuario usuario in usuarios)
            {
                var usuarioDTO = new UsuarioDTO
                {
                    IdMatricula = usuario.IdMatricula,
                    NombreUsuario = usuario.NombreUsuario,
                    ApellidoUsuario = usuario.ApellidoUsuario,
                    CorreoInstitucional = usuario.CorreoInstitucional,
                    ContrasenaHash = usuario.ContrasenaHash,
                    Telefono = usuario.Telefono,
                    Direccion = usuario.Direccion,
                    IdRol = usuario.IdRol,
                    FechaCreacion = usuario.FechaCreacion,
                    FechaUltimaModificacion = usuario.FechaUltimaModificacion
                };
                usuariosDTO.Add(usuarioDTO);
            }

            // Devolver la lista de usuarios DTO
            return usuariosDTO;
        }

        // Método para obtener un usuario por su id
        public Usuario ObtenerUsuarioPorId(int id)
        {
            // Obtener el usuario por su id
            var usuario = _repositorioUsuario.obtenerUsuarioPorId(id);
            if (usuario == null)
            {
                throw new Exception("No se encontró el usuario");
            }

            // Devolver el usuario encontrado
            return usuario;


            //// Mapear el usuario a DTO
            //var usuarioDTO = new UsuarioDTO
            //{
            //    IdMatricula = usuario.IdMatricula,
            //    NombreUsuario = usuario.NombreUsuario,
            //    ApellidoUsuario = usuario.ApellidoUsuario,
            //    CorreoInstitucional = usuario.CorreoInstitucional,
            //    ContrasenaHash = usuario.ContrasenaHash,
            //    Telefono = usuario.Telefono,
            //    Direccion = usuario.Direccion,
            //    IdRol = usuario.IdRol,
            //    FechaCreacion = usuario.FechaCreacion,
            //    FechaUltimaModificacion = usuario.FechaUltimaModificacion
            //};
            //// Devolver el usuario DTO
            //return usuarioDTO;
        }

        // Método para crear un usuario
        public UsuarioDTO crearUsuario(UsuarioDTO usuarioDTO)
        {
            // Obtener el usuario del repositorio
            var usuario = _repositorioUsuario.crearUsuario(usuarioDTO);

            // Crear un nuevo usuario
            var crearUsuarioDTO = new UsuarioDTO
            {
                IdMatricula = usuario.IdMatricula,
                NombreUsuario = usuario.NombreUsuario,
                ApellidoUsuario = usuario.ApellidoUsuario,
                CorreoInstitucional = usuario.CorreoInstitucional,
                ContrasenaHash = usuario.ContrasenaHash,
                Telefono = usuario.Telefono,
                Direccion = usuario.Direccion,
                IdRol = usuario.IdRol,
                FechaCreacion = usuario.FechaCreacion,
                FechaUltimaModificacion = usuario.FechaUltimaModificacion
            };

            // Devolver el nuevo usuario creado
            return crearUsuarioDTO;
        }

        // Método para actualizar un usuario
        public UsuarioDTO actualizarUsuario(int id, ActualizarUsuarioDTO actualizarUsuarioDTO)
        {
            // Obtener el usuario del repositorio
            var usuario = _repositorioUsuario.actualizarUsuario(id, actualizarUsuarioDTO);

            // Crear un actualizar el usuario
            var crearUsuarioDTO = new UsuarioDTO
            {
                IdMatricula = usuario.IdMatricula,
                NombreUsuario = usuario.NombreUsuario,
                ApellidoUsuario = usuario.ApellidoUsuario,
                CorreoInstitucional = usuario.CorreoInstitucional,
                ContrasenaHash = usuario.ContrasenaHash,
                Telefono = usuario.Telefono,
                Direccion = usuario.Direccion,
                IdRol = usuario.IdRol,
                FechaCreacion = usuario.FechaCreacion,
                FechaUltimaModificacion = usuario.FechaUltimaModificacion
            };

            // Devolver el nuevo usuario creado
            return crearUsuarioDTO;
        }

        // Método para eliminar un usuario
        public void eliminarUsuario(int id)
        {
            // Obtener el usuario del repositorio
            var usuario = _repositorioUsuario.obtenerUsuarioPorId(id);
            if (usuario == null)
            {
                throw new Exception("No se encontró el usuario");
            }
            // Eliminar el usuario
            _repositorioUsuario.eliminarUsuario(id);
        }
    }
}
