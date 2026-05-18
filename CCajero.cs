using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBanco
{
    internal class CCajero
    {
        // Atributos privados
        private string aDNI;
        private string aNombres;
        private string aDireccion;
        private string aEmail;
        private string aTelefono;

        // Constructor vacío
        public CCajero()
        {
            aDNI = "";
            aNombres = "";
            aDireccion = "";
            aEmail = "";
            aTelefono = "";
        }

        // Constructor con parámetros
        public CCajero(string dni, string nombres, string direccion, string email, string telefono)
        {
            aDNI = dni;
            aNombres = nombres;
            aDireccion = direccion;
            aEmail = email;
            aTelefono = telefono;
        }

        // Propiedades
        public string DNI
        {
            get { return aDNI; }
            set { aDNI = value; }
        }

        public string Nombres
        {
            get { return aNombres; }
            set { aNombres = value; }
        }

        public string Direccion
        {
            get { return aDireccion; }
            set { aDireccion = value; }
        }

        public string Email
        {
            get { return aEmail; }
            set { aEmail = value; }
        }

        public string Telefono
        {
            get { return aTelefono; }
            set { aTelefono = value; }
        }

        // Método Leer: registrar datos con validaciones
        public void Leer()
        {
            // DNI
            do
            {
                Console.Write("Ingrese DNI (8 dígitos): ");
                aDNI = Console.ReadLine();
            } while (!CBiblioteca.ValidarDNI(aDNI));

            // Nombres
            Console.Write("Ingrese Nombres: ");
            aNombres = CBiblioteca.ConvertirMayusculas(Console.ReadLine());

            // Dirección
            Console.Write("Ingrese Dirección: ");
            aDireccion = CBiblioteca.ConvertirMayusculas(Console.ReadLine());

            // Email
            do
            {
                Console.Write("Ingrese Email (debe contener @): ");
                aEmail = Console.ReadLine();
            } while (!CBiblioteca.ValidarEmail(aEmail));

            // Teléfono
            do
            {
                Console.Write("Ingrese Teléfono (9 dígitos): ");
                aTelefono = Console.ReadLine();
            } while (!CBiblioteca.ValidarTelefono(aTelefono));
        }

        // Método Mostrar: mostrar datos
        public void Mostrar()
        {
            Console.WriteLine("\n=== Datos del Cajero ===");
            Console.WriteLine($"DNI: {aDNI}");
            Console.WriteLine($"Nombres: {aNombres}");
            Console.WriteLine($"Dirección: {aDireccion}");
            Console.WriteLine($"Email: {aEmail}");
            Console.WriteLine($"Teléfono: {aTelefono}");
        }

        public override string ToString()
        {
            return $"DNI: {DNI} | Nombre: {Nombres} | Dirección: {Direccion} | Email: {Email} | Tel: {Telefono}";
        }

    }
}
