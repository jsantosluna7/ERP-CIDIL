using Usuarios.Abstraccion.Repositorios;
using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Implementaciones.Repositorios
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly DbErpContext _context;

        public RepositorioUsuario(DbErpContext context)
        {
            _context = context;
        }

        //Método para obtener todo los usuarios
        public List<Usuario> obtenerUsuarios()
        {
            return [.. _context.Usuarios];
        }

        //Método para obtener un usuario por su id
        public Usuario? obtenerUsuarioPorId(int id)
        {
            // Verificar si el usuario existe
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return null;
            }
            // Devolver el usuario encontrado
            return usuario;
        }

        //Método para crear un usuario
        public Usuario crearUsuario(UsuarioDTO usuarioDTO)
        {
            // Verificar si el usuario ya existe
            var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.IdMatricula == usuarioDTO.IdMatricula);
            if (usuarioExistente != null)
            {
                throw new Exception("El usuario ya existe");
            }

            // Crear un nuevo usuario
            var usuario = new Usuario
            {
                IdMatricula = usuarioDTO.IdMatricula,
                NombreUsuario = usuarioDTO.NombreUsuario,
                ApellidoUsuario = usuarioDTO.ApellidoUsuario,
                CorreoInstitucional = usuarioDTO.CorreoInstitucional,
                ContrasenaHash = usuarioDTO.ContrasenaHash,
                Telefono = usuarioDTO.Telefono,
                Direccion = usuarioDTO.Direccion,
                IdRol = usuarioDTO.IdRol,
                FechaCreacion = DateTime.UtcNow,
                FechaUltimaModificacion = DateTime.UtcNow
            };

            // Guardar el nuevo usuario en la base de datos
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            // Devolver el usuario creado
            return usuario;
        }

        //Método para actualizar un usuario
        public Usuario actualizarUsuario(int id, ActualizarUsuarioDTO actualizarUsuarioDTO)
        {
            // Verificar si el usuario existe
            var usuarioExiste = obtenerUsuarioPorId(id);

            // Si el usuario no existe, lanzar una excepción
            if (usuarioExiste == null)
            {
                throw new Exception("El usuario no existe");
            }

            // Actualizar los campos del usuario, si tienen valores nuevos, si no, se deja el que ya tenia.
            usuarioExiste.IdMatricula = actualizarUsuarioDTO.IdMatricula ?? usuarioExiste.IdMatricula;
            usuarioExiste.NombreUsuario = actualizarUsuarioDTO.NombreUsuario ?? usuarioExiste.NombreUsuario;
            usuarioExiste.ApellidoUsuario = actualizarUsuarioDTO.ApellidoUsuario ?? usuarioExiste.ApellidoUsuario;
            usuarioExiste.CorreoInstitucional = actualizarUsuarioDTO.CorreoInstitucional ?? usuarioExiste.CorreoInstitucional;
            usuarioExiste.ContrasenaHash = actualizarUsuarioDTO.ContrasenaHash ?? usuarioExiste.ContrasenaHash;
            usuarioExiste.Telefono = actualizarUsuarioDTO.Telefono ?? usuarioExiste.Telefono;
            usuarioExiste.Direccion = actualizarUsuarioDTO.Direccion ?? usuarioExiste.Direccion;
            usuarioExiste.IdRol = actualizarUsuarioDTO.IdRol ?? usuarioExiste.IdRol;
            usuarioExiste.FechaUltimaModificacion = DateTime.UtcNow;

            // Guardar los cambios en la base de datos
            _context.Update(usuarioExiste);
            _context.SaveChanges();

            // Devolver el usuario actualizado
            var usuarioActualizado = obtenerUsuarioPorId(id);
            return usuarioActualizado;
        }

        //Método para eliminar un usuario
        public void eliminarUsuario(int id)
        {
            // Verificar si el usuario existe
            var usuario = obtenerUsuarioPorId(id);
            if (usuario != null)
            {
                _context.Remove(usuario);
                _context.SaveChanges();
            }
        }

    }
}
