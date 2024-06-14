using System;
using System.Collections.Generic;
using System.Numerics;

class ListaDeCitas
{
    private List<Cita> listaDeCitas;
    private List<Cita> citasConfirmadas = new List<Cita>();

    public ListaDeCitas()
    {
        listaDeCitas = new List<Cita>();
        Cargar();
    }
    public void Anyadir(Cita cita)
    {
        listaDeCitas.Add(cita);
        listaDeCitas.Sort();
        citasConfirmadas.Add(cita);
        citasConfirmadas.Sort();
        Guardar();
    }

    public void Cancelar(Cita cita)
    {
        cita.Cancelada = true;
        citasConfirmadas.Remove(cita);
        Guardar();
    }

    public void Modificar1aCita(int posicion, string paciente, string doctor,
        string fecha, string valoracion)
    {
        if (listaDeCitas[posicion] is CitaPrimera citaPrimera)
        {
            citaPrimera.Paciente.NombreCompleto = paciente;
            citaPrimera.Doctor.Nombre = doctor;
            citaPrimera.Fecha = Convert.ToDateTime(fecha);
            citaPrimera.ValoracionBucal = valoracion;
        }
        Guardar();
    }

    public void ModificarCitaNormal(int posicion, string paciente, string doctor,
        string fecha, string problematica, string tratamiento)
    {
        if (listaDeCitas[posicion] is CitaNormal citaNormal)
        {
            citaNormal.Paciente.NombreCompleto = paciente;
            citaNormal.Doctor.Nombre = doctor;
            citaNormal.Fecha = DateTime.Parse(fecha);
            citaNormal.DescripcionProblema = problematica;
            citaNormal.Tratamiento = tratamiento;
        }
        Guardar();
    }

    public Cita Obtener(int posicion)
    {
        return listaDeCitas[posicion];
    }

    public string[] ObtenerCitasConfirmadas(string texto)
    {
        List<string> listaConfirmadas = new List<string>();
        for (int i = 0; i < listaDeCitas.Count; i++)
        {
            if (listaDeCitas[i].Contiene(texto) && listaDeCitas[i].Cancelada == false)
            {
                listaConfirmadas.Add((i + 1) + ". " + listaDeCitas[i].ToString());
            }
        }
        return listaConfirmadas.ToArray();
    }

    public List<Cita> ListaCitasConfirmadas()
    {
        for (int i = 0; i < listaDeCitas.Count; i++)
        {
            if (listaDeCitas[i].Cancelada == false)
                citasConfirmadas.Add(listaDeCitas[i]);
        }
        return citasConfirmadas;
    }

    public string[] ObtenerCitasTotales(string texto)
    {
        List<string> listaTotal = new List<string>();
        for (int i = 0; i < listaDeCitas.Count; i++)
        {
            if (listaDeCitas[i].Contiene(texto))
            {
                listaTotal.Add((i + 1) + ". " + listaDeCitas[i].ToString());
            }
        }
        return listaTotal.ToArray();
    }

    public int CantidadDeCitas()
    {
        return listaDeCitas.Count;
    }

    //falta crear la clase listaDeEmpleados
    public void ConvertirPacientesYDoctor(ref string nombreP, ref string nombreD,
        ref Doctor doctor, ref Paciente paciente)
    {
        for (int i = 0; i < GestorDeEmpleados.lista.CantidadDeEmpleados(); i++)
        {
            if (GestorDeEmpleados.lista.Obtener(i) is Doctor d)
            {
                if (d.Nombre.ToLower().Contains(nombreD.ToLower()))
                    doctor = d;
            }
        }
        for (int i = 0; i < GestorDePacientes.lista.CantidadDePacientes(); i++)
        {
            if (GestorDePacientes.lista.Obtener(i).NombreCompleto.ToLower().Contains(nombreP.ToLower()))
                paciente = GestorDePacientes.lista.Obtener(i);
        }
    }

    private void Guardar()
    {
        try
        {
            StreamWriter f = File.CreateText("Citas.txt");
            foreach (Cita cita in listaDeCitas)
            {
                if (cita is CitaPrimera citaPrimera)
                {
                    f.Write(citaPrimera.IdCita + "#");
                    f.Write(citaPrimera.Paciente.NombreCompleto + "#");
                    f.Write(citaPrimera.Doctor.Nombre + "#");
                    f.Write(citaPrimera.Fecha + "#");
                    f.Write(citaPrimera.Cancelada + "#");
                    f.WriteLine(citaPrimera.ValoracionBucal);
                }
                else if (cita is CitaNormal citaNormal)
                {
                    f.Write(citaNormal.IdCita + "#");
                    f.Write(citaNormal.Paciente.NombreCompleto + "#");
                    f.Write(citaNormal.Doctor.Nombre + "#");
                    f.Write(citaNormal.Fecha + "#");
                    f.Write(citaNormal.Cancelada + "#");
                    f.Write(citaNormal.DescripcionProblema + "#");
                    f.WriteLine(citaNormal.Tratamiento);
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
        if (File.Exists("Citas.txt"))
        {
            try
            {
                string linea;
                StreamReader f = File.OpenText("Citas.txt");
                Doctor doctor = null;
                Paciente paciente = null;
                do
                {
                    linea = f.ReadLine();
                    if (linea != null)
                    {
                        string[] trozos = linea.Split('#');
                        ConvertirPacientesYDoctor(ref trozos[1], ref trozos[2],
                            ref doctor, ref paciente);

                        if (trozos.Length == 6)
                        {
                            Cita cita1a = new CitaPrimera(trozos[0], paciente,
                                doctor, Convert.ToDateTime(trozos[3]),
                                Convert.ToBoolean(trozos[4]), trozos[5]);
                            listaDeCitas.Add(cita1a);
                        }
                        else if (trozos.Length == 7)
                        {
                            Cita citaNormal = new CitaNormal(trozos[0], paciente,
                                doctor, Convert.ToDateTime(trozos[3]),
                                Convert.ToBoolean(trozos[4]), trozos[5], trozos[6]);
                            listaDeCitas.Add(citaNormal);
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

