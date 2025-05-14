using IoT.Abstraccion.Repositorio;
using IoT.Abstraccion.Servicios;
using IoT.DTO;
using IoT.Implementaciones.Repositorios;
using IoT.Modelos;

namespace IoT.Implementaciones.Servicios
{
    public class ServicioIoT : IServicioIoT
    {
        private readonly IRepositorioIoT _repositorio;
        public ServicioIoT(IRepositorioIoT repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<bool?> Eliminar(int id)
        {
            var r = await _repositorio.Eliminar(id);
            if (r == null)
            {
                return null;
            }
            return r;
        }

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

            };
        }

        public async Task<List<IoTDTO>?> GetIot()
        {
            var ioT = await _repositorio.GetIot();
            
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
                };
                ioTDTO.Add(nueovoIoT);
            }
            return ioTDTO;
        }
    }
}
