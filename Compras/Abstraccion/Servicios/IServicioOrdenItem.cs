using Compras.DTO.OrdenItemDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Servicios
{
    public interface IServicioOrdenItem
    {
        Task<Resultado<OrdenItemDTO>> ActualizarOrdenItem(int id, CrearOrdenItemDTO ordenItemsDTO);
        Task<Resultado<OrdenItemDTO>> CrearOrdenItem(CrearOrdenItemDTO itemsOrden);
        Task<Resultado<bool?>> Eliminar(int id);
        Task<Resultado<List<OrdenItem>>> OrdenItem();
        Task<Resultado<OrdenItem>> OrdenItemId(int id);
        Task<Resultado<List<OrdenItem>>> OrdenItemPorOrdenId(int ordenId);
    }
}