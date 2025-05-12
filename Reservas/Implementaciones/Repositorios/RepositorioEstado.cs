using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;
using Reservas.Modelos;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioEstado : IRepositorioEstado
    {
        private readonly DbErpContext _context;

        public  RepositorioEstado(DbErpContext context)
        {
            _context = context;
        }

        public async Task<Estado?> GetById(int id)
        {
            var estado = await _context.Estados.Where(e => e.Id == id).FirstOrDefaultAsync();

            if (estado == null) 
            {
                return null;           
            }
            return estado;
        }

        public async Task<List<Estado>?> GetEstado()
        {
            return await _context.Estados.ToListAsync();
        }
    }
}
