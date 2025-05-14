using IoT.Abstraccion.Repositorio;
using IoT.Modelos;
using Microsoft.EntityFrameworkCore;

namespace IoT.Implementaciones.Repositorios
{
    public class RepositorioIoT : IRepositorioIoT
    {

        private readonly DbErpContext _dbErpContext;
        public RepositorioIoT(DbErpContext dbErpContext)
        {
            _dbErpContext = dbErpContext;
        }

        public async Task<bool?> Eliminar(int id)
        {
            var ioT = await GetByIdIot(id);
            if (ioT == null)
            {
                return null;
            }
            _dbErpContext.Remove(ioT);
            await _dbErpContext.SaveChangesAsync();
            return true;
        }

        public async Task<Iot?> GetByIdIot(int id)
        {
            return await _dbErpContext.Iots.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Iot>?> GetIot()
        {
            return await _dbErpContext.Iots.ToListAsync();
        }
    }
}
