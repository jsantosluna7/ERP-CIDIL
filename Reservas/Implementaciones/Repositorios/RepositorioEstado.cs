using Reservas.Abstraccion.Repositorio;
using Reservas.Modelos;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioEstado : IRepositorioEstado
    {
        private readonly DbErpContext _context;

        public RepositorioEstado(DbErpContext context)
        {
            _context = context;
        }

        public Estado GetById(int id)
        {
            return _context.Estados.Where(e => e.Id == id).FirstOrDefault();
        }

        public List<Estado> GetEstado()
        {
            return [.._context.Estados];
        }
    }
}
