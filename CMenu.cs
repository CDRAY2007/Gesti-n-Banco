using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBanco
{
    internal class CMenu
    {
        //Instancia del gestor principal
        private CGestionBanco sistema = new CGestionBanco();

        public void MostrarMenuPrincipal()
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("===============================================");
                Console.WriteLine("  SISTEMA DE GESTIÓN - Ventanillas \"Tu Plata es Mía\"");
                Console.WriteLine("===============================================");
                Console.WriteLine("1. Cargar Datos");
                Console.WriteLine("2. Registrar");
                Console.WriteLine("3. Listados");
                Console.WriteLine("4. Reportes");
                Console.WriteLine("5. Ventanillas / Cola de prioridad / Simulación");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");
                if (!int.TryParse(Console.ReadLine(), out opcion)) opcion = -1;

                switch (opcion)
                {
                    case 1: sistema.CargarDatos(); Pausa();  break;
                    case 2: MenuRegistrar(); break;
                    case 3: MenuListados(); break;
                    case 4: MenuReportes(); break;
                    case 5: MenuVentanillas(); break;
                    case 0: Console.WriteLine("Saliendo..."); break;
                    default:
                        Console.WriteLine("Opción inválida. Presione una tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            } while (opcion != 0);
        }

        // ---------- MENÚ REGISTRAR ----------
        private void MenuRegistrar()
        {
            int op;
            do
            {
                Console.Clear();
                Console.WriteLine("\n--- REGISTRAR ---");
                Console.WriteLine("1. Registrar Cliente");
                Console.WriteLine("2. Registrar Cajero");
                Console.WriteLine("3. Registrar Servicio");
                Console.WriteLine("4. Registrar Ventanilla");
                Console.WriteLine("5. Registrar Atención (emitir ticket)");
                Console.WriteLine("0. Volver");
                Console.Write("Seleccione una opción: ");
                if (!int.TryParse(Console.ReadLine(), out op)) op = -1;

                switch (op)
                {
                    case 1: sistema.RegistrarCliente(); Pausa(); break;
                    case 2: sistema.RegistrarCajero(); Pausa(); break;
                    case 3: sistema.RegistrarServicio(); Pausa(); break;
                    case 4: sistema.RegistrarVentanilla(); Pausa(); break;
                    case 5: sistema.RegistrarAtencion(); Pausa(); break;
                    case 0: break;
                    default:
                        Console.WriteLine("Opción inválida. Presione una tecla...");
                        Console.ReadKey();
                        break;
                }
            } while (op != 0);
        }

        // ---------- MENÚ LISTADOS ----------
        private void MenuListados()
        {
            int op;
            do
            {
                Console.Clear();
                Console.WriteLine("\n--- LISTADOS ---");
                Console.WriteLine("1. Listar Clientes");
                Console.WriteLine("2. Listar Cajeros");
                Console.WriteLine("3. Listar Servicios");
                Console.WriteLine("4. Listar Ventanillas");
                Console.WriteLine("5. Listar Atenciones");
                Console.WriteLine("0. Volver");
                Console.Write("Seleccione una opción: ");
                if (!int.TryParse(Console.ReadLine(), out op)) op = -1;

                switch (op)
                {
                    case 1: sistema.ListarClientes(); Pausa(); break;
                    case 2: sistema.ListarCajeros(); Pausa(); break;
                    case 3: sistema.ListarServicios(); Pausa(); break;
                    case 4: sistema.ListarVentanillas(); Pausa(); break;
                    case 5: sistema.ListarAtenciones(); Pausa(); break;
                    case 0: break;
                    default:
                        Console.WriteLine("Opción inválida. Presione una tecla...");
                        Console.ReadKey();
                        break;
                }
            } while (op != 0);
        }

        // ---------- MENÚ REPORTES ----------
        private void MenuReportes()
        {
            int op;
            do
            {
                Console.WriteLine("\n--- REPORTES ---");
                Console.WriteLine("1. Atenciones por Servicio (entre 2 fechas)");
                Console.WriteLine("2. Totales de Retiros/Depósitos por Servicio (entre 2 fechas)");
                Console.WriteLine("3. Atenciones realizadas por Cajero (entre 2 fechas)");
                Console.WriteLine("4. Transacciones por Cliente (últimos 2 meses) -> EXPORTAR");
                Console.WriteLine("5. Reporte por Ventanilla y Atenciones -> EXPORTAR");
                Console.WriteLine("6. Atenciones por Cliente (historial)");
                Console.WriteLine("0. Volver");
                Console.Write("Seleccione una opción: ");
                if (!int.TryParse(Console.ReadLine(), out op)) op = -1;

                switch (op)
                {
                    case 1:
                        sistema.ReporteAtencionesPorServicioEntreFechas(sistema.ListaAtenciones);
                        Pausa();
                        break;
                    case 2:
                        sistema.ReporteTotalesRetirosDepositosPorServicio(sistema.ListaServicios, sistema.ListaAtenciones);
                        Pausa();
                        break;
                    case 3:
                        sistema.ReporteAtencionesPorCajero(sistema.ListaCajeros, sistema.ListaAtenciones);
                        Pausa();
                        break;
                    case 4:
                        sistema.ReporteTransaccionesPorCliente_Ultimos2Meses_Exportar(sistema.ListaAtenciones);
                        Pausa();
                        break;
                    case 5:
                        sistema.ReportePorVentanillaYAtenciones_Exportar(sistema.ListaVentanillas, sistema.ListaAtenciones);
                        Pausa();
                        break;
                    case 6:
                        
                        sistema.ReporteAtencionesPorCliente(sistema.ListaClientes, sistema.ListaAtenciones);    
                        Pausa();
                        break;

                    default:
                        Console.WriteLine("Módulo en desarrollo...");
                        Pausa();
                        break;
                }
            } while (op != 0);
        }

        // ---------- MENÚ VENTANILLAS / SIMULACIÓN ----------
        private void MenuVentanillas()
        {
            int op;
            do
            {
                Console.Clear();
                Console.WriteLine("\n--- VENTANILLAS / COLAS / SIMULACIÓN ---");
                Console.WriteLine("1. Configurar Ventanillas");
                Console.WriteLine("2. Ver Colas de Prioridad");
                Console.WriteLine("3. Encolar Clientes");
                Console.WriteLine("4. Ejecutar Simulación Completa");
                Console.WriteLine("0. Volver");
                Console.Write("Seleccione una opción: ");
                if (!int.TryParse(Console.ReadLine(), out op)) op = -1;

                switch (op)
                {
                    case 1:
                        sistema.ModificarVentanillas();
                        Pausa();
                        break;
                    case 2:
                        sistema.VerColas();
                        Pausa();
                        break;
                    case 3:
                        sistema.CargarClientesDesdeCSV();
                        Pausa();
                        break;
                    case 4:
                        sistema.EjecutarSimulacionCompleta();
                        Pausa();
                        break;
                    case 0:
                        Console.WriteLine("Saliendo...");
                        Pausa();
                        break;
                    default:
                        Console.WriteLine("Opcion no encontrada...");
                        Pausa();
                        break;
                }
            } while (op != 0);
        }

        // ---------- Helper ----------
        private void Pausa()
        {
            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey();
        }
    }
}
