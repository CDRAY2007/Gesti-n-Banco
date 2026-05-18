using System;

namespace AppBanco
{
    internal class CColaPrioridad
    {
        private CClientes elemento;
        private CColaPrioridad subCola;

        public CColaPrioridad()
        {
            elemento = null;
            subCola = null;
        }

        public bool EstaVacia()
        {
            return elemento == null && subCola == null;
        }

        public void Encolar(CClientes nuevo)
        {
            if (EstaVacia())
            {
                elemento = nuevo;
                return;
            }

            // Si el nuevo tiene mayor prioridad (número menor), lo ponemos al frente
            if (nuevo.ObtenerPrioridad() < elemento.ObtenerPrioridad())
            {
                var temp = new CColaPrioridad();
                temp.elemento = elemento;
                temp.subCola = subCola;

                elemento = nuevo;
                subCola = temp;
            }
            else
            {
                if (subCola == null)
                    subCola = new CColaPrioridad();

                subCola.Encolar(nuevo);
            }
        }

        public CClientes Desencolar()
        {
            if (EstaVacia()) return null;

            CClientes temp = elemento;

            if (subCola == null)
            {
                elemento = null;
            }
            else
            {
                elemento = subCola.elemento;
                subCola = subCola.subCola;
            }

            return temp;
        }

        public void Mostrar()
        {
            if (EstaVacia()) return;

            Console.WriteLine(elemento.ToString());
            subCola?.Mostrar();
        }

        public int Contar()
        {
            if (EstaVacia()) return 0;
            return 1 + (subCola?.Contar() ?? 0);
        }

        // 🔹 Método unido desde tu menú
        public void VerColasPrioridad()
        {
            Console.Clear();
            Console.WriteLine("=== Colas de Prioridad ===");

            if (EstaVacia())
            {
                Console.WriteLine("No hay clientes en la cola.");
            }
            else
            {
                Mostrar();
            }

            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey();
        }
    }
}
