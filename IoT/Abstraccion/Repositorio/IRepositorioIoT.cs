using ERP.Data.Modelos;

namespace IoT.Abstraccion.Repositorio
{
    public interface IRepositorioIoT
    {
        //Creamos los metodos para  Eliminar y Optener
        //Task<List<Iot>?> GetIot();
        Task<Iot?> GetByIdIot(int id);
        Task<bool?> Eliminar(int id);
        Task<bool?> desactivarIoT(int id);
        Task<List<Iot>?> GetIot(int pagina, int tamanoPagina);
    }
}
