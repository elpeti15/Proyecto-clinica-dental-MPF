using System;
using System.Collections.Generic;

class GestorDeEmpleados
{
    public static ListaDeEmpleados lista = new ListaDeEmpleados();

    /*public static void Lanzar()
    {
        int indice = 0;
        bool salir = false;
        do
        {
            ConsoleKey opcion = PantallaPacientes(indice); //devuelve la tecla que ha pulsado

            switch (opcion)
            {
                case ConsoleKey.LeftArrow:
                    indice = (indice - 1 + listaDePacientes.Count) % listaDePacientes.Count;
                    break;

                case ConsoleKey.RightArrow:
                    indice = (indice + 1) % listaDePacientes.Count;
                    break;

                case ConsoleKey.D1:
                    Anyadir(listaDePacientes.Count);
                    listaDePacientes.Sort();
                    GuardarPacientes();
                    break;

                case ConsoleKey.D2:
                    indice = Numero();
                    PantallaPacientes(indice);
                    break;

                case ConsoleKey.D3:
                    Buscar();
                    break;

                case ConsoleKey.D4:
                    Modificar(indice);
                    GuardarPacientes();
                    break;

                case ConsoleKey.D5:
                    Eliminar(indice);
                    GuardarPacientes();
                    break;
                case ConsoleKey.S: salir = true; break;
                default:
                    Pantalla.Escribir(25, 15, "Opción no válida", "rojo");
                    break;
            }
        } while (!salir);
    }*/
}