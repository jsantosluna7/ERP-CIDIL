using ERP.Data.Modelos;

namespace Compras.Abstraccion.Repositorios
{
    public interface IRepositorioEspecializado
    {
        Task GuardarCambios();
        void InsertarTimeline(OrdenTimeline timeline);
        Task<OrdenItem?> ObtenerItemPorId(int itemId);
        Task<List<OrdenItem>> ObtenerItemsPorOrden(int ordenId);
        Task<Ordene?> ObtenerOrdenPorId(int ordenId);
        Task<List<OrdenTimeline>> ObtenerTimeline(int ordenId);
    }
}