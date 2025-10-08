using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.Modelos
{
     public class Comentario
    {
        public int Id { get; set; }
        public string Texto { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public int AnuncioId { get; set; }
        public Anuncio? Anuncio { get; set; }




    }


}
