using ERP.Data;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;

namespace Usuarios.Implementaciones.Repositorios
{
    public class UsuarioPublicoRepositorio : IUsuarioPublicoRepositorio
    {
        private readonly DbErpContext _context;

        public UsuarioPublicoRepositorio(DbErpContext context)
        {
            _context = context;
        }

        public async Task<UsuarioPublico?> ObtenerPorIdAsync(int id)
        {
            return await _context.UsuarioPublicos.FindAsync(id);
        }

        public async Task<UsuarioPublico?> ObtenerPorCorreoAsync(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return null;

            return await _context.UsuarioPublicos
                .FirstOrDefaultAsync(u => u.Correo == correo);
        }

        public async Task CrearAsync(UsuarioPublico usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            await _context.UsuarioPublicos.AddAsync(usuario);
        }

        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
