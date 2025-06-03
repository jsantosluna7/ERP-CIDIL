using ERP.Data.Modelos;
using Inventario.Abstraccion.Repositorio;
using Inventario.Abstraccion.Servicios;
using Inventario.DTO.LaboratorioDTO;
using Microsoft.EntityFrameworkCore;

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
                Nombre= laboratorio.Nombre,
                Piso = laboratorio.Piso,
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
                Horarios=lab.Horarios,
                InventarioEquipos = lab.InventarioEquipos,
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
                    Descripcion = laboratorio1.Descripcion,
                     
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
                    Activado = pisos.Activado,
                    Nombre = pisos.Nombre,
                };
                pDTO.Add(nuevopDTO);
            }
            return pDTO;
            
        }
    }
}
