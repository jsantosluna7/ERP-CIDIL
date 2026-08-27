using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.Modelos
{
    public class Resultado<T>
    {
        public bool esExitoso { get; init; }
        public T? Valor { get; init; }
        public string? MensajeError { get; init; }

        public static Resultado<T> Exito(T valor) =>
            new() { esExitoso = true, Valor = valor };
        public static Resultado<T> Falla(string mensajeError) =>
            new() { esExitoso = false, MensajeError = mensajeError };
    }
}
