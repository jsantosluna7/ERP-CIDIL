using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IAnuncioRepositorio
    {
        // ✅ Obtener todos los anuncios
        Task<List<Anuncio>> ObtenerTodosAsync();

        // ✅ Obtener un anuncio por ID
        Task<Anuncio?> ObtenerPorIdAsync(int id);

        // ✅ Crear un nuevo anuncio
        Task CrearAsync(Anuncio anuncio);

        // ✅ Actualizar un anuncio existente
        void Actualizar(Anuncio anuncio);

        // ✅ Eliminar un anuncio
        void Eliminar(Anuncio anuncio);

        // ✅ Guardar cambios en la base de datos
        Task GuardarAsync();
    }
}
