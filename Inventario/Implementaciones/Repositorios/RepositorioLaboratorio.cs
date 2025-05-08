using Inventario.Abstraccion.Repositorio;
using Inventario.DTO.LaboratorioDTO;
using Inventario.Modelos;

namespace Inventario.Implementaciones.Repositorios
{
    public class RepositorioLaboratorio : IRepositorioLaboratorio
    {
        // Se hace una inyeccion de dependencia
        private readonly DbErpContext _context;

        public RepositorioLaboratorio(DbErpContext context)
        {
            _context = context;
        }

        // se utliliza el metodo para actualizar los laboratorios
        
        public Laboratorio Actualizar(int id, ActualizarLaboratorioDTO actualizarLaboratorioDTO)
        {
          var laboratorioExistente = GetById(id);


            laboratorioExistente.CodigoDeLab= actualizarLaboratorioDTO.CodigoDeLab;
            laboratorioExistente.Capacidad = actualizarLaboratorioDTO.Capacidad;
            laboratorioExistente.Descripcion= actualizarLaboratorioDTO.Descripcion;

            _context.Update(laboratorioExistente);
            _context.SaveChanges();
            var laboratorioActualizado = GetById(id);
            return laboratorioActualizado;
        }

        // Se utiliza el metodo Para crear los laboratorios
        public Laboratorio Crear(CrearLaboratorioDTO crearlaboratorioDTO)
        {
            var laboratorio = new Laboratorio
            {
                CodigoDeLab = crearlaboratorioDTO.CodigoDeLab,
                Capacidad = crearlaboratorioDTO.Capacidad,
                Descripcion = crearlaboratorioDTO.Descripcion,
            };
            _context.Laboratorios.Add(laboratorio);
            _context.SaveChanges();
            return laboratorio;



        }


        //Se utiliza el metodo para eliminar el registro por ID
        public void Eliminar(int id)
        {
          Laboratorio laboratorio = GetById(id);
            _context.Remove(laboratorio);
            _context.SaveChanges();
        }

        //Se optienen los registros por ID
        public Laboratorio GetById(int id)
        {
            return _context.Laboratorios.Where(l => l.Id == id).FirstOrDefault();
        }


        //Se optienen todos los registros
        public List<Laboratorio> GetLaboratorio()
        {
           return [.. _context.Laboratorios];
        }
    }
}
