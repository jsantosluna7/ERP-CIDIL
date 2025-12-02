using Compras.Abstraccion.Repositorios;
using Compras.DTO.OrdenesDTO;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Compras.Implementaciones.Repositorios
{
    public class RepositorioOrdenes : IRepositorioOrdenes
    {
        private readonly CasaosContext _context;

        public RepositorioOrdenes(CasaosContext context)
        {
            _context = context;
        }

        public async Task<Resultado<List<Ordene>>> OrdenesAll()
        {
            var resultado = await _context.Ordenes.ToListAsync();

            if(resultado == null || resultado.Count == 0)
            {
                return Resultado<List<Ordene>>.Falla("No se encontraron ordenes.");
            }

            return Resultado<List<Ordene>>.Exito(resultado);
        }

        public async Task<Resultado<Ordene>> ObtenerPorId(int id)
        {
            var ordenes = await _context.Ordenes.FirstOrDefaultAsync(o => o.Id == id);
            if(ordenes == null)
            {
                Resultado<Ordene>.Falla("No se encontró una orden con ese ID");
            }

            return Resultado<Ordene>.Exito(ordenes);
        }

        public async Task<Resultado<Ordene>> CrearOrdenes(OrdenesDTO ordene)
        {
            if (ordene == null)
            {
                Resultado<Ordene>.Falla("No se pueden dejar campos vacios.");
            }

            var existeOrden = await _context.Ordenes.FirstOrDefaultAsync(u => u.Codigo == ordene.Codigo);
            if (existeOrden == null)
            {
                Resultado<Ordene>.Falla("Ya existe una orden con el Código.");
            }

            var ordenes = new Ordene
            {
                Codigo = ordene.Codigo,
                Nombre = ordene.Nombre,
                Departamento = ordene.Departamento,
                UnidadNegocio = ordene.UnidadNegocio,
                SolicitadoPor = ordene.SolicitadoPor,
                FechaSolicitud = ordene.FechaSolicitud,
                FechaSubida = ordene.FechaSubida,
                Moneda = ordene.Moneda,
                ImporteTotal = ordene.ImporteTotal,
                Comentario = ordene.Comentario,
                EstadoTimelineId = ordene.EstadoTimelineId,
                CreadoPor = ordene.CreadoPor,
                ActualizadoEn = ordene.ActualizadoEn
            };

            _context.Ordenes.Add(ordenes);
            await _context.SaveChangesAsync();
            return Resultado<Ordene>.Exito(ordenes);
        }

        public async Task<Resultado<Ordene>> ActualizarOrdenes(int id, OrdenesDTO ordenesDTO)
        {
            var existeOrden = await ObtenerPorId(id);
            var orden = existeOrden.Valor;

            if (orden == null)
            {
                return Resultado<Ordene>.Falla(existeOrden.MensajeError);
            }

            var ordenes = new Ordene
            {
                Codigo = ordenesDTO.Codigo,
                Nombre = ordenesDTO.Nombre,
                Departamento = ordenesDTO.Departamento,
                UnidadNegocio = ordenesDTO.UnidadNegocio,
                SolicitadoPor = ordenesDTO.SolicitadoPor,
                FechaSolicitud = ordenesDTO.FechaSolicitud,
                FechaSubida = ordenesDTO.FechaSubida,
                Moneda = ordenesDTO.Moneda,
                ImporteTotal = ordenesDTO.ImporteTotal,
                Comentario = ordenesDTO.Comentario,
                EstadoTimelineId = ordenesDTO.EstadoTimelineId,
                CreadoPor = ordenesDTO.CreadoPor,
                ActualizadoEn = ordenesDTO.ActualizadoEn
            };

            _context.Update(ordenes);
            _context.SaveChanges();
            var ordenesActualizadas = await ObtenerPorId(id);
            var ordenesAct = ordenesActualizadas.Valor!;
            return Resultado<Ordene>.Exito(ordenesAct);
        }

        public async Task<Resultado<bool?>> Eliminar(int id)
        {
            var ordenesPorId = await ObtenerPorId(id);
            var ordenes = ordenesPorId.Valor!;

            if(ordenes == null)
            {
                return Resultado<bool?>.Falla(ordenesPorId.MensajeError);
            }

            _context.Remove(ordenesPorId);
            _context.SaveChanges();
            return Resultado<bool?>.Exito(true);
        }

        //Hacer la parte de desactivar una orden.
    }
}
