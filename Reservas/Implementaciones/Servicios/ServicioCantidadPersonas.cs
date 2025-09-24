using ERP.Data.Modelos;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioCantidadPersonas
    {
        private readonly DbErpContext _context;

        public ServicioCantidadPersonas(DbErpContext context)
        {
            _context = context;
        }

        public async Task<Resultado<bool>> VerificarCantidad(int idLab, int capacidad)
        {
            var lab = await _context.Laboratorios.FindAsync(idLab);

            if(lab == null)
            {
                return Resultado<bool>.Falla("No se encontraron laboratorios con ese id");
            }

            if(lab.Capacidad < capacidad)
            {
                return Resultado<bool>.Falla("El espacio no permite esa cantidad de personas");
            }

            return Resultado<bool>.Exito(true);
        }
    }
}
