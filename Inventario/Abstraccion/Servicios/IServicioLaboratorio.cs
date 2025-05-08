using Inventario.DTO.LaboratorioDTO;
using Inventario.Modelos;

namespace Inventario.Abstraccion.Servicios
{
    public interface IServicioLaboratorio
    {
        //Creamos los metodos para Crear, actualizar, Eliminar y Optener
        List<LaboratorioDTO> GetLaboratorio();
        Laboratorio GetById(int id);
        LaboratorioDTO Crear(CrearLaboratorioDTO crearlaboratorioDTO);
        LaboratorioDTO Actualizar(int id,ActualizarLaboratorioDTO actualizarlaboratorioDTO);
        void Eliminar(int id);
    }
}
