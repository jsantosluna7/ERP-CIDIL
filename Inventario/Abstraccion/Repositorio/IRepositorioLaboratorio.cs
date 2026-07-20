using ERP.Data.Modelos;
using Inventario.DTO.LaboratorioDTO;

namespace Inventario.Abstraccion.Repositorio
{
    public interface IRepositorioLaboratorio
    {
        //Creamos los metodos para Crear, actualizar, Eliminar y Optener
        Task<List<Laboratorio>?> GetLaboratorio();
        Task<Laboratorio?> GetById(int id);
        Task<Laboratorio?> Crear(CrearLaboratorioDTO laboratorioDTO);
        Task<Laboratorio?> Actualizar(int id, ActualizarLaboratorioDTO actualizarLaboratorioDTO);
        Task<bool?> Eliminar(int id);
        Task<bool?> DesactivarLaboratorio(int id);
        Task<List<Laboratorio>?> GetPisos(int piso);
        Task<Laboratorio?> obtenerPorCodigo(string codigo);
    }
}
