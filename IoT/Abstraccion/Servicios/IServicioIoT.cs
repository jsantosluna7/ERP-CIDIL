using ERP.Data.Modelos;
using IoT.DTO;

namespace IoT.Abstraccion.Servicios
{
    public interface IServicioIoT
    {
        //Creamos los metodos para  Eliminar y Optener
        //Task<List<IoTDTO>?> GetIot();
        Task<Iot?> GetByIdIoT(int id);
        Task<bool?> Eliminar(int id);
        Task<bool?> desactivarIoT(int id);
        Task<List<IoTDTO>?> GetIot(int pagina, int tamanoPagina);
    }
}
