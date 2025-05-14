using Inventario.Abstraccion.Repositorio;
using Inventario.DTO.LaboratorioDTO;
using Inventario.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Implementaciones.Repositorios
{
    public class RepositorioLaboratorio : IRepositorioLaboratorio
    {
        // Se hace una inyeccion de dependencia
        private readonly DbErpContext _context;

        public RepositorioLaboratorio(DbErpContext context)
        {
            _context = context;
        }

        // se utliliza el metodo para actualizar los laboratorios
        
        public async Task<Laboratorio?> Actualizar(int id, ActualizarLaboratorioDTO actualizarLaboratorioDTO)
        {
          var laboratorioExistente = await GetById(id);
            if (laboratorioExistente == null)
            {
                return null;
            };


            laboratorioExistente.CodigoDeLab= actualizarLaboratorioDTO.CodigoDeLab;
            laboratorioExistente.Capacidad = actualizarLaboratorioDTO.Capacidad;
            laboratorioExistente.Descripcion= actualizarLaboratorioDTO.Descripcion;

             _context.Update(laboratorioExistente);
            await _context.SaveChangesAsync();
            var laboratorioActualizado = await GetById(id);
            return laboratorioActualizado;
        }

        // Se utiliza el metodo Para crear los laboratorios
        public async Task<Laboratorio?> Crear(CrearLaboratorioDTO crearlaboratorioDTO)
        {
            var laboratorio = new Laboratorio
            {
                CodigoDeLab = crearlaboratorioDTO.CodigoDeLab,
                Capacidad = crearlaboratorioDTO.Capacidad,
                Descripcion = crearlaboratorioDTO.Descripcion,
            };
            _context.Laboratorios.Add(laboratorio);
            await _context.SaveChangesAsync();
            return laboratorio;
        }


        //Se utiliza el metodo para eliminar el registro por ID
        public async Task<bool?> Eliminar(int id)
        {
          var laboratorio =await GetById(id);
            if (laboratorio == null)
            {
                return null;
            }
            _context.Remove(laboratorio);
           await _context.SaveChangesAsync();
            return true;
        }

        //Se optienen los registros por ID
        public async Task<Laboratorio?> GetById(int id)
        {
            return await _context.Laboratorios
                .Include(h => h.Horarios)
                .Include(i => i.InventarioEquipos)
                .Include(i => i.Iots)
                .Include(i => i.ReservaDeEspacios)
                .Include(i => i.SolicitudReservaDeEspacios)
                .Where(l => l.Id == id).FirstOrDefaultAsync();
        }


        //Se optienen todos los registros
        public async Task<List<Laboratorio>?> GetLaboratorio()
        {
           return await _context.Laboratorios.ToListAsync();
        }
    }
}
