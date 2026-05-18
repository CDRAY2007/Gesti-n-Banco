using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBanco
{
    internal class CClientes
    {
        //Atributos privados
        private string aDNI;
        private string aNombres;
        private DateTime aFechaNacimiento;
        private bool aDiscapacidad;
        private bool aNinos;
        private string aEmail;
        private string aTelefono;
        private double aMonto;
        

        // Constructor por defecto
        public CClientes()
        {
            aDNI = "";
            aNombres = "";
            aFechaNacimiento = DateTime.MinValue;
            aDiscapacidad = false;
            aNinos = false;
            aEmail = "";
            aTelefono = "";
            aMonto = 0.0;
        }

        //Constructor con parámetros
        public CClientes(string dni, string nombres, DateTime fechaNacimiento, bool discapacidad, bool ninos, string email, string telefono, double monto)
        {
            aDNI = dni;
            aNombres = nombres;
            aFechaNacimiento = fechaNacimiento;
            aDiscapacidad = discapacidad;
            aNinos = ninos;
            aEmail = email;
            aTelefono = telefono;
            aMonto = monto;
        }

        //Getters y Setters (Propiedades públicas)
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

        public DateTime FechaNacimiento
        {
            get { return aFechaNacimiento; }
            set { aFechaNacimiento = value; }
        }

        public bool Discapacidad
        {
            get { return aDiscapacidad; }
            set { aDiscapacidad = value; }
        }

        public bool Niños
        {
            get { return aNinos; }
            set { aNinos = value; }
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

        public double Monto
        {
            get { return aMonto; }
            set { aMonto = value; }
        }

        // Método para calcular la edad
        public int CalcularEdad()
        {
            if (aFechaNacimiento == DateTime.MinValue) return 0;
            int edad = DateTime.Now.Year - aFechaNacimiento.Year;
            if (DateTime.Now.DayOfYear < aFechaNacimiento.DayOfYear)
                edad--;
            return edad;
        }

        //Método para determinar prioridad
        // Mayores de 60 años → prioridad 2
        // Clientes con hijos → prioridad 1
        // Clientes con discapacidad → prioridad 2
        // Clientes comunes → prioridad 3
        public int ObtenerPrioridad()
        {
            int edad = CalcularEdad();

            if (aNinos)
                return 1; // prioridad más alta
            else if (edad >= 60 || aDiscapacidad)
                return 2;
            else
                return 3;
        }

        //Método para saber si va a ventanilla preferencial
        public bool EsVentanillaPreferencial()
        {
            return aNinos || aDiscapacidad || CalcularEdad() >= 60;
        }

        //Método para mostrar información
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

            // Fecha de nacimiento
            bool fechaValida = false;
            while (!fechaValida)
            {
                Console.Write("Ingrese Fecha de Nacimiento (dd/mm/yyyy): ");
                string entrada = Console.ReadLine();
                if (DateTime.TryParse(entrada, out aFechaNacimiento))
                {
                    fechaValida = true;
                }
                else
                {
                    Console.WriteLine("❌ Fecha no válida. Intente de nuevo.");
                }
            }

            // Discapacidad
            string opcion;
            do
            {
                Console.Write("¿Tiene discapacidad? (S/N): ");
                opcion = Console.ReadLine().ToUpper();
            } while (opcion != "S" && opcion != "N");
            aDiscapacidad = (opcion == "S");

            // Tiene niños
            do
            {
                Console.Write("¿Tiene niños a su cargo? (S/N): ");
                opcion = Console.ReadLine().ToUpper();
            } while (opcion != "S" && opcion != "N");
            aNinos = (opcion == "S");

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

            // Monto
            double montoTemp;
            do
            {
                Console.Write("Ingrese monto de la operación: ");
            } while (!double.TryParse(Console.ReadLine(), out montoTemp) || montoTemp < 0);
            aMonto = montoTemp;
        }

        public void MostrarDatos()
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("DNI: " + aDNI);
            Console.WriteLine("Nombres: " + aNombres);
            Console.WriteLine("Fecha de Nacimiento: " + aFechaNacimiento.ToString("dd/MM/yyyy"));
            Console.WriteLine("Edad: " + CalcularEdad());
            Console.WriteLine("Discapacidad: " + (aDiscapacidad ? "Sí" : "No"));
            Console.WriteLine("Tiene Niños: " + (aNinos ? "Sí" : "No"));
            Console.WriteLine("Email: " + aEmail);
            Console.WriteLine("Teléfono: " + aTelefono);
            Console.WriteLine("Monto: S/ " + aMonto.ToString("F2"));
            Console.WriteLine("Prioridad: " + ObtenerPrioridad());
            Console.WriteLine("Ventanilla Preferencial: " + (EsVentanillaPreferencial() ? "Sí" : "No"));
            Console.WriteLine("=========================================");
        }

        public override string ToString()
        {
            return $"DNI: {DNI} | Nombre: {Nombres} | F.Nac: {FechaNacimiento:d} | " +
                   $"Discapacidad: {(Discapacidad ? "Sí" : "No")} | Niños: {(Niños ? "Sí" : "No")} | " +
                   $"Email: {Email} | Tel: {Telefono} | Monto: {Monto}";
        }

    }
}
