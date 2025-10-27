using ERP.Data;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;

namespace Usuarios.Implementaciones.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly DbErpContext _context;

        public UsuarioRepositorio(DbErpContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.CorreoInstitucional.ToLower() == correo.ToLower());
        }

        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
