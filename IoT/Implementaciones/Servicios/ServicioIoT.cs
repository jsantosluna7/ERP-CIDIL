using IoT.Abstraccion.Repositorio;
using IoT.Abstraccion.Servicios;
using IoT.DTO;
using IoT.Implementaciones.Repositorios;
using IoT.Modelos;

namespace IoT.Implementaciones.Servicios
{
    public class ServicioIoT : IServicioIoT
    {
        //Se hace una inyeccion de dependencia
        private readonly IRepositorioIoT _repositorio;
        public ServicioIoT(IRepositorioIoT repositorio)
        {
            _repositorio = repositorio;
        }

        //Se usa el metodo Eliminar para borrar los registros por ID
        public async Task<bool?> Eliminar(int id)
        {
            var r = await _repositorio.Eliminar(id);
            if (r == null)
            {
                return null;
            }
            return r;
        }
        //Se usa el metodo para optener los registros por ID
        public async Task<Iot?> GetByIdIoT(int id)
        {
            var ioT = await _repositorio.GetByIdIot(id);
            return new Iot
            {
                Id = ioT.Id,
                IdPlaca = ioT.IdPlaca,
                IdLaboratorio = ioT.IdLaboratorio,
                Sensor1 = ioT.Sensor1,
                Sensor2 = ioT.Sensor2,
                Sensor3 = ioT.Sensor3,
                Sensor4 = ioT.Sensor4,
                Sensor5 = ioT.Sensor5,
                Actuador = ioT.Actuador,
                HoraEntrada = ioT.HoraEntrada

            };
        }
        //Se usa el metodo optener para llamar a todos los registros disponibles
        public async Task<List<IoTDTO>?> GetIot(int pagina, int tamanoPagina)
        {
            var ioT = await _repositorio.GetIot(pagina, tamanoPagina);
            
           var ioTDTO = new List<IoTDTO>();
            foreach(Iot ioTs in ioT)
            {
                var nueovoIoT = new IoTDTO
                {
                    Id = ioTs.Id,
                    IdPlaca = ioTs.IdPlaca,
                    IdLaboratorio = ioTs.IdLaboratorio,
                    Sensor1 = ioTs.Sensor1,
                    Sensor2 = ioTs.Sensor2,
                    Sensor3 = ioTs.Sensor3,
                    Sensor4 = ioTs.Sensor4,
                    Sensor5 = ioTs.Sensor5,
                    Actuador = ioTs.Actuador,
                    HoraEntrada = ioTs.HoraEntrada
                };
                ioTDTO.Add(nueovoIoT);
            }
            return ioTDTO;
        }

        //Método para desactivar un IoT
        public async Task<bool?> desactivarIoT(int id)
        {
            // Verificar si el IoT existe
            var IoT = await _repositorio.GetByIdIot(id);
            if (IoT == null)
            {
                return null;
            }
            // Desactivar el IoT
            IoT.Activado = false;
            // Guardar los cambios en la base de datos
            await _repositorio.desactivarIoT(id);
            return true;
        }
    }
}
