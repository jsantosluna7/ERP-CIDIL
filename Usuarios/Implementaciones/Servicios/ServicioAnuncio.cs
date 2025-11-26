using ERP.Data.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Implementaciones
{
    public class ServicioAnuncio : IServicioAnuncio
    {
        private readonly IRepositorioAnuncio _repositorio;
        private readonly IServicioUsuarios _usuarioServicio;

        public ServicioAnuncio(IRepositorioAnuncio repositorio, IServicioUsuarios usuarioServicio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            _usuarioServicio = usuarioServicio ?? throw new ArgumentNullException(nameof(usuarioServicio));
        }

        // Crear un nuevo anuncio
        public async Task<Resultado<Anuncio>> CrearAsync(Anuncio anuncio)
        {
            if (anuncio == null)
                return Resultado<Anuncio>.Falla("El anuncio no puede ser nulo.");

            if (anuncio.UsuarioId <= 0 && anuncio.Usuario != null)
                anuncio.UsuarioId = anuncio.Usuario.Id;

            if (anuncio.UsuarioId <= 0)
                return Resultado<Anuncio>.Falla("No se ha especificado el usuario que publica el anuncio.");

            anuncio.FechaPublicacion = DateTime.Now;

            var resultado = await _repositorio.CrearAsync(anuncio);
            if (!resultado.esExitoso)
                return Resultado<Anuncio>.Falla(resultado.MensajeError ?? "Error al crear el anuncio.");

            await _repositorio.GuardarAsync();
            return Resultado<Anuncio>.Exito(anuncio);
        }

        // Obtener todos (ahora con esPasantia y esCarrusel)
        public async Task<Resultado<List<AnuncioDetalleDTO>>> ObtenerTodosAsync(bool? esPasantia = null, bool? esCarrusel = null)
        {
            var resultado = await _repositorio.ObtenerTodosAsync();
            if (!resultado.esExitoso)
                return Resultado<List<AnuncioDetalleDTO>>.Falla(resultado.MensajeError ?? "Error al obtener los anuncios.");

            var anuncios = resultado.Valor ?? new List<Anuncio>();

            // filtro pasantía
            if (esPasantia.HasValue)
                anuncios = anuncios.Where(a => a.EsPasantia == esPasantia.Value).ToList();

            // filtro carrusel
            if (esCarrusel.HasValue)
                anuncios = anuncios.Where(a => a.EsCarrusel == esCarrusel.Value).ToList();

            var dtos = new List<AnuncioDetalleDTO>();

            foreach (var a in anuncios)
            {
                string nombreCompleto = await ObtenerNombreUsuarioAsync(a);

                dtos.Add(new AnuncioDetalleDTO
                {
                    Id = a.Id,
                    Titulo = a.Titulo,
                    Descripcion = a.Descripcion,
                    ImagenUrl = a.ImagenUrl,
                    EsPasantia = a.EsPasantia,
                    EsCarrusel = a.EsCarrusel,
                    FechaPublicacion = a.FechaPublicacion,
                    UsuarioId = a.UsuarioId,
                    NombreUsuario = nombreCompleto
                });
            }

            return Resultado<List<AnuncioDetalleDTO>>.Exito(dtos);
        }

        // Obtener anuncio por ID
        public async Task<Resultado<AnuncioDetalleDTO>> ObtenerPorIdAsync(int id)
        {
            var resultado = await _repositorio.ObtenerPorIdAsync(id);
            if (!resultado.esExitoso)
                return Resultado<AnuncioDetalleDTO>.Falla(resultado.MensajeError ?? "Error al obtener el anuncio.");

            var anuncio = resultado.Valor!;
            string nombreCompleto = await ObtenerNombreUsuarioAsync(anuncio);

            var dto = new AnuncioDetalleDTO
            {
                Id = anuncio.Id,
                Titulo = anuncio.Titulo,
                Descripcion = anuncio.Descripcion,
                ImagenUrl = anuncio.ImagenUrl,
                EsPasantia = anuncio.EsPasantia,
                EsCarrusel = anuncio.EsCarrusel,
                FechaPublicacion = anuncio.FechaPublicacion,
                UsuarioId = anuncio.UsuarioId,
                NombreUsuario = nombreCompleto
            };

            return Resultado<AnuncioDetalleDTO>.Exito(dto);
        }

        // Actualizar anuncio
        public async Task<Resultado<bool>> ActualizarAsync(int id, ActualizarAnuncioDTO dto)
        {
            var resultado = await _repositorio.ObtenerPorIdAsync(id);
            if (!resultado.esExitoso)
                return Resultado<bool>.Falla(resultado.MensajeError ?? "El anuncio no existe o no se pudo obtener.");

            var anuncio = resultado.Valor!;

            anuncio.Titulo = string.IsNullOrWhiteSpace(dto.Titulo) ? anuncio.Titulo : dto.Titulo;
            anuncio.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? anuncio.Descripcion : dto.Descripcion;
            anuncio.ImagenUrl = string.IsNullOrWhiteSpace(dto.ImagenUrl) ? anuncio.ImagenUrl : dto.ImagenUrl;
            anuncio.EsPasantia = dto.EsPasantia ?? anuncio.EsPasantia;
            anuncio.EsCarrusel = dto.EsCarrusel ?? anuncio.EsCarrusel;

            var resActualiza = await _repositorio.ActualizarAsync(anuncio);
            if (!resActualiza.esExitoso)
                return Resultado<bool>.Falla(resActualiza.MensajeError ?? "Error al actualizar el anuncio.");

            await _repositorio.GuardarAsync();
            return Resultado<bool>.Exito(true);
        }

        // Eliminar anuncio
        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            var resultado = await _repositorio.EliminarAsync(id);
            if (!resultado.esExitoso)
                return Resultado<bool>.Falla(resultado.MensajeError ?? "Error al eliminar el anuncio.");

            await _repositorio.GuardarAsync();
            return Resultado<bool>.Exito(true);
        }

        // Obtener curriculums
        public async Task<Resultado<List<string>>> ObtenerCurriculumsAsync(int id)
        {
            var resultado = await _repositorio.ObtenerCurriculumsAsync(id);
            if (!resultado.esExitoso)
                return Resultado<List<string>>.Falla(resultado.MensajeError ?? "Error al obtener los currículums.");

            var lista = resultado.Valor!
                .Select(c => c.ArchivoUrl)
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .ToList();

            return Resultado<List<string>>.Exito(lista);
        }

        // Alternar like
        public async Task<Resultado<bool>> ToggleLikeAsync(int anuncioId, int usuarioId)
        {
            var resultado = await _repositorio.ToggleLikeAsync(anuncioId, usuarioId);
            return resultado.esExitoso
                ? Resultado<bool>.Exito(true)
                : Resultado<bool>.Falla(resultado.MensajeError ?? "Error al alternar el 'like'.");
        }

        // Obtener nombre usuario
        private async Task<string> ObtenerNombreUsuarioAsync(Anuncio anuncio)
        {
            if (anuncio.Usuario != null)
                return $"{anuncio.Usuario.NombreUsuario} {anuncio.Usuario.ApellidoUsuario}".Trim();

            if (anuncio.UsuarioId > 0)
            {
                var usuario = await _usuarioServicio.ObtenerUsuarioPorId(anuncio.UsuarioId);
                if (usuario != null)
                    return $"{usuario.NombreUsuario} {usuario.ApellidoUsuario}".Trim();
            }

            return "Usuario Desconocido";
        }
    }
}
