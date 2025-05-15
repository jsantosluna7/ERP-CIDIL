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
        public async Task<List<UsuarioDTO>?> ObtenerUsuarios()
        {
            // Obtener todos los usuarios
            var usuarios = await _repositorioUsuario.obtenerUsuarios();
            if (usuarios == null || usuarios.Count == 0)
            {
                return null;
            }

            //Inicializar la lista de usuarios DTO
            var usuariosDTO = new List<UsuarioDTO>();

            // Mapear los usuarios a DTOs
            foreach (Usuario usuario in usuarios)
            {
                var usuarioDTO = new UsuarioDTO
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
                    FechaUltimaModificacion = usuario.FechaUltimaModificacion,
                    UltimaSesion = usuario.UltimaSesion
                };
                usuariosDTO.Add(usuarioDTO);
            }

            // Devolver la lista de usuarios DTO
            return usuariosDTO;
        }

        // Método para obtener un usuario por su id
        public async Task<Usuario?> ObtenerUsuarioPorId(int id)
        {
            // Obtener el usuario por su id
            var usuario = await _repositorioUsuario.obtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return null;
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
        //public UsuarioDTO crearUsuario(UsuarioDTO usuarioDTO)
        //{
        //    // Obtener el usuario del repositorio
        //    var usuario = _repositorioUsuario.crearUsuario(usuarioDTO);

        //    // Crear un nuevo usuario
        //    var crearUsuarioDTO = new UsuarioDTO
        //    {
        //        IdMatricula = usuario.IdMatricula,
        //        NombreUsuario = usuario.NombreUsuario,
        //        ApellidoUsuario = usuario.ApellidoUsuario,
        //        CorreoInstitucional = usuario.CorreoInstitucional,
        //        ContrasenaHash = usuario.ContrasenaHash,
        //        Telefono = usuario.Telefono,
        //        Direccion = usuario.Direccion,
        //        IdRol = usuario.IdRol,
        //        FechaCreacion = usuario.FechaCreacion,
        //        FechaUltimaModificacion = usuario.FechaUltimaModificacion
        //    };

        //    // Devolver el nuevo usuario creado
        //    return crearUsuarioDTO;
        //}

        // Método para actualizar un usuario
        public async Task<ActualizarUsuarioDTO?> actualizarUsuario(int id, ActualizarUsuarioDTO actualizarUsuarioDTO)
        {
            // Obtener el usuario del repositorio
            var usuario = await _repositorioUsuario.actualizarUsuario(id, actualizarUsuarioDTO);

            if (usuario == null)
            {
                return null;
            }

            // Crear un actualizar el usuario
            var crearUsuarioDTO = new ActualizarUsuarioDTO
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
        public async Task<bool?> eliminarUsuario(int id)
        {
            // Obtener el usuario del repositorio
            var usuario = await _repositorioUsuario.obtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return null;
            }
            // Eliminar el usuario
            await _repositorioUsuario.eliminarUsuario(id);
            return true;
        }

        // Método para desactivar un usuario
        public async Task<bool?> desactivarUsuario(int id)
        {
            // Obtener el usuario del repositorio
            var usuario = await _repositorioUsuario.obtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return null;
            }
            // Desactivar el usuario
            usuario.Activado = false;
            await _repositorioUsuario.desactivarUsuario(id);
            return true;
        }
    }
}
