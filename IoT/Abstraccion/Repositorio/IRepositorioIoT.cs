using IoT.Modelos;

namespace IoT.Abstraccion.Repositorio
{
    public interface IRepositorioIoT
    {
        Task<List<Iot>?> GetIot();
        Task<Iot?> GetByIdIot(int id);
        Task<bool?> Eliminar(int id);
    }
}
