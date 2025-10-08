using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;

namespace Usuarios.Implementaciones.Repositorios
{
    /// <summary>
    /// Implementación del repositorio de Curriculum usando Entity Framework
    /// </summary>
    public class CurriculumRepositorio : ICurriculumRepositorio
    {
        private readonly DbErpContext _context;

        public CurriculumRepositorio(DbErpContext context)
        {
            _context = context;
        }

        public async Task<List<Curriculum>> ObtenerTodosAsync()
        {
            return await _context.Curriculums
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Curriculum?> ObtenerPorIdAsync(int id)
        {
            return await _context.Curriculums
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CrearAsync(Curriculum curriculum)
        {
            await _context.Curriculums.AddAsync(curriculum);
        }

        public async Task ActualizarAsync(Curriculum curriculum)
        {
            _context.Curriculums.Update(curriculum);
            await Task.CompletedTask;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var curriculum = await _context.Curriculums.FindAsync(id);
            if (curriculum == null) return false;

            _context.Curriculums.Remove(curriculum);
            return true;
        }

        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
