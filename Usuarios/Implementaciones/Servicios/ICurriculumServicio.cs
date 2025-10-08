using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO; // ✅ Aquí están CurriculumDTO y CurriculumDetalleDTO

namespace Usuarios.Abstraccion.Servicios
{
    public interface ICurriculumServicio
    {
        Task<List<CurriculumDetalleDTO>> ObtenerTodosAsync();
        Task<CurriculumDetalleDTO?> ObtenerPorIdAsync(int id);
        Task CrearAsync(CurriculumDTO dto); // ✅ Ahora apunta al namespace correcto
        Task<bool> EliminarAsync(int id);
    }
}
