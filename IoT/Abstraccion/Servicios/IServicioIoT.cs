using IoT.DTO;
using IoT.Modelos;

namespace IoT.Abstraccion.Servicios
{
    public interface IServicioIoT
    {
        Task<List<IoTDTO>?> GetIot();
        Task<Iot?> GetByIdIoT(int id);
        Task<bool?> Eliminar(int id);
    }
}
