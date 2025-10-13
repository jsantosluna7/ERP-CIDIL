using ERP.Data.Modelos;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IUsuarioPublicoRepositorio
    {
        Task<UsuarioPublico?> ObtenerPorIdAsync(int id);
        Task<UsuarioPublico?> ObtenerPorCorreoAsync(string correo);
        Task CrearAsync(UsuarioPublico usuario);
        Task GuardarAsync();
    }
}
