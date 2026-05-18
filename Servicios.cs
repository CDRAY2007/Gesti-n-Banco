using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBanco
{
    internal class Servicios
    {
        // ===== Atributos privados =====
        private string aIdServicio;
        private string aDescripcion;

        // ===== Constructor vacío =====
        public Servicios()
        {
            aIdServicio = "";
            aDescripcion = "";
        }

        // ===== Constructor con parámetros =====
        public Servicios(string idServicio, string descripcion)
        {
            aIdServicio = idServicio;
            aDescripcion = descripcion;
        }

        // ===== Propiedades =====
        public string IdServicio
        {
            get { return aIdServicio; }
            set { aIdServicio = value; }
        }

        public string Descripcion
        {
            get { return aDescripcion; }
            set { aDescripcion = value; }
        }

        // ===== Método Leer =====
        public void Leer()
        {
            // ID del servicio
            do
            {
                Console.Write("Ingrese ID del servicio (texto o número): ");
                aIdServicio = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(aIdServicio));

            // Descripción
            Console.Write("Ingrese descripción del servicio: ");
            aDescripcion = CBiblioteca.ConvertirMayusculas(Console.ReadLine());
        }

        // ===== Método Mostrar =====
        public void Mostrar()
        {
            Console.WriteLine("\n=== Datos del Servicio ===");
            Console.WriteLine($"ID Servicio: {aIdServicio}");
            Console.WriteLine($"Descripción: {aDescripcion}");
        }

        public override string ToString()
        {
            return $"ID: {IdServicio} | Descripción: {Descripcion}";
        }

    }
}
