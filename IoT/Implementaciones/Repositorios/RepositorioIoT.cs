using IoT.Abstraccion.Repositorio;
using IoT.Modelos;
using Microsoft.EntityFrameworkCore;

namespace IoT.Implementaciones.Repositorios
{
    public class RepositorioIoT : IRepositorioIoT
    {
        //se hace una inyeccion de dependencia
        private readonly DbErpContext _dbErpContext;
        public RepositorioIoT(DbErpContext dbErpContext)
        {
            _dbErpContext = dbErpContext;
        }

        //Se llama el metodo eliminar para eliminar los registros por ID
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

        //Método para desactivar un IoT
        public async Task<bool?> desactivarIoT(int id)
        {
            // Verificar si el IoT existe
            var IoT = await GetByIdIot(id);
            if (IoT == null)
            {
                return null;
            }
            // Desactivar el IoT
            IoT.Activado = false;
            // Guardar los cambios en la base de datos
            _dbErpContext.Update(IoT);
            await _dbErpContext.SaveChangesAsync();
            return true;
        }

        //Se usa el metodo Para llamar a todos los reguistros disponibles por ID
        public async Task<Iot?> GetByIdIot(int id)
        {
            return await _dbErpContext.Iots.Where(i => i.Id == id).FirstOrDefaultAsync();
        }


        //Se usa el metodo Para llamar a todos los reguistros disponibles 
        public async Task<List<Iot>?> GetIot(int pagina, int tamanoPagina)
        {
            if (pagina <= 0) pagina = 1;
            if (tamanoPagina <= 0) tamanoPagina = 20;

            return await _dbErpContext.Iots
                .Where(i => i.Activado == true)
                .OrderBy(i => i.Id)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();
        }
    }
}
