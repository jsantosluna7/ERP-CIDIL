using Compras.Abstraccion.Repositorios;
using Compras.Abstraccion.Servicios;
using Compras.DTO.EspecializadosDTO;
using Compras.DTO.PdfExtractionDTO;
using ERP.Data.Modelos;
using System.Globalization;
using System.Text.Json;

namespace Compras.Implementaciones.Servicios
{
    public class ServicioEspecializado : IServicioEspecializado
    {
        private readonly IRepositorioEspecializado _repositorioEspecializado;
        private readonly DbErpContext _context;
        public ServicioEspecializado(IRepositorioEspecializado repositorioEspecializado, DbErpContext context)
        {
            _repositorioEspecializado = repositorioEspecializado;
            _context = context;
        }

        public async Task<Resultado<object>> ActualizarEstadoOrden(int ordenId, ActualizarEstadoOrdenDTO actualizarEstadoOrdenDTO)
        {
            var orden = await _repositorioEspecializado.ObtenerOrdenPorId(ordenId);
            if (orden == null)
            {
                return Resultado<object>.Falla("Orden no encontrada.");
            }

            orden.EstadoTimelineId = actualizarEstadoOrdenDTO.EstadoTimelineId;
            orden.ActualizadoEn = DateTime.Now;

            _repositorioEspecializado.InsertarTimeline(new OrdenTimeline
            {
                OrdenId = ordenId,
                EstadoTimelineId = actualizarEstadoOrdenDTO.EstadoTimelineId,
                Evento = actualizarEstadoOrdenDTO.Evento ?? "Cambio Manual",
                FechaEvento = DateTime.Now,
                CreadoPor = actualizarEstadoOrdenDTO.UsuarioId
            });

            await _repositorioEspecializado.GuardarCambios();

            return Resultado<object>.Exito(new
            {
                estadoActual = actualizarEstadoOrdenDTO.EstadoTimelineId
            });
        }

        public async Task<Resultado<object>> ActualizarItemRecepcion(int itemId, ActualizarItemRecepcionDTO actualizarItemRecepcionDTO)
        {
            var item = await _repositorioEspecializado.ObtenerItemPorId(itemId);
            if (item == null)
            {
                return Resultado<object>.Falla("Item no encontrado.");
            }

            item.CantidadRecibida = actualizarItemRecepcionDTO.CantidadRecibida;

            if (actualizarItemRecepcionDTO.CantidadRecibida == 0)
            {
                item.EstadoTimelineId = 1; // Registrado
            }
            else if (actualizarItemRecepcionDTO.CantidadRecibida < item.Cantidad)
            {
                item.EstadoTimelineId = 6; // Parcialmente Recibido
            }
            else
            {
                item.EstadoTimelineId = 7; // Recibido 
            }

            await _repositorioEspecializado.GuardarCambios();

            var recalculado = await RecalcularEstadoOrden(item.OrdenId, actualizarItemRecepcionDTO.UsuarioId);

            return Resultado<object>.Exito(new
            {
                item = new
                {
                    item.Id,
                    item.CantidadRecibida,
                    item.EstadoTimelineId
                },
                orden = recalculado.Valor!
            });
        }

        public async Task<Resultado<object>> RecalcularEstadoOrden(int ordenId, int usuarioId)
        {
            var items = await _repositorioEspecializado.ObtenerItemsPorOrden(ordenId);
            var orden = await _repositorioEspecializado.ObtenerOrdenPorId(ordenId);

            if (orden == null)
            {
                return Resultado<object>.Falla("Orden no encontrada.");
            }

            int total = items.Count;
            int recibidos = items.Count(items => items.EstadoTimelineId == 7);
            int parciales = items.Count(items => items.EstadoTimelineId == 6);

            int nuevoEstadoId;

            if (recibidos == total)
            {
                nuevoEstadoId = 8; // Completamente Recibido
            }
            else if (parciales > 0)
            {
                nuevoEstadoId = 6; // Parcialmente Recibido
            }
            else
            {
                nuevoEstadoId = 1; // Registrado
            }

            if (orden.EstadoTimelineId != nuevoEstadoId)
            {
                await ActualizarEstadoOrden(ordenId, new ActualizarEstadoOrdenDTO
                {
                    EstadoTimelineId = nuevoEstadoId,
                    Evento = "Actualización automática por ítems",
                    UsuarioId = usuarioId
                });
            }

            return Resultado<object>.Exito(new
            {
                estadoActual = nuevoEstadoId
            });
        }

        public async Task<Resultado<List<TimelineDTO>>> ObtenerTimeline(int ordenId)
        {
            var timeline = await _repositorioEspecializado.ObtenerTimeline(ordenId);

            var resultado = timeline.Select(t => new TimelineDTO
            {
                FechaEvento = t.FechaEvento,
                Evento = t.Evento,
                EstadoTimelineId = t.EstadoTimelineId,
                Usuario = t.CreadoPorNavigation != null ? $"{t.CreadoPorNavigation.NombreUsuario} {t.CreadoPorNavigation.ApellidoUsuario}" : "Desconocido"
            }).ToList();

            return Resultado<List<TimelineDTO>>.Exito(resultado);
        }

        public async Task<Resultado<List<OrdenItem>>> ObtenerItems(int ordenId)
        {
            var items = await _repositorioEspecializado.ObtenerItemsPorOrden(ordenId);
            return Resultado<List<OrdenItem>>.Exito(items);
        }

        public async Task<Resultado<object>> PdfExtraction(IFormFile file, int usuarioId)
        {
            if(file == null || file.Length == 0)
            {
                return Resultado<object>.Falla("Debe adjuntar un PDF");
            }

            //Enviar PDF al API de Python
            using var client = new HttpClient();
            using var form = new MultipartFormDataContent();

            var stream = file.OpenReadStream();
            form.Add(new StreamContent(stream), "file", file.FileName);

            var response = await client.PostAsync(
                "https://pdf.cidilipl.online/extract-requisition",
                form);

            if (!response.IsSuccessStatusCode)
            {
                return Resultado<object>.Falla("Error al procesar el PDF");
                throw new Exception("Error al procesar el PDF");
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<RequisicionDTO>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            string[] formatos = {
                "M/d/yy",
                "MM/dd/yy",
                "M/d/yyyy",
                "MM/dd/yyyy",
                "dd/MM/yyyy",
                "d/M/yy"
            };
            
            if (!DateOnly.TryParseExact(data.entered_date.Trim(), formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fechaSolicitud))
            {
                throw new Exception($"Formato de fecha inválido: {data.entered_date}");
            }


            //Crear la orden
            var orden = new Ordene
            {
                Codigo = data.requisition_id,
                Nombre = data.requisition_name,
                UnidadNegocio = data.business_unit,
                SolicitadoPor = data.requested_by,
                ItemsCount = data.items_count,
                Comentario = data.header_comments,
                FechaSolicitud = fechaSolicitud,
                EstadoTimelineId = 1, // Registrado
                CreadoPor = usuarioId,
                FechaSubida = DateOnly.FromDateTime(DateTime.Now),
                Departamento = "Compras"
            };

            _context.Ordenes.Add(orden);
            await _context.SaveChangesAsync();

            //Crear los items
            foreach (var linea in data.lines)
            {
                var item = new OrdenItem
                {
                    Nombre = linea.item_description,
                    OrdenId = orden.Id,
                    NumeroLista = linea.line_number.ToString(),
                    Cantidad = ((int)linea.quantity),
                    CantidadRecibida = 0,
                    Comentario = linea.line_comments,
                    EstadoTimelineId = 1, // Registrado
                };

                _context.OrdenItems.Add(item);
            }

            await _context.SaveChangesAsync();

            //Timeline inicial
            _context.OrdenTimelines.Add(new OrdenTimeline
            {
                OrdenId = orden.Id,
                EstadoTimelineId = 1,
                Evento = "Orden creada desde extracción de PDF",
                FechaEvento = DateTime.Now,
                CreadoPor = usuarioId
            });

            await _context.SaveChangesAsync();

            //Respuesta al frontend
            return Resultado<object>.Exito(new
            {
                ordenId = orden.Id,
                ordenCodigo = orden.Codigo,
                items = data.lines.Count
            });
        }

        public async Task<Resultado<int>> CantidadDeOrdenes()
        {
            var resultado = await _repositorioEspecializado.CantidadDeOrdenes();
            if (resultado <= 0)
            {
                return Resultado<int>.Falla("No se pudo obtener la cantidad de órdenes.");
            }

            return Resultado<int>.Exito(resultado);
        }
    }
}
