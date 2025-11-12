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
    public class ServicioAnuncio : IAnuncioServicio
    {
        private readonly IAnuncioRepositorio _repositorio;
        private readonly IServicioUsuarios _usuarioServicio;

        public ServicioAnuncio(IAnuncioRepositorio repositorio, IServicioUsuarios usuarioServicio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            _usuarioServicio = usuarioServicio ?? throw new ArgumentNullException(nameof(usuarioServicio));
        }

        // Crear un nuevo anuncio
        //Cambiado a retornar Resultado<Anuncio> para coincidir con la interfaz y el controlador.
        public async Task<Resultado<Anuncio>> CrearAsync(Anuncio anuncio)
        {
            if (anuncio == null)
                return Resultado<Anuncio>.Falla("El anuncio no puede ser nulo.");

            // Asigna correctamente el UsuarioId si el Usuario está presente
            if (anuncio.UsuarioId <= 0 && anuncio.Usuario != null)
                anuncio.UsuarioId = anuncio.Usuario.Id;

            if (anuncio.UsuarioId <= 0)
                return Resultado<Anuncio>.Falla("No se ha especificado el usuario que publica el anuncio.");

            anuncio.FechaPublicacion = DateTime.Now;

            var resultado = await _repositorio.CrearAsync(anuncio);
            if (!resultado.esExitoso)
                return Resultado<Anuncio>.Falla(resultado.MensajeError ?? "Error al crear el anuncio.");

            await _repositorio.GuardarAsync();

            // 🔥 CORRECCIÓN 1: Devuelve el objeto Anuncio que ahora tiene el ID de la BD.
            return Resultado<Anuncio>.Exito(anuncio);
        }

        // Obtener todos los anuncios (opcionalmente filtrados por pasantías)
        public async Task<Resultado<List<AnuncioDetalleDTO>>> ObtenerTodosAsync(bool? esPasantia = null)
        {
            var resultado = await _repositorio.ObtenerTodosAsync();
            if (!resultado.esExitoso)
                return Resultado<List<AnuncioDetalleDTO>>.Falla(resultado.MensajeError ?? "Error al obtener los anuncios.");

            var anuncios = resultado.Valor ?? new List<Anuncio>();

            if (esPasantia.HasValue)
                anuncios = anuncios.Where(a => a.EsPasantia == esPasantia.Value).ToList();

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
                    FechaPublicacion = a.FechaPublicacion,

                    //Mapeo del UsuarioId del modelo al DTO
                    UsuarioId = a.UsuarioId,

                    NombreUsuario = nombreCompleto
                });
            }

            return Resultado<List<AnuncioDetalleDTO>>.Exito(dtos);
        }

        // Obtener un anuncio por su ID
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
                FechaPublicacion = anuncio.FechaPublicacion,

                // 🔥 CORRECCIÓN 3: Mapeo del UsuarioId del modelo al DTO
                UsuarioId = anuncio.UsuarioId,

                NombreUsuario = nombreCompleto
            };

            return Resultado<AnuncioDetalleDTO>.Exito(dto);
        }

        // Actualizar un anuncio existente
        public async Task<Resultado<bool>> ActualizarAsync(int id, ActualizarAnuncioDTO dto)
        {
            var resultado = await _repositorio.ObtenerPorIdAsync(id);
            if (!resultado.esExitoso)
                return Resultado<bool>.Falla(resultado.MensajeError ?? "El anuncio no existe o no se pudo obtener.");

            // Ya no hay ObtenerPorIdModeloAsync. Asumimos que resultado.Valor es el modelo Anuncio.
            var anuncio = resultado.Valor!;

            anuncio.Titulo = string.IsNullOrWhiteSpace(dto.Titulo) ? anuncio.Titulo : dto.Titulo;
            anuncio.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? anuncio.Descripcion : dto.Descripcion;
            anuncio.ImagenUrl = string.IsNullOrWhiteSpace(dto.ImagenUrl) ? anuncio.ImagenUrl : dto.ImagenUrl;
            anuncio.EsPasantia = dto.EsPasantia ?? anuncio.EsPasantia;

            var resActualiza = await _repositorio.ActualizarAsync(anuncio);
            if (!resActualiza.esExitoso)
                return Resultado<bool>.Falla(resActualiza.MensajeError ?? "Error al actualizar el anuncio.");

            await _repositorio.GuardarAsync();
            return Resultado<bool>.Exito(true);
        }

        // Eliminar un anuncio
        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            var resultado = await _repositorio.EliminarAsync(id);
            if (!resultado.esExitoso)
                return Resultado<bool>.Falla(resultado.MensajeError ?? "Error al eliminar el anuncio.");

            await _repositorio.GuardarAsync();
            return Resultado<bool>.Exito(true);
        }

        // Obtener currículums asociados a un anuncio
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

        // Guardar currículum externo
        public async Task<Resultado<bool>> GuardarCurriculumAsync(int anuncioId, string nombreArchivo)
        {
            if (string.IsNullOrWhiteSpace(nombreArchivo))
                return Resultado<bool>.Falla("El archivo no puede estar vacío.");

            var curriculum = new Curriculum
            {
                AnuncioId = anuncioId,
                Nombre = "Externo",
                Email = "",
                ArchivoUrl = nombreArchivo,
                FechaEnvio = DateTime.UtcNow,
                EsExterno = true
            };

            var resultado = await _repositorio.AgregarCurriculumAsync(curriculum);
            if (!resultado.esExitoso)
                return Resultado<bool>.Falla(resultado.MensajeError ?? "Error al guardar el currículum.");

            await _repositorio.GuardarAsync();
            return Resultado<bool>.Exito(true);
        }

        // Alternar "like" de un usuario usando su Id
        public async Task<Resultado<bool>> ToggleLikeAsync(int anuncioId, int usuarioId)
        {
            var resultado = await _repositorio.ToggleLikeAsync(anuncioId, usuarioId);
            return resultado.esExitoso
                ? Resultado<bool>.Exito(true)
                : Resultado<bool>.Falla(resultado.MensajeError ?? "Error al alternar el 'like'.");
        }

        // Método auxiliar privado para obtener el nombre del usuario
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