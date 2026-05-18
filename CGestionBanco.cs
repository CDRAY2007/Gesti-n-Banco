using System;
using System.IO;

namespace AppBanco
{
    internal class CGestionBanco
    {
        // ==================== LISTAS RECURSIVAS ====================
        private CLista listaClientes;
        private CLista listaCajeros;
        private CLista listaServicios;
        private CLista listaVentanillas;
        private CLista listaAtenciones;
        // ==================== COLAS ====================
        private cCola colaPreferencial;
        private cCola colaGeneral;

        // ==================== CONSTRUCTOR ====================
        public CGestionBanco()
        {
            listaClientes = new CLista();
            listaCajeros = new CLista();
            listaServicios = new CLista();
            listaVentanillas = new CLista();
            listaAtenciones = new CLista();

            colaPreferencial = new cCola();
            colaGeneral = new cCola();
        }

        /// <summary>
        /// METODOS GET Y SET
        /// </summary>


        public CLista ListaClientes { get => listaClientes; set => listaClientes = value; }
        public CLista ListaCajeros { get => listaCajeros; set => listaCajeros = value; }
        public CLista ListaServicios { get => listaServicios; set => listaServicios = value; }
        public CLista ListaVentanillas { get => listaVentanillas; set => listaVentanillas = value; }
        public CLista ListaAtenciones { get => listaAtenciones; set => listaAtenciones = value; }

        public cCola ColaPreferencial { get => colaPreferencial; }
        public cCola ColaGeneral { get => colaGeneral; }


        static bool pausado = false;
        static bool salir = false;
        static Random rnd = new Random();

        static CLista atencionesEnCurso = new CLista();
        public void CargarDatos()
        {
            // ---------- Clientes ----------
            //listaClientes.Agregar(new CClientes("12345678", "Juan Pérez", new DateTime(1990, 5, 20), false, false, "juanperez@mail.com", "987654321", 1500.0));
            //listaClientes.Agregar(new CClientes("87654321", "María López", new DateTime(1985, 3, 15), true, true, "marialopez@mail.com", "912345678", 3000.0));
            //listaClientes.Agregar(new CClientes("45678912", "Carlos Gómez", new DateTime(2000, 11, 10), false, false, "cgomez@mail.com", "934567890", 750.0));

            // ---------- Cajeros ----------
            listaCajeros.Agregar(new CCajero("11112222", "Luis Ramírez", "Av. Principal 123", "lramirez@banco.com", "987112233"));
            listaCajeros.Agregar(new CCajero("33334444", "Ana Torres", "Calle Secundaria 456", "atorres@banco.com", "987445566"));
            listaCajeros.Agregar(new CCajero("44445555", "Pedro Castillo", "Jr. Los Álamos 789", "pcastillo@banco.com", "987556677"));
            listaCajeros.Agregar(new CCajero("55556666", "Lucía Fernández", "Av. Los Olivos 321", "lfernandez@banco.com", "987667788"));
            listaCajeros.Agregar(new CCajero("66667777", "Miguel Herrera", "Calle Las Flores 654", "mherrera@banco.com", "987778899"));
            listaCajeros.Agregar(new CCajero("77778888", "Rosa Delgado", "Pasaje Central 852", "rdelgado@banco.com", "987889900"));
            listaCajeros.Agregar(new CCajero("88889999", "Jorge Silva", "Av. La Cultura 963", "jsilva@banco.com", "987990011"));

            // ---------- Servicios ----------
            listaServicios.Agregar(new Servicios("S001", "Depósitos"));
            listaServicios.Agregar(new Servicios("S002", "Retiros"));
            listaServicios.Agregar(new Servicios("S003", "Pago de Servicios"));
            //-------------Ventanillas---------
            listaVentanillas.Agregar(new Ventanilla(1, "11112222", "", "", true, false));
            listaVentanillas.Agregar(new Ventanilla(2, "33334444", "", "", false, false));
            listaVentanillas.Agregar(new Ventanilla(3, "11112222", "", "", false, false));
            listaVentanillas.Agregar(new Ventanilla(4, "33334444", "", "", false, false));
            listaVentanillas.Agregar(new Ventanilla(5, "11112222", "", "", false, false));

            Console.WriteLine("Datos cargados");
        }

        //Opcion 1
        public void ModificarVentanillas()
        {
            if (listaVentanillas.EstaVacia())
            {
                Console.WriteLine("No hay ventanillas configuradas todavía.");
                return;
            }

            Console.WriteLine("\n=== MODIFICAR VENTANILLAS ===");
            listaVentanillas.Mostrar();

            Console.Write("Seleccione número de ventanilla a modificar (0 = salir): ");
            if (!int.TryParse(Console.ReadLine(), out int nro) || nro == 0) return;

            Ventanilla v = (Ventanilla)listaVentanillas.Iesimo(nro);

            Console.WriteLine($"\n--- Modificando Ventanilla {v.NroVentanilla} ---");

            // Mostrar cajeros disponibles
            Console.WriteLine("Cajeros disponibles:");
            ListaCajeros.Mostrar();

            Console.Write("Ingrese nuevo DNI de cajero: ");
            string nuevoCajero = Console.ReadLine();
            v.DNI_Cajero = nuevoCajero;

            // Reemplazar en lista
            listaVentanillas.Eliminar(nro);
            listaVentanillas.Insertar(nro, v);

            Console.WriteLine("Ventanilla modificada correctamente.");
        }

        //Opcion 2
        public void VerColas()
        {
            Console.WriteLine("\n=== ESTADO DE LAS COLAS ===");

            Console.WriteLine("\nCola Preferencial:");
            if (colaPreferencial.EstaVacia())
                Console.WriteLine("Vacía");
            else
                colaPreferencial.Mostrar();

            Console.WriteLine("\nCola General:");
            if (colaGeneral.EstaVacia())
                Console.WriteLine("Vacía");
            else
                colaGeneral.Mostrar();
        }

        //Encolar desde .csv
        public void CargarClientesDesdeCSV()
        {
            Console.Write("Ingrese la ruta del archivo CSV: ");
            string ruta = Console.ReadLine();

            if (!File.Exists(ruta))
            {
                Console.WriteLine("Archivo no encontrado.");
                return;
            }

            string[] lineas = File.ReadAllLines(ruta);

            Console.WriteLine($"Leyendo {lineas.Length} clientes desde {ruta}...");

            // Iniciar recursión
            ProcesarLineasCSV(lineas, 0);

            Console.WriteLine("Clientes cargados en las colas.");
        }

        // Función recursiva que procesa cada línea del CSV
        public void ProcesarLineasCSV(string[] lineas, int index)
        {
            if (index >= lineas.Length) return;

            string[] datos = lineas[index].Split(',');

            if (datos.Length >= 8)
            {
                CClientes c = new CClientes(
                    dni: datos[0],
                    nombres: datos[1],
                    fechaNacimiento: DateTime.Parse(datos[2]),
                    discapacidad: datos[3].Trim().ToUpper() == "S",
                    ninos: datos[4].Trim().ToUpper() == "S",
                    email: datos[5],
                    telefono: datos[6],
                    monto: double.Parse(datos[7])
                );
                //Agregamos a la lista clientes
                listaClientes.Agregar(c);
                //Encolamos a las colas
                if (c.EsVentanillaPreferencial())
                    colaPreferencial.EnColar(c);
                else
                    colaGeneral.EnColar(c);
            }

            ProcesarLineasCSV(lineas, index + 1);
        }
        public void CargarClienteManual()
        {
            Console.WriteLine("\n--- INGRESAR CLIENTE MANUAL ---");
            CClientes nuevo = new CClientes();
            nuevo.Leer();

            if (nuevo.EsVentanillaPreferencial())
                colaPreferencial.EnColar(nuevo);
            else
                colaGeneral.EnColar(nuevo);

            Console.WriteLine("Cliente encolado correctamente.");
        }
        //A partir de la lista
        public void EncolarClientesRec(CLista clientes, int pos)
        {
            if (pos > clientes.Longitud()) return;

            CClientes c = (CClientes)clientes.Iesimo(pos);

            if (c.EsVentanillaPreferencial())
                colaPreferencial.EnColar(c);
            else
                colaGeneral.EnColar(c);

            EncolarClientesRec(clientes, pos + 1);
        }

        // ================= OPCIÓN 4 DEL MENÚ =================
        public void EjecutarSimulacionCompleta()
        {
            Console.WriteLine("\nIniciando simulación...");
            Console.WriteLine("5 ventanillas: 1 preferencial + 4 normales.");
            Console.WriteLine("Cada 10 segundos: (p) pausar | (c) continuar | (s) salir.");


            // Iniciar recursión
            SimularTick(listaVentanillas, 1);
        }

        // ================= TICK DE SIMULACIÓN =================
        public void SimularTick(CLista ventanillas, int tick)
        {
            if (salir) return;

            Console.WriteLine($"\nTick {tick}");

            // Procesar ventanillas
            ProcesarVentanillas(ventanillas);

            // Cada 10 segundos, preguntar
            if (tick % 10 == 0)
            {
                Console.WriteLine("\nOpciones: (p) pausar | (c) continuar | (s) salir");
                string cmd = Console.ReadLine()?.Trim().ToLower();

                if (cmd == "p") pausado = true;
                if (cmd == "s") { salir = true; return; }
                if (cmd == "c") pausado = false;
            }

            // Pausa
            if (pausado)
            {
                Console.WriteLine("Simulación pausada. Escriba 'c' para continuar.");
                string cmd;
                do
                {
                    cmd = Console.ReadLine()?.Trim().ToLower();
                } while (cmd != "c");
                pausado = false;
            }

            // Esperar 1 segundo real
            System.Threading.Thread.Sleep(1000);

            // Recursión siguiente tick
            SimularTick(ventanillas, tick + 1);
        }

        // ================= PROCESAR VENTANILLAS =================
        public void ProcesarVentanillas(CLista ventanillas)
        {
            if (ventanillas.EstaVacia()) return;

            Ventanilla v = (Ventanilla)ventanillas.Elemento;

            if (v.Atendido) // ya tiene cliente
            {
                v.TiempoRestante--;
                if (v.TiempoRestante <= 0)
                {
                    Console.WriteLine($"Ventanilla {v.NroVentanilla} terminó con cliente {v.DNI_Cliente}");

                    // Buscar la atención en curso por ticket
                    int pos = BuscarAtencionPorTicketRec(v.NroTicket, atencionesEnCurso);
                    if (pos > 0)
                    {
                        Atenciones finalizada = (Atenciones)atencionesEnCurso.Iesimo(pos);
                        listaAtenciones.Agregar(finalizada);  // mover a finalizadas
                        atencionesEnCurso.Eliminar(pos);      // eliminar de en curso
                    }

                    // Liberar ventanilla
                    v.Atendido = false;
                    v.DNI_Cliente = "";
                    v.NroTicket = "";
                }
                else
                {
                    Console.WriteLine($"Ventanilla {v.NroVentanilla} atendiendo {v.DNI_Cliente} (faltan {v.TiempoRestante}s)");
                }
            }
            else // ventanilla libre
            {
                if (v.Preferencial && !colaPreferencial.EstaVacia())
                {
                    AsignarClienteAVentanilla(v, (CClientes)colaPreferencial.Elemento, colaPreferencial);
                    colaPreferencial.Desencolar();
                }
                else if (!v.Preferencial && !colaGeneral.EstaVacia())
                {
                    AsignarClienteAVentanilla(v, (CClientes)colaGeneral.Elemento, colaGeneral);
                    colaGeneral.Desencolar();
                }
            }

            // Recursión a la sublista
            ProcesarVentanillas(ventanillas.SubLista);
        }

        // ================= ASIGNAR CLIENTE A VENTANILLA =================
        public void AsignarClienteAVentanilla(Ventanilla v, CClientes c, cCola cola)
        {
            v.DNI_Cliente = c.DNI;
            v.NroTicket = "T-" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper(); ;
            v.TiempoRestante = ObtenerTiempoAtencion(c);
            v.Atendido = true;

            // Elegir servicio y monto random
            Servicios servicio = (Servicios)listaServicios.Iesimo(rnd.Next(1, listaServicios.Longitud() + 1));
            float monto = rnd.Next(50, 10001); // entre 50 y 10 000

            // Crear atención y guardarla en "en curso"
            Atenciones at = new Atenciones(
                nroTicket: v.NroTicket,
                fechaHora: DateTime.Now,
                idCliente: c.DNI,
                idCajero: v.DNI_Cajero,
                idServicio: servicio.IdServicio,
                monto: monto,
                segundos: v.TiempoRestante
            );
            atencionesEnCurso.Agregar(at);

            Console.WriteLine($"Ventanilla {v.NroVentanilla} tomó cliente {c.Nombres} , Servicio: {servicio.IdServicio}, Monto: {monto}");
        }

        // ================= UTILIDADES =================
        public int ObtenerTiempoAtencion(CClientes c)
        {
            if (c.Niños) return 5;
            if (c.CalcularEdad() >= 60) return 7;
            if (c.Discapacidad) return 6;
            return rnd.Next(3, 6); // entre 3 y 5
        }

        // Buscar atención en curso por ticket
        public int BuscarAtencionPorTicketRec(string ticket, CLista lista, int pos = 1)
        {
            if (lista.EstaVacia()) return 0;

            Atenciones actual = (Atenciones)lista.Elemento;
            if (actual.NroTicket == ticket) return pos;

            return BuscarAtencionPorTicketRec(ticket, lista.SubLista, pos + 1);
        }


        // =======================================================
        // ========== MÓDULOS DE REGISTRO ========================
        // =======================================================

        #region Registrar Cliente
        public void RegistrarCliente()
        {
            Console.Clear();
            Console.WriteLine("=== REGISTRO DE CLIENTE ===");

            CClientes nuevo = new CClientes();
            nuevo.DNI = ValidarDniUnicoCliente();
            nuevo.Nombres = LeerTextoNoVacio("Ingrese nombres: ");
            nuevo.FechaNacimiento = ValidarFechaNacimiento();
            nuevo.Discapacidad = LeerSN("¿Tiene discapacidad? (S/N): ");
            nuevo.Niños = LeerSN("¿Tiene niños a su cargo? (S/N): ");
            nuevo.Email = ValidarEmailEntrada();
            nuevo.Telefono = ValidarTelefonoEntrada();
            nuevo.Monto = ValidarMontoEntrada("Ingrese monto inicial: ");

            listaClientes.Agregar(nuevo);
            Console.WriteLine("Cliente registrado exitosamente.");
            Pausa();
        }
        #endregion

        #region Registrar Cajero
        public void RegistrarCajero()
        {
            Console.Clear();
            Console.WriteLine("=== REGISTRO DE CAJERO ===");

            CCajero nuevo = new CCajero();

            nuevo.DNI = ValidarDniUnicoCajero();
            nuevo.Nombres = LeerTextoNoVacio("Ingrese nombres: ");
            nuevo.Direccion = LeerTextoNoVacio("Ingrese dirección: ");
            nuevo.Email = ValidarEmailEntrada();
            nuevo.Telefono = ValidarTelefonoEntrada();

            listaCajeros.Agregar(nuevo);
            Console.WriteLine("Cajero registrado exitosamente.");
            Pausa();
        }
        #endregion

        #region Registrar Servicio
        public void RegistrarServicio()
        {
            Console.Clear();
            Console.WriteLine("=== REGISTRO DE SERVICIO ===");

            Servicios nuevo = new Servicios();

            nuevo.IdServicio = ValidarIdServicioUnico();
            nuevo.Descripcion = LeerTextoNoVacio("Ingrese descripción del servicio: ").ToUpper();

            listaServicios.Agregar(nuevo);
            Console.WriteLine("Servicio registrado exitosamente.");
            Pausa();
        }
        #endregion

        #region Registrar Ventanilla
        public void RegistrarVentanilla()
        {
            Console.Clear();
            Console.WriteLine("=== REGISTRO DE VENTANILLA ===");

            int nroVentanilla = ValidarNumeroVentanillaUnico();

            if (listaCajeros.EstaVacia())
            {
                Console.WriteLine("No hay cajeros registrados. Registre uno primero.");
                Pausa();
                return;
            }

            Console.WriteLine("\n--- Seleccione Cajero ---");
            ListarCajeros();

            string dniCajero;
            do
            {
                Console.Write("Ingrese DNI del cajero asignado: ");
                dniCajero = Console.ReadLine();
                if (!ExisteCajero(dniCajero))
                {
                    Console.WriteLine("Cajero no encontrado. Intente nuevamente.");
                    dniCajero = "";
                }
            } while (string.IsNullOrEmpty(dniCajero));

            bool preferencial = LeerSN("¿Será una ventanilla preferencial? (S/N): ");

            Ventanilla nueva = new Ventanilla(nroVentanilla, dniCajero, "", "", preferencial, false);
            listaVentanillas.Agregar(nueva);

            Console.WriteLine("Ventanilla registrada exitosamente.");
            Pausa();
        }
        #endregion

        #region Registrar Atención
        public void RegistrarAtencion()
        {
            Console.Clear();
            Console.WriteLine("=== REGISTRO DE ATENCIÓN ===");

            string nroTicket = ValidarNroTicketUnico();

            if (listaClientes.EstaVacia())
            {
                Console.WriteLine("No hay clientes registrados.");
                Pausa();
                return;
            }
            Console.WriteLine("\n--- Clientes disponibles ---");
            ListarClientes();

            string idCliente;
            do
            {
                Console.Write("Ingrese DNI del cliente: ");
                idCliente = Console.ReadLine();
                if (!ExisteCliente(idCliente))
                {
                    Console.WriteLine("Cliente no encontrado.");
                    idCliente = "";
                }
            } while (string.IsNullOrEmpty(idCliente));

            if (listaCajeros.EstaVacia())
            {
                Console.WriteLine("No hay cajeros registrados.");
                Pausa();
                return;
            }
            Console.WriteLine("\n--- Cajeros disponibles ---");
            ListarCajeros();
            string idCajero;
            do
            {
                Console.Write("Ingrese DNI del cajero: ");
                idCajero = Console.ReadLine();
                if (!ExisteCajero(idCajero))
                {
                    Console.WriteLine("Cajero no encontrado.");
                    idCajero = "";
                }
            } while (string.IsNullOrEmpty(idCajero));

            if (listaServicios.EstaVacia())
            {
                Console.WriteLine("No hay servicios registrados.");
                Pausa();
                return;
            }
            Console.WriteLine("\n--- Servicios disponibles ---");
            ListarServicios();
            string idServicio;
            do
            {
                Console.Write("Ingrese ID del servicio: ");
                idServicio = Console.ReadLine();
                if (!ExisteServicio(idServicio))
                {
                    Console.WriteLine("Servicio no encontrado.");
                    idServicio = "";
                }
            } while (string.IsNullOrEmpty(idServicio));

            float monto;
            do
            {
                Console.Write("Ingrese monto de la operación: ");
            } while (!float.TryParse(Console.ReadLine(), out monto) || monto < 0);

            int segundos;
            do
            {
                Console.Write("Ingrese duración de atención (segundos): ");
            } while (!int.TryParse(Console.ReadLine(), out segundos) || segundos <= 0);

            Atenciones nueva = new Atenciones(nroTicket, DateTime.Now, idCliente, idCajero, idServicio, monto, segundos);
            listaAtenciones.Agregar(nueva);

            Console.WriteLine("Atención registrada correctamente.");
            Pausa();
        }
        #endregion

        // =======================================================
        // ========== LISTADOS (RECURSIVOS) ======================
        // =======================================================
        public void ListarClientes()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE CLIENTES ===");
            if (listaClientes.EstaVacia())
                Console.WriteLine("No hay clientes registrados.");
            else
                listaClientes.Mostrar();
            Pausa();
        }

        public void ListarCajeros()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE CAJEROS ===");
            if (listaCajeros.EstaVacia())
                Console.WriteLine("No hay cajeros registrados.");
            else
                listaCajeros.Mostrar();
            Pausa();
        }

        public void ListarServicios()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE SERVICIOS ===");
            if (listaServicios.EstaVacia())
                Console.WriteLine("No hay servicios registrados.");
            else
                listaServicios.Mostrar();
            Pausa();
        }

        public void ListarVentanillas()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE VENTANILLAS ===");
            if (listaVentanillas.EstaVacia())
                Console.WriteLine("No hay ventanillas registradas.");
            else
                listaVentanillas.Mostrar();
            Pausa();
        }

        public void ListarAtenciones()
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO DE ATENCIONES ===");
            if (listaAtenciones.EstaVacia())
                Console.WriteLine("No hay atenciones registradas.");
            else
                listaAtenciones.Mostrar();
            Pausa();
        }

        // =======================================================
        // ========== VALIDACIONES ===============================
        // =======================================================
        #region Validaciones DNI
        private string ValidarDniUnicoCliente()
        {
            Console.Write("Ingrese DNI (8 dígitos): ");
            string dni = Console.ReadLine();

            if (CBiblioteca.ValidarDNI(dni) && !ExisteCliente(dni))
                return dni;

            Console.WriteLine("DNI inválido o ya registrado. Intente de nuevo.");
            return ValidarDniUnicoCliente(); // llamada recursiva
        }


        private string ValidarDniUnicoCajero()
        {
            Console.Write("Ingrese DNI (8 dígitos): ");
            string dni = Console.ReadLine();

            if (CBiblioteca.ValidarDNI(dni) && !ExisteCajero(dni))
                return dni;

            Console.WriteLine("❌ DNI inválido o ya registrado. Intente de nuevo.");
            return ValidarDniUnicoCajero(); // llamada recursiva
        }
        #endregion

        #region Validar Servicio
        private string ValidarIdServicioUnico()
        {
            Console.Write("Ingrese ID del servicio: ");
            string id = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(id) && !ExisteServicio(id))
                return id;

            Console.WriteLine("ID inválido o ya existente. Intente de nuevo.");
            return ValidarIdServicioUnico();
        }

        #endregion

        #region Validaciones Generales
        private string LeerTextoNoVacio(string mensaje)
        {
            Console.Write(mensaje);
            string texto = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(texto))
                return CBiblioteca.ConvertirMayusculas(texto);

            return LeerTextoNoVacio(mensaje);
        }


        private bool LeerSN(string mensaje)
        {
            Console.Write(mensaje);
            string opcion = Console.ReadLine().ToUpper();

            if (opcion == "S") return true;
            if (opcion == "N") return false;

            Console.WriteLine("Opción inválida. Responda con S o N.");
            return LeerSN(mensaje);
        }


        private DateTime ValidarFechaNacimiento()
        {
            Console.Write("Ingrese fecha de nacimiento (dd/mm/yyyy): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime fecha) && fecha < DateTime.Now)
                return fecha;

            Console.WriteLine("Fecha no válida. Intente de nuevo.");
            return ValidarFechaNacimiento();
        }


        private string ValidarEmailEntrada()
        {
            Console.Write("Ingrese email: ");
            string email = Console.ReadLine();

            if (CBiblioteca.ValidarEmail(email))
                return email;

            Console.WriteLine("❌ Email inválido. Intente de nuevo.");
            return ValidarEmailEntrada(); // llamada recursiva
        }

        private string ValidarTelefonoEntrada()
        {
            Console.Write("Ingrese teléfono (9 dígitos): ");
            string telefono = Console.ReadLine();

            if (CBiblioteca.ValidarTelefono(telefono))
                return telefono;

            Console.WriteLine("❌ Teléfono inválido. Intente de nuevo.");
            return ValidarTelefonoEntrada(); // llamada recursiva
        }


        private double ValidarMontoEntrada(string mensaje)
        {
            Console.Write(mensaje);
            if (double.TryParse(Console.ReadLine(), out double monto) && monto >= 0)
                return monto;

            Console.WriteLine("Monto inválido. Debe ser un número no negativo.");
            return ValidarMontoEntrada(mensaje);
        }

        #endregion

        // =======================================================
        // ========== EXISTENCIA / BÚSQUEDAS ======================
        // =======================================================
        private bool ExisteCliente(string dni)
        {
            return ExisteClienteRec(dni, 1);
        }

        private bool ExisteClienteRec(string dni, int index)
        {
            if (index > listaClientes.Longitud())
                return false;

            CClientes c = (CClientes)listaClientes.Iesimo(index);
            if (c.DNI == dni)
                return true;

            return ExisteClienteRec(dni, index + 1);
        }

        // ===================== EXISTE CAJERO =====================
        private bool ExisteCajero(string dni)
        {
            return ExisteCajeroRec(dni, 1);
        }

        private bool ExisteCajeroRec(string dni, int index)
        {
            if (index > listaCajeros.Longitud())
                return false;

            CCajero c = (CCajero)listaCajeros.Iesimo(index);
            if (c.DNI == dni)
                return true;

            return ExisteCajeroRec(dni, index + 1);
        }

        // ===================== EXISTE SERVICIO =====================
        private bool ExisteServicio(string id)
        {
            return ExisteServicioRec(id, 1);
        }

        private bool ExisteServicioRec(string id, int index)
        {
            if (index > listaServicios.Longitud())
                return false;

            Servicios s = (Servicios)listaServicios.Iesimo(index);
            if (s.IdServicio.Equals(id, StringComparison.OrdinalIgnoreCase))
                return true;

            return ExisteServicioRec(id, index + 1);
        }

        // ===================== VALIDAR VENTANILLA =====================
        private int ValidarNumeroVentanillaUnico()
        {
            int numero;
            do
            {
                Console.Write("Ingrese número de ventanilla: ");
            } while (!int.TryParse(Console.ReadLine(), out numero) || numero <= 0 || ExisteVentanilla(numero));
            return numero;
        }

        private bool ExisteVentanilla(int numero)
        {
            return ExisteVentanillaRec(numero, 1);
        }

        private bool ExisteVentanillaRec(int numero, int index)
        {
            if (index > listaVentanillas.Longitud())
                return false;

            Ventanilla v = (Ventanilla)listaVentanillas.Iesimo(index);
            if (v.NroVentanilla == numero)
                return true;

            return ExisteVentanillaRec(numero, index + 1);
        }

        // ===================== VALIDAR TICKET =====================
        private string ValidarNroTicketUnico()
        {
            string nro;
            do
            {
                Console.Write("Ingrese número de ticket: ");
                nro = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(nro) || ExisteTicket(nro));
            return nro;
        }

        private bool ExisteTicket(string nro)
        {
            return ExisteTicketRec(nro, 1);
        }

        private bool ExisteTicketRec(string nro, int index)
        {
            if (index > listaAtenciones.Longitud())
                return false;

            Atenciones a = (Atenciones)listaAtenciones.Iesimo(index);
            if (a.NroTicket.Equals(nro, StringComparison.OrdinalIgnoreCase))
                return true;

            return ExisteTicketRec(nro, index + 1);
        }

        /// <summary>
        /// REPORTES GOZZUS
        /// </summary>

        //REPORTE DE ATENCIONES POR SERVICIO SEGUN ENTRE FECHAS
        public void ReporteAtencionesPorServicioEntreFechas(CLista listaAtenciones)
        {
            Console.Write("Ingrese ID del servicio: ");
            string idServicio = Console.ReadLine();

            Console.Write("Ingrese fecha inicial (dd/mm/yyyy): ");
            DateTime fechaInicio = DateTime.Parse(Console.ReadLine());

            Console.Write("Ingrese fecha final (dd/mm/yyyy): ");
            DateTime fechaFin = DateTime.Parse(Console.ReadLine());

            Console.WriteLine($"\n======= Reporte de Atenciones - Servicio: {idServicio} =======");
            ReporteAtencionesPorServicioEntreFechasRecursivo(listaAtenciones, idServicio, fechaInicio, fechaFin);
        }
        public void ReporteAtencionesPorServicioEntreFechasRecursivo(CLista lista, string idServicio, DateTime inicio, DateTime fin)
        {
            if (lista == null || lista.EstaVacia())
            {
                return;
            }
            Atenciones ATENCION = (Atenciones)lista.Elemento;
            if (ATENCION.IdServicio == idServicio && ATENCION.FechaHora >= inicio && ATENCION.FechaHora <= fin)
            {
                Console.WriteLine(
                        $"Ticket: {ATENCION.NroTicket}\n" +
                        $"Cliente: {ATENCION.IdCliente}\n" +
                        $"Cajero: {ATENCION.IdCajero}\n" +
                        $"Fecha: {ATENCION.FechaHora}\n" +
                        $"Monto: S/ {ATENCION.Monto}"
                );
            }

            if (lista.SubLista != null)
                ReporteAtencionesPorServicioEntreFechasRecursivo(lista.SubLista, idServicio, inicio, fin);
        }


        //REPORTES TOTALES RETIROS  o DEPOSITOS POR SERVICIO
        public void ReporteTotalesRetirosDepositosPorServicio(CLista listaServicios, CLista listaAtenciones)
        {
            Console.WriteLine("\n======= Totales por Servicio ========");
            ReporteTotalesPorServicioRecursivo(listaServicios, listaAtenciones);
        }

        private void ReporteTotalesPorServicioRecursivo(CLista listaServicios, CLista listaAtenciones)
        {
            if (listaServicios == null || listaServicios.EstaVacia())
            {
                return;
            }
            Servicios servicio = (Servicios)listaServicios.Elemento;
            double total = CalcularTotalPorServicio(listaAtenciones, servicio.IdServicio);

            Console.WriteLine($"Servicio: {servicio.Descripcion} ({servicio.IdServicio}) - Total Recaudado: S/ {total:F2}");

            if (listaServicios.SubLista != null)
            {
                ReporteTotalesPorServicioRecursivo(listaServicios.SubLista, listaAtenciones);
            }
        }

        private double CalcularTotalPorServicio(CLista listaAtenciones, string idServicio)
        {
            if (listaAtenciones == null || listaAtenciones.EstaVacia())
            {
                return 0f;
            }
            Atenciones atencion = (Atenciones)listaAtenciones.Elemento;
            double subtotal = (atencion.IdServicio == idServicio) ? (double)atencion.Monto : 0f;

            return subtotal + CalcularTotalPorServicio(listaAtenciones.SubLista, idServicio);
        }



        //REPORTE ATENCIONES POR CAJERO
        public void ReporteAtencionesPorCajero(CLista listaCajeros, CLista listaAtenciones)
        {
            Console.WriteLine("\n=== Reporte de Atenciones por Cajero ===");
            ReporteAtencionesPorCajeroRecursivo(listaCajeros, listaAtenciones);
        }

        private void ReporteAtencionesPorCajeroRecursivo(CLista listaCajeros, CLista listaAtenciones)
        {
            if (listaCajeros == null || listaCajeros.EstaVacia()) return;

            CCajero cajero = (CCajero)listaCajeros.Elemento;
            int totalAtenciones = ContarAtencionesPorCajero(listaAtenciones, cajero.DNI);
            double totalMonto = CalcularMontoPorCajero(listaAtenciones, cajero.DNI);

            Console.WriteLine($"Cajero: {cajero.Nombres} ({cajero.DNI}) - Total Atenciones: {totalAtenciones} - Total Monto: S/ {totalMonto:F2}");

            if (listaCajeros.SubLista != null)
                ReporteAtencionesPorCajeroRecursivo(listaCajeros.SubLista, listaAtenciones);
        }

        private int ContarAtencionesPorCajero(CLista lista, string idCajero)
        {
            if (lista == null || lista.EstaVacia()) return 0;

            Atenciones ATENCION = (Atenciones)lista.Elemento;
            int count = (ATENCION.IdCajero == idCajero) ? 1 : 0;

            return count + ContarAtencionesPorCajero(lista.SubLista, idCajero);
        }

        private double CalcularMontoPorCajero(CLista lista, string idCajero)
        {
            if (lista == null || lista.EstaVacia()) return 0f;

            Atenciones ATENCION = (Atenciones)lista.Elemento;
            double total = (ATENCION.IdCajero == idCajero) ? ATENCION.Monto : 0f;

            return total + CalcularMontoPorCajero(lista.SubLista, idCajero);
        }


        //REPORTE TRANSACCIONES DE ACUERDO AL CLIENTE EN LOS ULTIMOS 2 MEESE
        public void ReporteTransaccionesPorCliente_Ultimos2Meses_Exportar(CLista listaAtenciones)
        {
            DateTime limite = DateTime.Now.AddMonths(-2);
            string ruta = "Reporte_Transacciones_Ultimos2Meses.txt";

            using (StreamWriter sw = new StreamWriter(ruta))
            {
                sw.WriteLine("=== Transacciones por Cliente (Últimos 2 meses) ===");
                ExportarTransaccionesRecursivo(listaAtenciones, limite, sw);
            }

            Console.WriteLine($"Reporte exportado correctamente a: {ruta}");
        }

        private void ExportarTransaccionesRecursivo(CLista lista, DateTime limite, StreamWriter sw)
        {
            if (lista == null || lista.EstaVacia()) return;

            Atenciones ATENCION = (Atenciones)lista.Elemento;
            if (ATENCION.FechaHora >= limite)
            {
                sw.WriteLine($"Cliente: {ATENCION.IdCliente} | Ticket: {ATENCION.NroTicket} | Servicio: {ATENCION.IdServicio} | Monto: {ATENCION.Monto} | Fecha: {ATENCION.FechaHora}");
            }

            if (lista.SubLista != null)
                ExportarTransaccionesRecursivo(lista.SubLista, limite, sw);
        }


        //REPORTE POR VENTANILLA Y POR ATENCIONES 
        public void ReportePorVentanillaYAtenciones_Exportar(CLista listaVentanillas, CLista listaAtenciones)
        {
            string ruta = "Reporte_Ventanillas.txt";

            using (StreamWriter sw = new StreamWriter(ruta))
            {
                sw.WriteLine("=== Reporte por Ventanilla ===");
                ReporteVentanillaRecursivo(listaVentanillas, listaAtenciones, sw);
            }

            Console.WriteLine($"Reporte exportado correctamente a: {ruta}");
        }

        private void ReporteVentanillaRecursivo(CLista listaVentanillas, CLista listaAtenciones, StreamWriter sw)
        {
            if (listaVentanillas == null || listaVentanillas.EstaVacia())
            {
                return;
            }
            Ventanilla VENTANILLA = (Ventanilla)listaVentanillas.Elemento;
            int total = ContarAtencionesPorVentanilla(listaAtenciones, VENTANILLA.NroTicket);

            sw.WriteLine($"Ventanilla {VENTANILLA.NroVentanilla} (Preferencial: {(VENTANILLA.Preferencial ? "Sí" : "No")}) - Total Atenciones: {total}");

            if (listaVentanillas.SubLista != null)
                ReporteVentanillaRecursivo(listaVentanillas.SubLista, listaAtenciones, sw);
        }

        private int ContarAtencionesPorVentanilla(CLista lista, string nroTicket)
        {
            if (lista == null || lista.EstaVacia())
            {
                return 0;
            }
            Atenciones ATENCION = (Atenciones)lista.Elemento;
            int count = (ATENCION.NroTicket == nroTicket) ? 1 : 0;

            return count + ContarAtencionesPorVentanilla(lista.SubLista, nroTicket);
        }



        //REPORTES POR ATENCIONES POR CLIENTE
        public void ReporteAtencionesPorCliente(CLista listaClientes, CLista listaAtenciones)
        {
            Console.WriteLine("\n=== Reporte de Atenciones por Cliente ===");
            ReporteClienteRecursivo(listaClientes, listaAtenciones);
        }

        private void ReporteClienteRecursivo(CLista listaClientes, CLista listaAtenciones)
        {
            if (listaClientes == null || listaClientes.EstaVacia()) return;

            CClientes cliente = (CClientes)listaClientes.Elemento;
            int totalAtenciones = ContarAtencionesPorCliente(listaAtenciones, cliente.DNI);
            double totalMonto = CalcularMontoPorCliente(listaAtenciones, cliente.DNI);

            Console.WriteLine($"Cliente: {cliente.Nombres} ({cliente.DNI}) - Total Atenciones: {totalAtenciones} - Total Monto: S/ {totalMonto:F2}");

            if (listaClientes.SubLista != null)
                ReporteClienteRecursivo(listaClientes.SubLista, listaAtenciones);
        }

        private int ContarAtencionesPorCliente(CLista lista, string idCliente)
        {
            if (lista == null || lista.EstaVacia()) return 0;

            Atenciones ATENCION = (Atenciones)lista.Elemento;
            int count = (ATENCION.IdCliente == idCliente) ? 1 : 0;

            return count + ContarAtencionesPorCliente(lista.SubLista, idCliente);
        }

        private double CalcularMontoPorCliente(CLista lista, string idCliente)
        {
            if (lista == null || lista.EstaVacia()) return 0f;

            Atenciones ATENCION = (Atenciones)lista.Elemento;
            double total = (ATENCION.IdCliente == idCliente) ? ATENCION.Monto : 0f;

            return total + CalcularMontoPorCliente(lista.SubLista, idCliente);
        }

        // =======================================================
        // ========== UTILIDADES ================================
        // =======================================================
        private void Pausa()
        {
            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey();
        }
    }
}
