using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBanco
{
    internal class CBiblioteca
    {
        // Validar DNI: exactamente 8 dígitos numéricos
        public static bool ValidarDNI(string dni)
        {
            if (dni.Length == 8 && long.TryParse(dni, out _))
                return true;
            return false;
        }

        // Convertir a mayúsculas
        public static string ConvertirMayusculas(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";
            return texto.ToUpper();
        }

        // Validar Email: que contenga '@'
        public static bool ValidarEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
                return true;
            return false;
        }

        // Validar Teléfono: exactamente 9 dígitos numéricos
        public static bool ValidarTelefono(string telefono)
        {
            if (telefono.Length == 9 && long.TryParse(telefono, out _))
                return true;
            return false;
        }
    }
}
