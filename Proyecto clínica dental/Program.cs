using Proyecto_clínica_dental;
using System;
using System.Collections.Generic;
class MenuPrincipal
{
    static void Main(string[] args)
    {
        GestorDePacientes.CargarPacientes();
        GestorDeEmpleados.CargarEmpleados();
        List<Empleado> empleados = GestorDeEmpleados.listaDeEmpleados;
        //Pantalla de login
        string login;
        string password;
        Empleado empleadoVerificado;
        do
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            Pantalla.Ventana(0, 0, 80, 25, "azul");
            Pantalla.Escribir(34, 2, "INICIE SESIÓN", "blanco");

            login = Pantalla.PedirNormal(25, 6, "Usuario: ");
            password = Pantalla.PedirNormal(25, 8, "Contraseña: ");
            empleadoVerificado = VerificarEmpleado(empleados,
                login, password);

            if (empleadoVerificado == null)
            {
                Pantalla.Escribir(25, 10, "Usuario o contraseña incorrectos", "rojo");
                Console.ReadKey();
            }
        } while (empleadoVerificado == null);

        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();

        //Menú general auxiliar
        if (empleadoVerificado is Auxiliar)
        {
            string opcion;
            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                MenuAuxiliar1();
                opcion = Pantalla.PedirNormal(25, 14, "Escoge una opción: ").ToString();

                switch (opcion)
                {
                    case "1":
                        GestorDePacientes.Lanzar();
                        break;
                    case "2": 
                        GestorDeCitas.Lanzar();
                        break;
                    case "3": 
                        Console.WriteLine();
                        break;
                    case "S":
                        Console.WriteLine();
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }
            } while (opcion != "S");
        }
    }

    static Empleado VerificarEmpleado (List<Empleado> empleados, string login, string password)
    {
        foreach (Empleado empleado in empleados)
        {
            if (empleado.Login == login && empleado.Password == password)
                return empleado;
        }
        return null;
    }

    static void MenuAuxiliar1()
    {
        Pantalla.Ventana(0, 0, 80, 25, "azul");
        Pantalla.Escribir(25, 2, "Menú de gestión para auxiliar", "blanco");
        Pantalla.Escribir(25, 6, "1. Gestion de pacientes", "blanco");
        Pantalla.Escribir(25, 8, "2. Gestion de citas", "blanco");
        Pantalla.Escribir(25, 10, "3. Gestion de presupuestos", "blanco");
        Pantalla.Escribir(25, 12, "S. Volver", "blanco");
    }
}
