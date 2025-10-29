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
        public async Task<Resultado<List<Curriculum>>> ObtenerTodosAsync()
        {
            var curriculums = await _context.Curriculums
                .AsNoTracking()
                .ToListAsync();

            if (curriculums == null || curriculums.Count == 0)
                return Resultado<List<Curriculum>>.Falla("No hay curriculums registrados.");

            return Resultado<List<Curriculum>>.Exito(curriculums);
        }

        // ✅ Obtener un currículum por ID
        public async Task<Resultado<Curriculum>> ObtenerPorIdAsync(int id)
        {
            var curriculum = await _context.Curriculums
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curriculum == null)
                return Resultado<Curriculum>.Falla($"No se encontró un currículum con Id = {id}.");

            return Resultado<Curriculum>.Exito(curriculum);
        }

        // ✅ Crear nuevo currículum
        public async Task<Resultado<bool>> CrearAsync(Curriculum curriculum)
        {
            await _context.Curriculums.AddAsync(curriculum);
            return Resultado<bool>.Exito(true);
        }

        // ✅ Actualizar un currículum existente
        public async Task<Resultado<bool>> ActualizarAsync(Curriculum curriculum)
        {
            _context.Curriculums.Update(curriculum);
            return Resultado<bool>.Exito(true);
        }

        // ✅ Eliminar currículum
        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            var curriculum = await _context.Curriculums.FindAsync(id);
            if (curriculum == null)
                return Resultado<bool>.Falla($"No se encontró un currículum con Id = {id}.");

            _context.Curriculums.Remove(curriculum);
            return Resultado<bool>.Exito(true);
        }

        // ✅ Guardar cambios
        public async Task<Resultado<bool>> GuardarAsync()
        {
            var cambios = await _context.SaveChangesAsync();
            return cambios > 0
                ? Resultado<bool>.Exito(true)
                : Resultado<bool>.Falla("No se pudieron guardar los cambios.");
        }
    }
}
