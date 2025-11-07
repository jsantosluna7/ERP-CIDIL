using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO;

namespace Usuarios.Abstraccion.Repositorios
{
    
    // Interfaz para manejar la persistencia de Curriculum
    // usando el patrón Resultado<T> para control de errores.
   
    public interface ICurriculumRepositorio
    {
        // Obtiene todos los curriculums
      
        Task<Resultado<List<Curriculum>>> ObtenerTodosAsync();

        
        // Obtiene un curriculum por Id
        
        Task<Resultado<Curriculum>> ObtenerPorIdAsync(int id);

     
        /// Crea un nuevo curriculum
  
        Task<Resultado<bool>> CrearAsync(Curriculum curriculum);

     
        /// Actualiza un curriculum existente
      
        Task<Resultado<bool>> ActualizarAsync(Curriculum curriculum);

     
        // Elimina un curriculum por Id
     
        Task<Resultado<bool>> EliminarAsync(int id);

        
        // Guarda los cambios pendientes en la base de datos
       
        Task<Resultado<bool>> GuardarAsync();
    }
}
