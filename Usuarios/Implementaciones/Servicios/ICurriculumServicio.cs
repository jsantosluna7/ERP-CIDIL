using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Abstraccion.Servicios
{
 
    // Interfaz del servicio de Curriculum usando Resultado<T>

    public interface ICurriculumServicio
    {
        Task<Resultado<List<CurriculumDetalleDTO>>> ObtenerTodosAsync();
        Task<Resultado<CurriculumDetalleDTO?>> ObtenerPorIdAsync(int id);
        Task<Resultado<bool>> CrearAsync(CurriculumDTO dto);
        Task<Resultado<bool>> EliminarAsync(int id);
    }
}
