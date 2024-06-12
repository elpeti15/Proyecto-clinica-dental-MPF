using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

class ListaDePacientes
{
    private List<Paciente> listaDePacientes;

    public ListaDePacientes()
    {
        listaDePacientes = new List<Paciente>();
        Cargar();
    }

    public void Anyadir(Paciente paciente)
    {
        listaDePacientes.Add(paciente);
        listaDePacientes.Sort();
        Guardar();
    }

    public void Eliminar(int posicion)
    {
        listaDePacientes.RemoveAt(posicion);
        Guardar();
    }

    public void Modificar(int posicion, string nombre, string telefono, 
        string direccion, bool alergia)
    {
        listaDePacientes[posicion].NombreCompleto = nombre;
        listaDePacientes[posicion].Telefono = telefono;
        listaDePacientes[posicion].Direccion = direccion;
        listaDePacientes[posicion].Alergia = alergia;
        Guardar();
    }

    public Paciente Obtener(int posicion)
    {
        return listaDePacientes[posicion];
    }

    public string[] ObtenerPacientes(string texto)
    {
        List<string> listaObtenidos = new List<string>();
        for (int i = 0; i < listaDePacientes.Count;  i++)
        {
            if (listaDePacientes[i].Contiene(texto))
            {
                listaObtenidos.Add((i + 1) + ". " + listaDePacientes[i].ToString());
            }
        }
        return listaObtenidos.ToArray();
    }

    public int CantidadDePacientes()
    {
        return listaDePacientes.Count;
    }

    private void Guardar()
    {
        try
        {
            StreamWriter f = File.CreateText("Pacientes.txt");
            foreach (Paciente paciente in listaDePacientes)
            {
                f.Write(paciente.NombreCompleto + "#");
                f.Write(paciente.Telefono + "#");
                f.Write(paciente.Direccion + "#");
                f.WriteLine(paciente.Alergia);
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

    private void Cargar()
    {
        if (File.Exists("Pacientes.txt"))
        {
            try
            {
                string linea;
                StreamReader f = File.OpenText("Pacientes.txt");
                do
                {
                    linea = f.ReadLine();
                    if (linea != null)
                    {
                        string[] trozos = linea.Split('#');

                        Paciente paciente = new Paciente(trozos[0], trozos[1],
                            trozos[2], Convert.ToBoolean(trozos[3]));
                        listaDePacientes.Add(paciente);
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

