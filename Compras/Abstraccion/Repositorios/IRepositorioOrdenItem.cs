using Compras.DTO.OrdenItemDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Repositorios
{
    public interface IRepositorioOrdenItem
    {
        Task<Resultado<OrdenItem>> ActualizarOrdenItem(int id, CrearOrdenItemDTO ordenDTO);
        Task<Resultado<OrdenItem>> CrearOrdenItem(CrearOrdenItemDTO orden);
        Task<Resultado<bool?>> Eliminar(int id);
        Task<Resultado<List<OrdenItem>>> OrdenItem();
        Task<Resultado<OrdenItem>> OrdenItemId(int id);
        Task<Resultado<List<OrdenItem>>> OrdenItemPorOrdenId(int ordenId);
    }
}