using System;
using System.Numerics;

class GestorDeCitas
{
    private static List<Cita> listaDeCitas = new List<Cita>();

    public static void Lanzar()
    {
        int indice = 0;
        bool salir = false;
        do
        {
            ConsoleKey opcion = PantallaCitas(indice); //devuelve la tecla que ha pulsado

            switch (opcion)
            {
                case ConsoleKey.LeftArrow:
                    indice = (indice - 1 + listaDeCitas.Count) % listaDeCitas.Count;
                    break;

                case ConsoleKey.RightArrow:
                    indice = (indice + 1) % listaDeCitas.Count;
                    break;

                case ConsoleKey.D1:
                    Anyadir(listaDeCitas.Count);
                    listaDeCitas.Sort();
                    GuardarCitas();
                    break;

                case ConsoleKey.D2:
                    indice = Numero();
                    PantallaCitas(indice);
                    break;

                case ConsoleKey.D3:
                    VerHorariosOcupados(indice);
                    break;

                case ConsoleKey.D4:
                    //Buscar();
                    break;

                case ConsoleKey.D5:
                    //Modificar(indice);
                    //GuardarPacientes();
                    break;

                case ConsoleKey.D6:
                    //Eliminar(indice);
                    //GuardarPacientes();
                    break;
                case ConsoleKey.S: salir = true; break;
                default:
                    Pantalla.Escribir(25, 15, "Opción no válida", "rojo");
                    break;
            }
        } while (!salir);
    }

    private static ConsoleKey PantallaCitas(int indice)
    {
        if (listaDeCitas.Count == 0)
        {
            Pantalla.Ventana(0, 0, 80, 25, "azul");
            Pantalla.Escribir(1, 20, new string('─', 78), "blanco");
            Pantalla.Escribir(2, 2, "No hay citas registradas", "blanco");
            Pantalla.Escribir(2, 22, "1- Añadir", "blanco");
            Pantalla.Escribir(20, 22, "S- Volver", "blanco");
            ConsoleKeyInfo tecla1 = Console.ReadKey(true);
            return tecla1.Key;
        }
        RellenoMenu(indice);
        Pantalla.Escribir(2, 6, "ID cita: ", "blanco");
        Pantalla.Escribir(20, 6, listaDeCitas[indice].IdCita, "gris");
        Pantalla.Escribir(2, 8, "Paciente: ", "blanco");
        Pantalla.Escribir(20, 8, listaDeCitas[indice].Paciente.NombreCompleto, "gris");
        Pantalla.Escribir(2, 10, "Doctor: ", "blanco");
        Pantalla.Escribir(20, 10, listaDeCitas[indice].Doctor.Nombre, "gris");
        Pantalla.Escribir(2, 12, "Fecha: ", "blanco");
        Pantalla.Escribir(20, 12, listaDeCitas[indice].Fecha.ToString("g"), "gris");

        if (listaDeCitas[indice] is CitaPrimera)
        {
            Pantalla1aCita(indice);
        }
        else if (listaDeCitas[indice] is CitaNormal)
        {
            PantallaCitaNormal(indice);
        } 
        ConsoleKeyInfo tecla = Console.ReadKey(true);
        return tecla.Key;
    }

    private static void Anyadir(int indice)
    {
        string idCita = "", nombreP = "", nombreD = "", valoracion = "",
            dProblema = "", tratamiento = ""; ;
        DateTime fecha = new DateTime();
        Doctor doctor = null;
        Paciente paciente = null;

        RellenoCabecera(indice);
        string respuesta = Pantalla.PedirNormal(2, 6, "Es la primera cita del paciente? (s/n) ").ToLower();
        while (respuesta != "s" && respuesta != "n")
        {
            respuesta = Pantalla.PedirNormal(2, 6, "Es la primera cita del paciente? (s/n)").ToLower();
        }
        if (respuesta == "s")
        {
            PedirDatosGeneral(ref idCita, ref nombreP, ref nombreD, ref fecha);
            PedirDatos1aCita(ref valoracion);
            ConvertirPacientesYDoctor(ref nombreP, ref nombreD, ref doctor, ref paciente);

            Pantalla.Escribir(2, 22, "1- Confirmar", "blanco");
            Pantalla.Escribir(25, 22, "S- Volver", "blanco");

            string confirmacion = Console.ReadLine();
            if (confirmacion == "1")
            {
                listaDeCitas.Add(new CitaPrimera(idCita, paciente, doctor,
                    fecha, valoracion));
                Pantalla.Escribir(2, 16, "Primera cita añadida correctamente", "verde");
                Console.ReadKey();
            }
        }
        else if (respuesta == "n")
        {
            PedirDatosGeneral(ref idCita, ref nombreP, ref nombreD, ref fecha);
            PedirDatosCitaNormal(ref dProblema, ref tratamiento);
            ConvertirPacientesYDoctor(ref nombreP, ref nombreD, ref doctor, ref paciente);

            Pantalla.Escribir(2, 22, "1- Confirmar", "blanco");
            Pantalla.Escribir(25, 22, "S- Volver", "blanco");
            string confirmacion = Console.ReadLine();
            if (confirmacion == "1")
            {
                listaDeCitas.Add(new CitaNormal(idCita, paciente, doctor,
                    fecha, dProblema, tratamiento));
                Pantalla.Escribir(2, 16, "Cita añadida correctamente", "verde");
                Console.ReadKey();
            }
        }
    }

    private static int Numero()
    {
        int indice = Convert.ToInt32(Pantalla.PedirNormal
            (2, 18, "Introduzca el número de registro ")) - 1;

        return indice;
    }

    private static void VerHorariosOcupados(int indice)
    {
        DateTime ahora = DateTime.Now;
        string fecha = ahora.ToString("d");
        string hora = ahora.ToString("t");
        Pantalla.Ventana(0, 0, 80, 25, "azul");
        Pantalla.Escribir(2, 2, "Horarios disponibles", "blanco");
        Pantalla.Escribir(30, 2, fecha, "gris");
        Pantalla.Escribir(50, 2, hora, "gris");

        for(int i = 0; i < listaDeCitas.Count; i++)
        {
            Pantalla.Escribir(2, 6, listaDeCitas[i].Fecha.ToString("g"), "amarillo");
        }
        Pantalla.Escribir(1, 20, new string('─', 78), "blanco");


    }

    private static void PedirDatos1aCita(ref string valoracionB)
    {
        Pantalla.Escribir(2, 14, new string(' ', 60), "azul");
        valoracionB = Pantalla.PedirNormal(2, 14, "Valoración: ");
        while (valoracionB == "")
        {
            valoracionB = Pantalla.PedirNormal(2, 14, "Valoración: ");
        }
    }

    private static void PedirDatosCitaNormal(ref string dProblema, ref string tratamiento)
    {
        Pantalla.Escribir(2, 14, new string(' ', 60), "azul");
        dProblema = Pantalla.PedirNormal(2, 14, "Problemática: ");
        while (dProblema == "")
        {
            dProblema = Pantalla.PedirNormal(2, 14, "Problemática: ");
        }
        tratamiento = Pantalla.PedirNormal(2, 16, "Tratamiento: ");
        while (tratamiento == "")
        {
            tratamiento = Pantalla.PedirNormal(2, 16, "Tratamiento: ");
        }
    }

    private static void PedirDatosGeneral(ref string idCita, ref string nombreP,
        ref string nombreD, ref DateTime fecha)
    {
        //idCita = 3 primeras letras del paciente (mayúsculas) + fecha(día/mes/año)
        nombreP = Pantalla.PedirNormal(2, 8, "Nombre del paciente: ");
        while (nombreP == "")
        {
            nombreP = Pantalla.PedirNormal(2, 8, "Nombre del paciente: ");
        }
        nombreD = Pantalla.PedirNormal(2, 10, "Nombre del doctor: ");
        while (nombreD == "")
        {
            nombreD = Pantalla.PedirNormal(2, 10, "Nombre del doctor: ");
        }
        int dia = Convert.ToInt32(Pantalla.PedirNormal(2, 12, "Día (dd): "));
        int mes = Convert.ToInt32(Pantalla.PedirNormal(16, 12, "Mes (mm): "));
        int anyo = Convert.ToInt32(Pantalla.PedirNormal(30, 12, "Año (aaaa): "));
        int hora = Convert.ToInt32(Pantalla.PedirNormal(47, 12, "Hora (HH): "));
        int minutos = Convert.ToInt32(Pantalla.PedirNormal(62, 12, "Minutos (mm): "));

        while ((dia <= 0 || dia > 31) || (mes <= 0 || mes > 12) || (anyo < 2024 || anyo <= 0)
            || (hora < 8 || hora >= 21) || (minutos < 0 || minutos > 59))
        {
            Pantalla.Escribir(2, 14, "Fecha u hora incorrectas. Inténtelo de nuevo.", "rojo");
            dia = Convert.ToInt32(Pantalla.PedirNormal(2, 12, "Día (dd): "));
            mes = Convert.ToInt32(Pantalla.PedirNormal(16, 12, "Mes (mm): "));
            anyo = Convert.ToInt32(Pantalla.PedirNormal(30, 12, "Año (aaaa): "));
            hora = Convert.ToInt32(Pantalla.PedirNormal(47, 12, "Hora (HH): "));
            minutos = Convert.ToInt32(Pantalla.PedirNormal(62, 12, "Minutos (mm): "));
        }
        fecha = new DateTime(anyo, mes, dia, hora, minutos, 0);
        idCita = nombreP.Substring(0, 3).ToUpper() + fecha.ToString("d");
    }

    private static void Pantalla1aCita(int indice)
    {
        Pantalla.Escribir(2, 14, "Valoración bucal: ", "blanco");
        if (listaDeCitas[indice] is CitaPrimera citaPrimera)
            Pantalla.Escribir(20, 14, citaPrimera.ValoracionBucal, "gris");
    }

    private static void PantallaCitaNormal(int indice)
    {
        Pantalla.Escribir(2, 14, "Problemática: ", "blanco");
        if (listaDeCitas[indice] is CitaNormal citaNormal)
            Pantalla.Escribir(19, 14, citaNormal.DescripcionProblema, "gris");

        Pantalla.Escribir(2, 16, "Tratamiento: ", "blanco");
        if (listaDeCitas[indice] is CitaNormal citaNormal2)
            Pantalla.Escribir(19, 14, citaNormal2.Tratamiento, "gris");
    }

    private static void RellenoMenu(int indice)
    {
        DateTime ahora = DateTime.Now;
        string fecha = ahora.ToString("d");
        string hora = ahora.ToString("t");

        Pantalla.Ventana(0, 0, 80, 25, "azul");
        Pantalla.Escribir(2, 2, "Citas (actual: "
            + (indice + 1) + "/" + listaDeCitas.Count + ")", "blanco");
        Pantalla.Escribir(30, 2, fecha, "gris");
        Pantalla.Escribir(50, 2, hora, "gris");

        Pantalla.Escribir(1, 20, new string('─', 78), "blanco");

        Pantalla.Escribir(2, 22, "<- Anterior", "blanco");
        Pantalla.Escribir(20, 22, "-> Siguiente", "blanco");
        Pantalla.Escribir(40, 22, "1- Añadir", "blanco");
        Pantalla.Escribir(58, 22, "2- Número", "blanco");
        Pantalla.Escribir(2, 23, "3- Buscar", "blanco");
        Pantalla.Escribir(20, 23, "4- Modificar", "blanco");
        Pantalla.Escribir(40, 23, "5- Eliminar", "blanco");
        Pantalla.Escribir(58, 23, "S- Volver", "blanco");
    }

    private static void RellenoCabecera(int indice)
    {
        DateTime ahora = DateTime.Now;
        string fecha = ahora.ToString("d");
        string hora = ahora.ToString("t");

        Pantalla.Ventana(0, 0, 80, 25, "azul");
        Pantalla.Escribir(2, 2, "Citas (actual: "
            + (indice + 1) + "/" + listaDeCitas.Count + ")", "blanco");
        Pantalla.Escribir(30, 2, fecha, "gris");
        Pantalla.Escribir(50, 2, hora, "gris");

        Pantalla.Escribir(1, 20, new string('─', 78), "blanco");
    }

    private static void ConvertirPacientesYDoctor(ref string nombreP, ref string nombreD,
        ref Doctor doctor, ref Paciente paciente)
    {
        foreach (Empleado e in GestorDeEmpleados.listaDeEmpleados)
        {
            if (e is Doctor d && d.Nombre.ToLower().Contains(nombreD.ToLower()))
                doctor = d;
        }
        foreach (Paciente p in GestorDePacientes.listaDePacientes)
        {
            if (p.NombreCompleto.ToLower().Contains(nombreP.ToLower()))
                paciente = p;
        }
    }

    private static void GuardarCitas()
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
                    f.Write(citaPrimera.ValoracionBucal);
                }
                else if (cita is CitaNormal citaNormal)
                {
                    f.Write(citaNormal.IdCita + "#");
                    f.Write(citaNormal.Paciente.NombreCompleto + "#");
                    f.Write(citaNormal.Doctor.Nombre + "#");
                    f.Write(citaNormal.Fecha + "#");
                    f.Write(citaNormal.DescripcionProblema + "#");
                    f.Write(citaNormal.Tratamiento);
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

    public static void CargarCitas()
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

                        if (trozos.Length == 5)
                        {
                            Cita cita1a = new CitaPrimera(trozos[0], paciente,
                                doctor, Convert.ToDateTime(trozos[3]), trozos[4]);
                            listaDeCitas.Add(cita1a);
                        }
                        else if (trozos.Length == 6)
                        {
                            Cita citaNormal = new CitaNormal(trozos[0], paciente,
                                doctor, Convert.ToDateTime(trozos[3]), trozos[4],
                                trozos[5]);
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