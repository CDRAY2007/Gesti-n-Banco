using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppBanco
{
    internal class cCola
    {
        //****Atributos*******
        private Object aElemento;
        private cCola aSubCola;
        //****Constructores*******
        public cCola()
        {
            this.aElemento = null;
            this.aSubCola = null;
        }
        public cCola(object pElemento, cCola pSubCola)
        {
            this.aElemento = pElemento;
            this.aSubCola = pSubCola;
        }
        //****Propiedades*******
        public object Elemento { get => aElemento; set => aElemento = value; }
        internal cCola SubCola { get => aSubCola; set => aSubCola = value; }
        //****Metodos*******
        public bool EstaVacia()
        {
            return Elemento == null && SubCola == null;
        }

        public void EnColar(Object pElemento)
        {
            if (EstaVacia())
            {
                aElemento = pElemento;
                return;
            }

            if (SubCola == null)
            {
                aSubCola = new cCola(pElemento, null);
                return;
            }
            SubCola.EnColar(pElemento);

        }

        public void Desencolar()
        {
            if (EstaVacia())
            {
                return;
            }

            if (aSubCola == null)
            {
                aElemento = null;
            }
            else
            {

                aElemento = aSubCola.aElemento;
                aSubCola = aSubCola.aSubCola;
            }
        }


        public void Mostrar()
        {
            if (EstaVacia())
            {
                return;
            }
            Console.WriteLine(Elemento.ToString());

            if (SubCola != null)
            {
                SubCola.Mostrar();
            }

        }





        public object Primero()
        {
            if (EstaVacia())
            {
                return null;
            }
            if (SubCola.EstaVacia())
            {
                return Elemento;
            }
            return SubCola.Primero();
        }

    }
}
