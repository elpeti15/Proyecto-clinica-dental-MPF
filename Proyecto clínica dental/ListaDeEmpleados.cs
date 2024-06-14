using System;
using System.Collections.Generic;

class ListaDeEmpleados
{
    private List<Empleado> listaDeEmpleados;

    public ListaDeEmpleados()
    {
        listaDeEmpleados = new List<Empleado>();
        Cargar();
    }

    public void Anyadir(Empleado empleado)
    {
        listaDeEmpleados.Add(empleado);
        listaDeEmpleados.Sort();
        Guardar();
    }

    public int CantidadDeEmpleados()
    {
        return listaDeEmpleados.Count;
    }

    public Empleado Obtener(int posicion)
    {
        return listaDeEmpleados[posicion];
    }

    public List<Empleado> ListaEmpleados()
    {
        List<Empleado> empleados = new List<Empleado>();
        for (int i = 0; i < listaDeEmpleados.Count; i++)
        {
            empleados.Add(listaDeEmpleados[i]);
        }
        return empleados;
    }

    private void Guardar()
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

    public void Cargar()
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