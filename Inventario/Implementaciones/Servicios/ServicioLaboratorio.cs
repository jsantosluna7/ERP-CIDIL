using ERP.Data.Modelos;
using Inventario.Abstraccion.Repositorio;
using Inventario.Abstraccion.Servicios;
using Inventario.DTO.LaboratorioDTO;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace Inventario.Implementaciones.Servicios
{
    public class ServicioLaboratorio : IServicioLaboratorio
    {
        //Hacemos una inyeccion de dependencia
        private readonly IRepositorioLaboratorio repositorioLaboratorio;
        private readonly HttpClient _httpClient;

        public ServicioLaboratorio(IRepositorioLaboratorio repositorio, IHttpClientFactory httpClientFactory)
        {
            repositorioLaboratorio = repositorio;
            _httpClient = httpClientFactory.CreateClient("ImageService");
        }   

        //Metodo para actualizar los laboratorios
        public async Task<LaboratorioDTO?> Actualizar(int id, ActualizarLaboratorioDTO dto)
        {
           var laboratorio = await repositorioLaboratorio.GetById(id);
            if (laboratorio == null)
            {
                return null;
            }

            if (dto.Imagen != null)
            {

                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(dto.Imagen.OpenReadStream());

                streamContent.Headers.ContentType = new MediaTypeHeaderValue(dto.Imagen.ContentType);

                content.Add(streamContent, "file", dto.Imagen.FileName);

                var response = await _httpClient.PostAsync("api/Imagenes/upload/laboratorios", content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error API Imagenes: {response.StatusCode} - {error}");
                }

                var result = await response.Content.ReadFromJsonAsync<ImageResponse>();
                dto.ImagenLaboratorio = result?.url;
            }
            else
            {
                dto.ImagenLaboratorio = laboratorio.ImagenLaboratorio;
            }

            var actualizado = await repositorioLaboratorio.Actualizar(id, dto);

            var laboratorioDTO = new LaboratorioDTO
            {
                Id = actualizado.Id,
                CodigoDeLab = actualizado.CodigoDeLab,
                Capacidad = actualizado.Capacidad,
                Descripcion = actualizado.Descripcion,
                ImagenLaboratorio = actualizado.ImagenLaboratorio,
                Nombre = actualizado.Nombre,
                Piso = actualizado.Piso

                
            };
            return laboratorioDTO;
        }

        //Metodo para crear los espcacios de los laboratorios
        public async Task<LaboratorioDTO?> Crear(CrearLaboratorioDTO dto)
        {
            string? imageUrl = null;

            if (dto.Imagen != null)
            {

                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(dto.Imagen.OpenReadStream());

                streamContent.Headers.ContentType = new MediaTypeHeaderValue(dto.Imagen.ContentType);

                content.Add(streamContent, "file", dto.Imagen.FileName);

                var response = await _httpClient.PostAsync("api/Imagenes/upload/laboratorios", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error ImageService: {response.StatusCode} - {errorBody}");
                }

                var result = await response.Content.ReadFromJsonAsync<ImageResponse>();
                imageUrl = result?.url;
            }

            dto.ImagenLaboratorio = imageUrl;

            var laboratorio = await repositorioLaboratorio.Crear(dto);

            if (laboratorio == null)
            {
                return null;
            }
            var laboratorioDTO = new LaboratorioDTO
            {
                Id = laboratorio.Id,
                CodigoDeLab = laboratorio .CodigoDeLab,
                Capacidad = laboratorio.Capacidad,
                Descripcion = laboratorio.Descripcion,
                Nombre= laboratorio.Nombre,
                Piso = laboratorio.Piso,
                ImagenLaboratorio = laboratorio.ImagenLaboratorio
            };
            return laboratorioDTO;
        }

        //Metodo para eliminar el reguistro de los laboratotio
        public async Task<bool?>  Eliminar(int id)
        {
           var r = await repositorioLaboratorio.Eliminar(id);
            if (r == null)
            {
                return null;
            }
            return r;
        }

        public async Task<bool?> DesactivarLaboratorio(int id)
        {
            var laboratorio = await repositorioLaboratorio.GetById(id);
            if (laboratorio == null)
            {
                return null;
            }
            laboratorio.Activado = false;
            await repositorioLaboratorio.DesactivarLaboratorio(id);
            return true;
        }

        //Metodo para optener los laboratorios por ID
        public async Task<Laboratorio?> GetById(int id)
        {
            var lab = await repositorioLaboratorio.GetById(id);
            return new Laboratorio
            {
                Id = lab.Id,
                CodigoDeLab = lab.CodigoDeLab,
                Capacidad = lab.Capacidad,
                Descripcion = lab.Descripcion,
                Nombre = lab.Nombre,
                Piso = lab.Piso,
                ImagenLaboratorio = lab.ImagenLaboratorio
            };
        }

        // Metodo para llamar todos los registros de los laboratorios
        public async Task<List<LaboratorioDTO>?> GetLaboratorio()
        {
            var laboratorio =await repositorioLaboratorio.GetLaboratorio();
            if (laboratorio == null)
            {
                return null ;
            }
            var laboratorioDTO = new List<LaboratorioDTO>();
            foreach(Laboratorio laboratorio1 in laboratorio)
            {
                var nuevolaboratorioDTO = new LaboratorioDTO
                {   Id = laboratorio1.Id,
                    CodigoDeLab = laboratorio1 .CodigoDeLab,
                    Capacidad = laboratorio1.Capacidad,
                    Nombre = laboratorio1.Nombre,
                    Piso = laboratorio1.Piso,
                    Descripcion = laboratorio1.Descripcion,
                    ImagenLaboratorio = laboratorio1.ImagenLaboratorio

                };
                laboratorioDTO.Add(nuevolaboratorioDTO);
            }
            return laboratorioDTO;
        }

        //Se optienen los registros por ID de los Pisos
        public async Task<List<LaboratorioDTO>?> GetPisos(int piso)
        {

            var p = await repositorioLaboratorio.GetPisos(piso);
            if(p == null)
            {
                return null;
            }
            var pDTO = new List<LaboratorioDTO>();
            foreach(Laboratorio pisos in p)
            {
                var nuevopDTO = new LaboratorioDTO
                {
                    Id = pisos.Id,
                    CodigoDeLab = pisos.CodigoDeLab,
                    Capacidad = pisos.Capacidad,
                    Descripcion = pisos.Descripcion,
                    Piso = pisos.Piso,
                    Nombre = pisos.Nombre,
                    ImagenLaboratorio = pisos.ImagenLaboratorio
                };
                pDTO.Add(nuevopDTO);
            }
            return pDTO;
            
        }

        public async Task<LaboratorioIdDTO?> obtenerPorCodigo(string codigo)
        {
            var codigoLab = await repositorioLaboratorio.obtenerPorCodigo(codigo);
            return new LaboratorioIdDTO
            {
                Id = codigoLab.Id
            };

        }
    }
}
