using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;

namespace Usuarios.Implementaciones.Repositorios
{
    /// <summary>
    /// Implementación del repositorio de Curriculum usando Entity Framework.
    /// </summary>
    public class CurriculumRepositorio : ICurriculumRepositorio
    {
        private readonly DbErpContext _context;

        public CurriculumRepositorio(DbErpContext context)
        {
            _context = context;
        }

        // ✅ Obtener todos los currículos
        public async Task<List<Curriculum>> ObtenerTodosAsync()
        {
            return await _context.Curriculums
                .Include(c => c.Anuncio)
                .AsNoTracking()
                .ToListAsync();
        }

        // ✅ Obtener un currículum por ID
        public async Task<Curriculum?> ObtenerPorIdAsync(int id)
        {
            return await _context.Curriculums
                .Include(c => c.Anuncio)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // ✅ Crear nuevo currículum
        public async Task CrearAsync(Curriculum curriculum)
        {
            await _context.Curriculums.AddAsync(curriculum);
        }

        // ✅ Actualizar un currículum existente
        public async Task ActualizarAsync(Curriculum curriculum)
        {
            _context.Curriculums.Update(curriculum);
            await Task.CompletedTask;
        }

        // ✅ Eliminar currículum
        public async Task<bool> EliminarAsync(int id)
        {
            var curriculum = await _context.Curriculums.FindAsync(id);
            if (curriculum == null) return false;

            _context.Curriculums.Remove(curriculum);
            return true;
        }

        // ✅ Guardar cambios
        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
