using ERP.Data.Modelos;
using Publicaciones.Abstraccion.Repositorios;
using Publicaciones.Abstraccion.Servicios;
using Publicaciones.DTO.AnuncioDTO;

namespace Publicaciones.Implementaciones.Servicios
{
    public class ServicioAnuncio : IServicioAnuncio
    {
        private readonly IRepositorioAnuncio _repositorioAnuncio;

        public ServicioAnuncio(IRepositorioAnuncio repositorioAnuncio)
        {
            _repositorioAnuncio = repositorioAnuncio;
        }

        // Mapea un modelo Anuncio a su DTO de respuesta
        private static AnuncioDTO MapearDTO(Anuncio a) => new AnuncioDTO
        {
            Id = a.Id,
            Titulo = a.Titulo,
            Descripcion = a.Descripcion,
            ImagenUrl = a.ImagenUrl,
            FechaCreacion = a.FechaCreacion,
            FechaPublicacion = a.FechaPublicacion,
            EsPasantia = a.EsPasantia,
            EsCarrusel = a.EsCarrusel
        };

        public async Task<List<AnuncioDTO>?> GetAnuncios()
        {
            var anuncios = await _repositorioAnuncio.GetAnuncios();
            if (anuncios == null) return null;
            return anuncios.Select(MapearDTO).ToList();
        }

        public async Task<List<AnuncioDTO>?> GetCarrusel()
        {
            var anuncios = await _repositorioAnuncio.GetCarrusel();
            if (anuncios == null) return null;
            return anuncios.Select(MapearDTO).ToList();
        }

        public async Task<List<AnuncioDTO>?> GetPasantias()
        {
            var anuncios = await _repositorioAnuncio.GetPasantias();
            if (anuncios == null) return null;
            return anuncios.Select(MapearDTO).ToList();
        }

        public async Task<AnuncioDTO?> GetById(int id)
        {
            var anuncio = await _repositorioAnuncio.GetById(id);
            if (anuncio == null) return null;
            return MapearDTO(anuncio);
        }

        public async Task<AnuncioDTO?> Crear(CrearAnuncioDTO dto, int usuarioId)
        {
            var anuncio = await _repositorioAnuncio.Crear(dto, usuarioId);
            if (anuncio == null) return null;
            return MapearDTO(anuncio);
        }

        public async Task<AnuncioDTO?> Actualizar(int id, ActualizarAnuncioDTO dto)
        {
            var anuncio = await _repositorioAnuncio.Actualizar(id, dto);
            if (anuncio == null) return null;
            return MapearDTO(anuncio);
        }

        public async Task<bool?> Eliminar(int id)
        {
            return await _repositorioAnuncio.Eliminar(id);
        }
    }
}
