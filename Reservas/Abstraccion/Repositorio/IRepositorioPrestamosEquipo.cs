using ERP.Data.Modelos;
using Reservas.DTO.DTOPrestamosEquipo;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioPrestamosEquipo
    {
        Task<PrestamosEquipo> Actualizar(PrestamosEquipo prestamo);
        Task<ExtensionPrestamosEquipo> ActualizarExtension(ExtensionPrestamosEquipo extension);
        Task<int> ContarTodos();
        Task<PrestamosEquipo> Crear(PrestamosEquipo prestamo);
        Task<ExtensionPrestamosEquipo> CrearExtension(ExtensionPrestamosEquipo extension);
        Task Eliminar(PrestamosEquipo prestamo);
        Task GuardarCambios();
        Task<List<PrestamosEquipoDTO>> ObtenerActivos();
        Task<List<Usuario>> ObtenerAdmins();
        Task<List<PrestamosEquipoDTO>> ObtenerAtrasados();
        Task<ExtensionPrestamosEquipo?> ObtenerExtensionPorId(int id);
        Task<List<ExtensionPrestamosEquipo>> ObtenerExtensionsPendientes();
        Task<List<ExtensionPrestamosEquipo>> ObtenerExtensionsPorPrestamo(int idPrestamo);
        Task<InventarioEquipo?> ObtenerInventarioPorId(int id);
        Task<List<PrestamosEquipoDTO>> ObtenerPendientes();
        Task<PrestamosEquipo?> ObtenerPorId(int id);
        Task<List<PrestamosEquipoDTO>> ObtenerPorUsuario(int idUsuario);
        Task<List<PrestamosEquipoDTO>> ObtenerTodos(int pagina, int tamanoPagina);
        Task<Usuario?> ObtenerUsuarioPorId(int id);
    }
}
