using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBanco
{
    internal class Atenciones
    {

        // ===== Atributos privados =====
        private string aNroTicket;
        private DateTime aFechaHora;
        private string aIdCliente;
        private string aIdCajero;
        private string aIdServicio;
        private double aMonto;
        private int aSegundos;

        // ===== Constructor vacío =====
        public Atenciones()
        {
            aNroTicket = "";
            aFechaHora = DateTime.Now;
            aIdCliente = "";
            aIdCajero = "";
            aIdServicio = "";
            aMonto = 0;
            aSegundos = 0;
        }

        // ===== Constructor con parámetros =====
        public Atenciones(string nroTicket, DateTime fechaHora, string idCliente, string idCajero, string idServicio, double monto, int segundos)
        {
            aNroTicket = nroTicket;
            aFechaHora = fechaHora;
            aIdCliente = idCliente;
            aIdCajero = idCajero;
            aIdServicio = idServicio;
            aMonto = monto;
            aSegundos = segundos;
        }

        // ===== Propiedades =====
        public string NroTicket
        {
            get { return aNroTicket; }
            set { aNroTicket = value; }
        }

        public DateTime FechaHora
        {
            get { return aFechaHora; }
            set { aFechaHora = value; }
        }

        public string IdCliente
        {
            get { return aIdCliente; }
            set { aIdCliente = value; }
        }

        public string IdCajero
        {
            get { return aIdCajero; }
            set { aIdCajero = value; }
        }

        public string IdServicio
        {
            get { return aIdServicio; }
            set { aIdServicio = value; }
        }

        public double Monto
        {
            get { return aMonto; }
            set { aMonto = value; }
        }

        public int Segundos
        {
            get { return aSegundos; }
            set { aSegundos = value; }
        }

        // ===== Método Leer =====
        public void Leer()
        {
            Console.Write("Ingrese número de ticket: ");
            aNroTicket = Console.ReadLine();

            aFechaHora = DateTime.Now;

            Console.Write("Ingrese DNI del cliente: ");
            aIdCliente = Console.ReadLine();

            Console.Write("Ingrese DNI del cajero: ");
            aIdCajero = Console.ReadLine();

            Console.Write("Ingrese ID del servicio: ");
            aIdServicio = Console.ReadLine();

            do
            {
                Console.Write("Ingrese monto de la operación: ");
                double.TryParse(Console.ReadLine(), out aMonto);
            } while (aMonto <= 0);

            do
            {
                Console.Write("Ingrese duración de la atención en segundos: ");
                int.TryParse(Console.ReadLine(), out aSegundos);
            } while (aSegundos <= 0);
        }

        // ===== Método Mostrar =====
        public void Mostrar()
        {
            Console.WriteLine("\n=== Datos de la Atención ===");
            Console.WriteLine($"Nro Ticket: {aNroTicket}");
            Console.WriteLine($"Fecha y Hora: {aFechaHora}");
            Console.WriteLine($"ID Cliente: {aIdCliente}");
            Console.WriteLine($"ID Cajero: {aIdCajero}");
            Console.WriteLine($"ID Servicio: {aIdServicio}");
            Console.WriteLine($"Monto: {aMonto:C}");
            Console.WriteLine($"Duración: {aSegundos} segundos");
        }

        public override string ToString()
        {
            return $"Ticket: {NroTicket} | Fecha: {FechaHora} | Cliente: {IdCliente} | Cajero: {IdCajero} | " +
                   $"Servicio: {IdServicio} | Monto: {Monto} | Segundos: {Segundos}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Atenciones other)
            {
                return this.NroTicket == other.NroTicket;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return NroTicket.GetHashCode();
        }

    }
}
