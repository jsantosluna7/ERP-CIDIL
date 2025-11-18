using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Abstraccion.Servicios
{
    // Interfaz del servicio de gestión de currículums
    public interface IServicioCurriculum
    {
        // Obtiene todos los currículums registrados
        Task<Resultado<List<CurriculumDetalleDTO>>> ObtenerTodosAsync();

        // Obtiene un currículum por su Id
        Task<Resultado<CurriculumDetalleDTO?>> ObtenerPorIdAsync(int id);

        // Crea un currículum de un usuario autenticado
        Task<Resultado<bool>> CrearAsync(CurriculumDTO dto);

        // Crea un currículum desde un usuario externo (sin autenticación)
        Task<Resultado<bool>> CrearExternoAsync(CurriculumDTO dto);

        // Elimina un currículum del sistema
        Task<Resultado<bool>> EliminarAsync(int id);
    }
}