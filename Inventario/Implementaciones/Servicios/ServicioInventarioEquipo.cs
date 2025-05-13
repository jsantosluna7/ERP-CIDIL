using Inventario.Abstraccion.Repositorio;
using Inventario.Abstraccion.Servicios;
using Inventario.DTO.InventarioEquipoDTO;
using Inventario.Modelos;

namespace Inventario.Implementaciones.Servicios
{
    public class ServicioInventarioEquipo : IServicioInventarioEquipo
    {
        //Hacemos una inyeccion de dependencia
        private readonly IRepositorioInventarioEquipo repositorioInventarioEquipo;

        public ServicioInventarioEquipo(IRepositorioInventarioEquipo rInventarioEquipo)
        {
            this.repositorioInventarioEquipo = rInventarioEquipo;
        }

        //Usamos el metodo actualizar los componentes 
        public async Task<InventarioEquipoDTO?> Actualizar(int id, ActualizarInventarioEquipoDTO actualizarInventarioEquipoDTO)
        {
           var invEquipo =await repositorioInventarioEquipo.Actualizar(id, actualizarInventarioEquipoDTO);
            if (invEquipo == null)
            {
                return null;
            }
            var invEquipoDTO = new InventarioEquipoDTO
            {
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
                 ValidacionPrestamo =invEquipo.ValidacionPrestamo,
                 FechaTransaccion = invEquipo.FechaTransaccion,
                 Departamento = invEquipo.Departamento,
            };
            return invEquipoDTO;
        }


        //Usamos el metodo para crear el registro de los equipos
        public async Task<InventarioEquipoDTO?> Crear(CrearInventarioEquipoDTO crearInventarioEquipoDTO)
        {
            var invEquipo =await repositorioInventarioEquipo.Crear(crearInventarioEquipoDTO);
            if (invEquipo == null)
            {
                return null;
            }
            var invEquipoDTO = new InventarioEquipoDTO
            {
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
                IdEstadoFisico= invEquipo.IdEstadoFisico,
                ValidacionPrestamo=invEquipo.ValidacionPrestamo,
                FechaTransaccion=invEquipo.FechaTransaccion,
                Departamento = invEquipo.Departamento,
            };

            return invEquipoDTO;
        }

        //Usamos el metodo para eliminar los registros
        public async Task<bool?> Eliminar(int id)
        {
         var r =  await repositorioInventarioEquipo.Eliminar(id);
            if (r == null)
            {
                return null;
            }
            return r;
        }
        //Metodo para llamar los equipos por ID
        public async Task<InventarioEquipo?> GetById(int id)
        {
            return await repositorioInventarioEquipo.GetById(id);
        }

        //Metodo para llamar todos los registros de los equipos 
        public async Task<List<InventarioEquipoDTO>?> GetInventarioEquipo()
        {
           var invEquipos =await repositorioInventarioEquipo.GetInventarioEquipos();
            if(invEquipos == null)
            {
                return null;
            }
           var invEquipoDTO =  new List<InventarioEquipoDTO>();
           foreach(InventarioEquipo invEquipo in invEquipos)
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
                    ValidacionPrestamo =invEquipo.ValidacionPrestamo,
                    FechaTransaccion = invEquipo.FechaTransaccion,
                    Departamento = invEquipo.Departamento,
                    Cantidad = invEquipo.Cantidad,
                };
                invEquipoDTO.Add(nuevoInvEquipo);
            }
           return invEquipoDTO;
        }
    }
}
