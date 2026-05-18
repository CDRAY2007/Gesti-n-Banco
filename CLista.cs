using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBanco
{
    internal class CLista
    {
        private Object aElemento;
        private CLista aSubLista;

        public CLista()
        {
            aElemento = null;
            aSubLista = null;
        }

        public CLista(Object pElemento, CLista pSubLista)
        {
            aElemento = pElemento;
            aSubLista = pSubLista;
        }

        public object Elemento { get => aElemento; set => aElemento = value; }
        internal CLista SubLista { get => aSubLista; set => aSubLista = value; }




        public bool EstaVacia()
        {
            return aElemento == null && aSubLista == null;
        }

        public int Longitud()
        {
            if (EstaVacia())
                return 0;
            else
                return 1 + aSubLista.Longitud();
        }

        public void Agregar(Object pElemento)
        {
            if (EstaVacia())
            {
                Elemento = pElemento;
                SubLista = new CLista();
            }
            else
            {
                SubLista.Agregar(pElemento);
            }

        }

        public void Mostrar()
        {
            if (!EstaVacia())
            {
                Console.WriteLine(Elemento.ToString());
                SubLista?.Mostrar();
            }
        }

        public void Eliminar(int posicion)
        {
            if (EstaVacia())
            {
                Console.WriteLine("No  hay datos");
                return;
            }
            if (posicion < 0 || posicion > Longitud())
            {
                Console.WriteLine("Posicion inexistente");
                return;
            }
            if (posicion == 1)
            {
                Elemento = SubLista.Elemento;
                SubLista = SubLista.SubLista;
            }
            else
            {
                SubLista.Eliminar(posicion - 1);
            }
        }

        public int Ubicacion(Object pElemento)
        {
            if (EstaVacia())
            {
                return 0;
            }
            else
            {
                return Elemento.Equals(pElemento) ? 1 : 1 + SubLista.Ubicacion(pElemento);
            }
        }


        public Object Iesimo(int posicion)
        {
            if (posicion == 1)
            {
                return Elemento;
            }
            else
            {
                return SubLista.Iesimo(posicion - 1);
            }
        }

        public void Insertar(int posicion, Object pElemento)
        {
            if (posicion < 0 || posicion > Longitud())
            {
                Console.WriteLine("Posicion inexistente");
                return;
            }
            if (posicion == 1)
            {
                SubLista = new CLista(Elemento, SubLista);
                Elemento = pElemento;
            }
            else
            {
                SubLista.Insertar(posicion - 1, pElemento);
            }
        }

        public void Ordenar()
        {
            if (SubLista.EstaVacia())
                return;

            CLista nodoMenor = this;
            CLista actual = SubLista;

            while (!actual.EstaVacia())
            {
                IComparable menor = (IComparable)nodoMenor.Elemento;
                IComparable candidato = (IComparable)actual.Elemento;


                if (menor.CompareTo(candidato) > 0)
                {
                    nodoMenor = actual;
                }
                actual = actual.SubLista;
            }

            object temp = this.Elemento;
            this.Elemento = nodoMenor.Elemento;
            nodoMenor.Elemento = temp;

            SubLista.Ordenar();
        }



        public bool Ordenado()
        {
            if (EstaVacia() || SubLista.EstaVacia())
            {
                return true;
            }
            IComparable actual = (IComparable)Elemento;
            IComparable siguiente = (IComparable)SubLista.Elemento;
            if (actual.CompareTo(siguiente) > 0)
            {
                return false;
            }
            return SubLista.Ordenado();
        }

        public void EliminarOcurrencias(Object pElemento)
        {
            if (EstaVacia())
                return;

            if (Elemento.Equals(pElemento))
            {

                Elemento = SubLista.Elemento;
                SubLista = SubLista.SubLista;

                EliminarOcurrencias(pElemento);
            }
            else
            {
                SubLista.EliminarOcurrencias(pElemento);
            }
        }

        public void EliminarTodasOcurrencias()
        {
            if (EstaVacia())
            {
                return;
            }

            object Ele = Elemento;
            CLista actual = SubLista;
            CLista anterior = this;

            while (!actual.EstaVacia())
            {
                int resultado = ((IComparable)Ele).CompareTo(actual.Elemento);
                if (resultado == 0)
                {
                    anterior.SubLista = actual.SubLista;
                    actual = anterior.SubLista;
                }
                else
                {
                    anterior = actual;
                    actual = actual.SubLista;
                }
            }

            if (!SubLista.EstaVacia())
            {
                SubLista.EliminarTodasOcurrencias();
            }
        }
    }
}
