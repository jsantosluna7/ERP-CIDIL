namespace IoT.DTO
{
    public class IoTDTO
    {
        public int Id { get; set; }

        public string IdPlaca { get; set; } = null!;

        public int IdLaboratorio { get; set; }

        public float? Sensor1 { get; set; }

        public float? Sensor2 { get; set; }

        public float? Sensor3 { get; set; }

        public float? Sensor4 { get; set; }

        public float? Sensor5 { get; set; }

        public bool? Actuador { get; set; }

        public DateTime? HoraEntrada { get; set; }
    }
}
