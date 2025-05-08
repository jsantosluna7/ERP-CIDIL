using Inventario.DTO.LaboratorioDTO;
using Inventario.Modelos;

namespace Inventario.Abstraccion.Repositorio
{
    public interface IRepositorioLaboratorio
    {
        //Creamos los metodos para Crear, actualizar, Eliminar y Optener
        List<Laboratorio> GetLaboratorio();
        Laboratorio GetById(int id);
        Laboratorio Crear(CrearLaboratorioDTO laboratorioDTO);
        Laboratorio Actualizar(int id, ActualizarLaboratorioDTO actualizarLaboratorioDTO);
        void Eliminar(int id);
    }
}
