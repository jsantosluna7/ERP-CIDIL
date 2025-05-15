using Inventario.DTO.LaboratorioDTO;
using Inventario.Modelos;

namespace Inventario.Abstraccion.Servicios
{
    public interface IServicioLaboratorio
    {
        //Creamos los metodos para Crear, actualizar, Eliminar y Optener
        Task<List<LaboratorioDTO>?> GetLaboratorio();
        Task<Laboratorio?> GetById(int id);
        Task<LaboratorioDTO?> Crear(CrearLaboratorioDTO crearlaboratorioDTO);
        Task<LaboratorioDTO?> Actualizar(int id,ActualizarLaboratorioDTO actualizarlaboratorioDTO);
        Task<bool?> Eliminar(int id);
        Task<bool?> DesactivarLaboratorio(int id);
        Task<List<LaboratorioDTO>?> GetPisos(int piso);
    }
}
