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
        public LaboratorioDTO Actualizar(int id, ActualizarLaboratorioDTO actualizarlaboratorioDTO)
        {
           var laboratorio = repositorioLaboratorio.Actualizar(id,actualizarlaboratorioDTO);
            var laboratorioDTO = new LaboratorioDTO
            {
                CodigoDeLab = laboratorio.CodigoDeLab,
                Capacidad = laboratorio.Capacidad,
                Descripcion = laboratorio.Descripcion,
            };
            return laboratorioDTO;
        }

        //Metodo para crear los espcacios de los laboratorios
        public LaboratorioDTO Crear(CrearLaboratorioDTO crearlaboratorioDTO)
        {
            var laboratorio = repositorioLaboratorio .Crear(crearlaboratorioDTO);
            var laboratorioDTO = new LaboratorioDTO
            {
                CodigoDeLab = laboratorio .CodigoDeLab,
                Capacidad = laboratorio.Capacidad,
                Descripcion = laboratorio.Descripcion,
            };
            return laboratorioDTO;
        }

        //Metodo para eliminar el reguistro de los laboratotio
        public void Eliminar(int id)
        {
            repositorioLaboratorio.Eliminar(id);
        }

        //Metodo para optener los laboratorios por ID
        public Laboratorio GetById(int id)
        {
            return repositorioLaboratorio.GetById(id);
        }

        // Metodo para llamar todos los registros de los laboratorios
        public List<LaboratorioDTO> GetLaboratorio()
        {
            var laboratorio = repositorioLaboratorio.GetLaboratorio();
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
