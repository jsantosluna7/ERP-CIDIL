using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioEstado : IRepositorioEstado
    {
        private readonly DbErpContext _context;

        public  RepositorioEstado(DbErpContext context)
        {
            _context = context;
        }

        public async Task<Resultado<Estado?>> GetById(int id)
        {
            var estado = await _context.Estados.Where(e => e.Id == id).FirstOrDefaultAsync();

            if (estado == null) 
            {
                return Resultado<Estado?>.Falla("El estado no existe o no se encontró.");           
            }
            return Resultado<Estado?>.Exito(estado);
        }

        public async Task<List<Estado>?> GetEstado()
        {
            return await _context.Estados.ToListAsync();
        }
    }
}
