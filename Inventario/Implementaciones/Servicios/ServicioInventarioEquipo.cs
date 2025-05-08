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
        public InventarioEquipoDTO Actualizar(int id, ActualizarInventarioEquipoDTO actualizarInventarioEquipoDTO)
        {
           var invEquipo = repositorioInventarioEquipo.Actualizar(id, actualizarInventarioEquipoDTO);
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
                 Estado = invEquipo.Estado,
            };
            return invEquipoDTO;
        }


        //Usamos el metodo para crear el registro de los equipos
        public InventarioEquipoDTO Crear(CrearInventarioEquipoDTO crearInventarioEquipoDTO)
        {
            var invEquipo = repositorioInventarioEquipo.Crear(crearInventarioEquipoDTO);
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
                Estado = invEquipo.Estado,
            };

            return invEquipoDTO;
        }

        //Usamos el metodo para eliminar los registros
        public void Eliminar(int id)
        {
            repositorioInventarioEquipo.Eliminar(id);
        }
        //Metodo para llamar los equipos por ID
        public InventarioEquipo GetById(int id)
        {
            return repositorioInventarioEquipo.GetById(id);
        }

        //Metodo para llamar todos los registros de los equipos 
        public List<InventarioEquipoDTO> GetInventarioEquipo()
        {
           var invEquipos = repositorioInventarioEquipo.GetInventarioEquipos();
           var invEquipoDTO =  new List<InventarioEquipoDTO>();
           foreach(InventarioEquipo invEquipo in invEquipos)
            {
                var nuevoInvEquipo = new InventarioEquipoDTO
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
                    Estado = invEquipo.Estado,
                };
                invEquipoDTO.Add(nuevoInvEquipo);
            }
           return invEquipoDTO;
        }
    }
}
