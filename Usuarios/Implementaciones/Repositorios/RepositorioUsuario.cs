using Microsoft.EntityFrameworkCore;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.DTO.LoginDTO;
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
        public async Task<List<Usuario>> obtenerUsuarios(int pagina, int tamanoPagina)
        {
            if (pagina <= 0) pagina = 1;
            if (tamanoPagina <= 0) tamanoPagina = 20;

            return await _context.Usuarios
                .Where(u => u.Activado == true)
                .OrderBy(i => i.Id)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();
        }

        //Método para obtener un usuario por su id
        public async Task<Usuario?> obtenerUsuarioPorId(int id)
        {
            // Verificar si el usuario existe
            var usuario = await _context.Usuarios
                .Include(usuario => usuario.PrestamosEquipoIdUsuarioAprobador)
                .Include(usuario => usuario.PrestamosEquipoIdUsuario)
                .Include(usuario => usuario.ReservaDeEspacioIdUsuarioAprobador)
                .Include(usuario => usuario.ReservaDeEspacioIdUsuario)
                .Include(usuario => usuario.SolicitudPrestamosDeEquipos)
                .Include(usuario => usuario.SolicitudReservaDeEspacios)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
            {
                return null;
            }
            // Devolver el usuario encontrado
            return usuario;
        }

        //Método para crear un usuario
        //public Usuario crearUsuario(UsuarioDTO usuarioDTO)
        //{
        //    // Verificar si el usuario ya existe
        //    var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.IdMatricula == usuarioDTO.IdMatricula);
        //    if (usuarioExistente != null)
        //    {
        //        throw new Exception("El usuario ya existe");
        //    }

        //    // Crear un nuevo usuario
        //    var usuario = new Usuario
        //    {
        //        IdMatricula = usuarioDTO.IdMatricula,
        //        NombreUsuario = usuarioDTO.NombreUsuario,
        //        ApellidoUsuario = usuarioDTO.ApellidoUsuario,
        //        CorreoInstitucional = usuarioDTO.CorreoInstitucional,
        //        ContrasenaHash = usuarioDTO.ContrasenaHash,
        //        Telefono = usuarioDTO.Telefono,
        //        Direccion = usuarioDTO.Direccion,
        //        IdRol = usuarioDTO.IdRol,
        //        FechaCreacion = DateTime.UtcNow,
        //        FechaUltimaModificacion = DateTime.UtcNow
        //    };

        //    // Guardar el nuevo usuario en la base de datos
        //    _context.Usuarios.Add(usuario);
        //    _context.SaveChanges();

        //    // Devolver el usuario creado
        //    return usuario;
        //}

        //Método para actualizar un usuario
        public async Task<Usuario?> actualizarUsuario(int id, ActualizarUsuarioDTO actualizarUsuarioDTO)
        {
            // Verificar si el usuario existe
            var usuarioExiste = await obtenerUsuarioPorId(id);

            // Si el usuario no existe, lanzar una excepción
            if (usuarioExiste == null)
            {
                return null;
            }

            //Creamos el hash de la contraseña
            string hash = BCrypt.Net.BCrypt.HashPassword(actualizarUsuarioDTO.ContrasenaHash);

            // Actualizar los campos del usuario, si tienen valores nuevos, si no, se deja el que ya tenia.
            usuarioExiste.IdMatricula = actualizarUsuarioDTO.IdMatricula ?? usuarioExiste.IdMatricula;
            usuarioExiste.NombreUsuario = actualizarUsuarioDTO.NombreUsuario ?? usuarioExiste.NombreUsuario;
            usuarioExiste.ApellidoUsuario = actualizarUsuarioDTO.ApellidoUsuario ?? usuarioExiste.ApellidoUsuario;
            usuarioExiste.CorreoInstitucional = actualizarUsuarioDTO.CorreoInstitucional ?? usuarioExiste.CorreoInstitucional;
            usuarioExiste.ContrasenaHash = hash ?? usuarioExiste.ContrasenaHash;
            usuarioExiste.Telefono = actualizarUsuarioDTO.Telefono ?? usuarioExiste.Telefono;
            usuarioExiste.Direccion = actualizarUsuarioDTO.Direccion ?? usuarioExiste.Direccion;
            usuarioExiste.IdRol = actualizarUsuarioDTO.IdRol ?? usuarioExiste.IdRol;
            usuarioExiste.FechaUltimaModificacion = DateTime.UtcNow;

            // Guardar los cambios en la base de datos
            _context.Update(usuarioExiste);
            _context.SaveChanges();

            // Devolver el usuario actualizado
            var usuarioActualizado = await obtenerUsuarioPorId(id);
            return usuarioActualizado;
        }

        //Método para eliminar un usuario
        public async Task<bool?> eliminarUsuario(int id)
        {
            // Verificar si el usuario existe
            var usuario = await obtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return null;
            }
            _context.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        //Método para desactivar un usuario
        public async Task<bool?> desactivarUsuario(int id)
        {
            // Verificar si el usuario existe
            var usuario = await obtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return null;
            }
            // Desactivar el usuario
            usuario.Activado = false;
            // Guardar los cambios en la base de datos
            _context.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
