using Inventario.Abstraccion.Repositorio;
using Inventario.Abstraccion.Servicios;
using Inventario.DTO.LaboratorioDTO;
using Inventario.Modelos;

namespace Inventario.Implementaciones.Servicios
{
    public class ServicioLaboratorio : IServicioLaboratorio
    {
        //Hacemos una inyeccion de dependencia
        private readonly IRepositorioLaboratorio repositorioLaboratorio;

        public ServicioLaboratorio(IRepositorioLaboratorio repositorio)
        {
            repositorioLaboratorio = repositorio;
        }   

        //Metodo para actualizar los laboratorios
        public async Task<LaboratorioDTO?> Actualizar(int id, ActualizarLaboratorioDTO actualizarlaboratorioDTO)
        {
           var laboratorio =await repositorioLaboratorio.Actualizar(id,actualizarlaboratorioDTO);
            if (laboratorio == null)
            {
                return null;
            }
            var laboratorioDTO = new LaboratorioDTO
            {
                CodigoDeLab = laboratorio.CodigoDeLab,
                Capacidad = laboratorio.Capacidad,
                Descripcion = laboratorio.Descripcion,
            };
            return laboratorioDTO;
        }

        //Metodo para crear los espcacios de los laboratorios
        public async Task<LaboratorioDTO?> Crear(CrearLaboratorioDTO crearlaboratorioDTO)
        {
            var laboratorio = await repositorioLaboratorio .Crear(crearlaboratorioDTO);
            if (laboratorio == null)
            {
                return null;
            }
            var laboratorioDTO = new LaboratorioDTO
            {
                CodigoDeLab = laboratorio .CodigoDeLab,
                Capacidad = laboratorio.Capacidad,
                Descripcion = laboratorio.Descripcion,
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

        //Metodo para optener los laboratorios por ID
        public async Task<Laboratorio?> GetById(int id)
        {
            return await repositorioLaboratorio.GetById(id);
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
                    Descripcion = laboratorio1.Descripcion,
                     
                };
                laboratorioDTO.Add(nuevolaboratorioDTO);
            }
            return laboratorioDTO;
        }
    }
}
