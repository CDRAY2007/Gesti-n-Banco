using System;
using System.Collections.Generic;

namespace AppBanco
{
    internal class Ventanilla
    {
        private int aNroVentanilla;
        private string aDNI_Cajero;
        private string aDNI_Cliente;
        private string aNroTicket;
        private bool aPreferencial;
        private bool aAtendido;//Si esta vacia o no 
        private int aTiempoRestante;

        // Constructor
        public Ventanilla(int nro, string cajero, bool preferencial)
        {
            NroVentanilla = nro;
            DNI_Cajero = cajero;
            Preferencial = preferencial;
            DNI_Cliente = "";
            NroTicket = "";
            Atendido = false;
            TiempoRestante = 0;
        }

        public Ventanilla(int nroVentanilla, string DNI_Cajero, 
            string DNI_Cliente,string NroTicket, bool preferencial, bool atendido)
        {
            this.aNroVentanilla = nroVentanilla;
            this.aDNI_Cajero = DNI_Cajero;
            this.aDNI_Cliente = DNI_Cliente;
            this.aNroTicket = NroTicket;
            this.aPreferencial = preferencial;
            this.aAtendido = atendido;
        }

        // Setters y Getters
        public int NroVentanilla
        {
            get { return aNroVentanilla; }
            set { aNroVentanilla = value; }
        }

        public string DNI_Cajero
        {
            get { return aDNI_Cajero; }
            set { aDNI_Cajero = value; }
        }

        public string DNI_Cliente
        {
            get { return aDNI_Cliente; }
            set { aDNI_Cliente = value; }
        }

        public string NroTicket
        {
            get { return aNroTicket; }
            set { aNroTicket = value; }
        }

        public bool Preferencial
        {
            get { return aPreferencial; }
            set { aPreferencial = value; }
        }

        public bool Atendido
        {
            get { return aAtendido; }
            set { aAtendido = value; }
        }
        public int TiempoRestante
        {
            get { return aTiempoRestante; }
            set { aTiempoRestante = value; }
        }
        // Método Leer
        public void Leer()
        {
            Console.Write("Ingrese número de ventanilla: ");
            aNroVentanilla = int.Parse(Console.ReadLine());

            Console.Write("Ingrese DNI del cajero: ");
            aDNI_Cajero = Console.ReadLine();

            Console.Write("Ingrese DNI del cliente: ");
            aDNI_Cliente = Console.ReadLine();

            Console.Write("Ingrese número de ticket: ");
            aNroTicket = Console.ReadLine();

            Console.Write("¿Es preferencial? (s/n): ");
            aPreferencial = Console.ReadLine().ToLower() == "s";

            Console.Write("¿Ya fue atendido? (s/n): ");
            aAtendido = Console.ReadLine().ToLower() == "s";
        }

        // Método Mostrar
        public void Mostrar()
        {
            Console.WriteLine("===== Datos de la Ventanilla =====");
            Console.WriteLine($"Nro. Ventanilla : {aNroVentanilla}");
            Console.WriteLine($"DNI Cajero      : {aDNI_Cajero}");
            Console.WriteLine($"DNI Cliente     : {aDNI_Cliente}");
            Console.WriteLine($"Nro. Ticket     : {aNroTicket}");
            Console.WriteLine($"Preferencial    : {(aPreferencial ? "Sí" : "No")}");
            Console.WriteLine($"Atendido        : {(aAtendido ? "Sí" : "No")}");
            Console.WriteLine("=================================\n");
        }

        public override string ToString()
        {
            return $"N°: {NroVentanilla} | Cajero: {DNI_Cajero} | Cliente: {DNI_Cliente} | Ticket: {NroTicket} | " +
                   $"Preferencial: {(Preferencial ? "Sí" : "No")} | Atendido: {(Atendido ? "Sí" : "No")}";
        }

    }
}
