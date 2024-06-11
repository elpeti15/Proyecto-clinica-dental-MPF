using System;
using System.Collections.Generic;

class GestorDeEmpleados
{
    public static List<Empleado> listaDeEmpleados = new List<Empleado>();

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

    private static void GuardarEmpleados()
    {
        try
        {
            StreamWriter f = File.CreateText("Empleados.txt");
            foreach (Empleado empleado in listaDeEmpleados)
            {
                if (empleado is Auxiliar auxiliar)
                {
                    f.Write(auxiliar.Login + "#");
                    f.Write(auxiliar.Password);
                }
                else if (empleado is Especialista especialista)
                {
                    f.Write(especialista.Login + "#");
                    f.Write(especialista.Password + "#");
                    f.Write(especialista.Nombre + "#");
                    f.Write(especialista.Especialidad);
                }
                else if (empleado is Doctor doctor)
                {
                    f.Write(doctor.Login + "#");
                    f.Write(doctor.Password + "#");
                    f.Write(doctor.Nombre);
                }
            }
            f.Close();
        }
        catch (PathTooLongException)
        {
            Console.WriteLine("La ruta del fichero es demasiado larga.");
        }
        catch (IOException e)
        {
            Console.WriteLine("Error de escritura: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error general: " + e.Message);
        }
    }

    public static void CargarEmpleados()
    {
        if (File.Exists("Empleados.txt"))
        {
            try
            {
                string linea;
                StreamReader f = File.OpenText("Empleados.txt");
                do
                {
                    linea = f.ReadLine();
                    if (linea != null)
                    {
                        string[] trozos = linea.Split('#');

                        if (trozos.Length == 2)
                        {
                            Empleado auxiliar = new Auxiliar(trozos[0], trozos[1]);
                            listaDeEmpleados.Add(auxiliar);
                        }
                        else if (trozos.Length == 3)
                        {
                            Empleado doctor = new Doctor(trozos[0], trozos[1], trozos[2]);
                            listaDeEmpleados.Add(doctor);
                        }
                        else if (trozos.Length == 4)
                        {
                            Empleado especialista = new Especialista(trozos[0], trozos[1],
                                trozos[2], trozos[3]);
                            listaDeEmpleados.Add(especialista);
                        }
                    }
                } while (linea != null);
                f.Close();
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("La ruta del fichero es demasiado larga.");
            }
            catch (IOException e)
            {
                Console.WriteLine("Error de lectura: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error general: " + e.Message);
            }
        }
    }
}