using ERP.Data.Modelos;
using Inventario.Abstraccion.Repositorio;
using Inventario.Abstraccion.Servicios;
using Inventario.DTO.InventarioEquipoDTO;
using Inventario.Implementaciones.Repositorios;
using System.Net.Http.Headers;

namespace Inventario.Implementaciones.Servicios
{
    public class ServicioInventarioEquipo : IServicioInventarioEquipo
    {
        //Hacemos una inyeccion de dependencia
        private readonly IRepositorioInventarioEquipo repositorioInventarioEquipo;
        private readonly HttpClient _httpClient;

        public ServicioInventarioEquipo(IRepositorioInventarioEquipo rInventarioEquipo, IHttpClientFactory httpClientFactory)
        {
            this.repositorioInventarioEquipo = rInventarioEquipo;
            _httpClient = httpClientFactory.CreateClient("ImageService");
        }

        //Usamos el metodo actualizar los componentes 
        public async Task<InventarioEquipoDTO?> Actualizar(int id, ActualizarInventarioEquipoDTO dto)
        {
            var equipoActual = await repositorioInventarioEquipo.GetById(id);
            if (equipoActual == null)
                return null;

            if (dto.Imagen != null)
            {

                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(dto.Imagen.OpenReadStream());

                streamContent.Headers.ContentType = new MediaTypeHeaderValue(dto.Imagen.ContentType);

                content.Add(streamContent, "file", dto.Imagen.FileName);

                var response = await _httpClient.PostAsync("api/Imagenes/upload/productos", content);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Error al subir imagen");

                var result = await response.Content.ReadFromJsonAsync<ImageResponse>();
                dto.ImagenEquipo = result?.url;
            }
            else
            {
                dto.ImagenEquipo = equipoActual.ImagenEquipo;
            }

            var actualizado = await repositorioInventarioEquipo.Actualizar(id, dto);
            return MapToDTO(actualizado);
        }


        //Usamos el metodo para crear el registro de los equipos
        public async Task<InventarioEquipoDTO?> Crear(CrearInventarioEquipoDTO dto)
        {
            string? imageUrl = null;

            if (dto.Imagen != null)
            {

                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(dto.Imagen.OpenReadStream());

                streamContent.Headers.ContentType = new MediaTypeHeaderValue(dto.Imagen.ContentType);

                content.Add(streamContent, "file", dto.Imagen.FileName);

                var response = await _httpClient.PostAsync("api/Imagenes/upload/productos", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error ImageService: {response.StatusCode} - {errorBody}");
                }

                var result = await response.Content.ReadFromJsonAsync<ImageResponse>();
                imageUrl = result?.url;
            }

            dto.ImagenEquipo = imageUrl;

            var invEquipo = await repositorioInventarioEquipo.Crear(dto);

            if (invEquipo == null)
                return null;

            return MapToDTO(invEquipo);
        }

        //Usamos el metodo para eliminar los registros
        public async Task<bool?> Eliminar(int id)
        {
            var r = await repositorioInventarioEquipo.Eliminar(id);
            if (r == null)
            {
                return null;
            }
            return r;
        }

        public async Task<bool?> DesactivarEquipo(int id)
        {
            var equipo = await repositorioInventarioEquipo.GetById(id);
            if (equipo == null)
            {
                return null;
            }
            equipo.Activado = false;
            await repositorioInventarioEquipo.DesactivarEquipo(id);
            return true;
        }
        //Metodo para llamar los equipos por ID
        public async Task<InventarioEquipo?> GetById(int id)
        {
            return await repositorioInventarioEquipo.GetById(id);
        }

        //Metodo para llamar todos los registros de los equipos 
        public async Task<List<InventarioEquipoDTO>?> GetInventarioEquipo(int pagina, int tamanoPagina)
        {
            var invEquipos = await repositorioInventarioEquipo.GetInventarioEquipos(pagina, tamanoPagina);
            if (invEquipos == null)
            {
                return null;
            }
            var invEquipoDTO = new List<InventarioEquipoDTO>();
            foreach (InventarioEquipo invEquipo in invEquipos)
            {
                var nuevoInvEquipo = new InventarioEquipoDTO
                {
                    Id = invEquipo.Id,
                    Nombre = invEquipo.Nombre,
                    NombreCorto = invEquipo.NombreCorto,
                    Perfil = invEquipo.Perfil,
                    IdLaboratorio = invEquipo.IdLaboratorio,
                    Fabricante = invEquipo.Fabricante,
                    Modelo = invEquipo.Modelo,
                    Serial = invEquipo.Serial,
                    DescripcionLarga = invEquipo.DescripcionLarga,
                    ImporteActivo = invEquipo.ImporteActivo,
                    ImagenEquipo = invEquipo.ImagenEquipo,
                    Disponible = invEquipo.Disponible,
                    IdEstadoFisico = invEquipo.IdEstadoFisico,
                    ValidacionPrestamo = invEquipo.ValidacionPrestamo,
                    FechaTransaccion = invEquipo.FechaTransaccion,
                    Departamento = invEquipo.Departamento,
                    Cantidad = invEquipo.Cantidad,
                    Activado = invEquipo.Activado,
                };
                invEquipoDTO.Add(nuevoInvEquipo);
            }
            return invEquipoDTO;
        }

        public async Task<Resultado<List<InventarioEquipo>>> BuscarPorNombre(string nombre)
        {
            var resultado = await repositorioInventarioEquipo.BuscarPorNombre(nombre);

            if (!resultado.esExitoso)
            {
                return Resultado<List<InventarioEquipo>>.Falla(resultado.MensajeError ?? "Error al buscar los equipos por nombre.");
            }

            var equipos = resultado.Valor!;

            return Resultado<List<InventarioEquipo>>.Exito(equipos);
        }

        private InventarioEquipoDTO MapToDTO(InventarioEquipo invEquipo)
        {
            return new InventarioEquipoDTO
            {
                Id = invEquipo.Id,
                Nombre = invEquipo.Nombre,
                NombreCorto = invEquipo.NombreCorto,
                Perfil = invEquipo.Perfil,
                IdLaboratorio = invEquipo.IdLaboratorio,
                Fabricante = invEquipo.Fabricante,
                Modelo = invEquipo.Modelo,
                Serial = invEquipo.Serial,
                DescripcionLarga = invEquipo.DescripcionLarga,
                ImporteActivo = invEquipo.ImporteActivo,
                ImagenEquipo = invEquipo.ImagenEquipo,
                Disponible = invEquipo.Disponible,
                IdEstadoFisico = invEquipo.IdEstadoFisico,
                ValidacionPrestamo = invEquipo.ValidacionPrestamo,
                FechaTransaccion = invEquipo.FechaTransaccion,
                Departamento = invEquipo.Departamento,
                Cantidad = invEquipo.Cantidad,
                Activado = invEquipo.Activado
            };
        }
    }
}
