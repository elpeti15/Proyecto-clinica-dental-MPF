using System;

class GestorDePacientes
{
    public static List<Paciente> listaDePacientes = new List<Paciente>();
    public static void Lanzar()
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
    }

    private static ConsoleKey PantallaPacientes(int indice)
    {
        if (listaDePacientes.Count == 0)
        {
            Pantalla.Ventana(0, 0, 80, 25, "azul");
            Pantalla.Escribir(2, 2, "No hay pacientes registrados", "blanco");
            Pantalla.Escribir(2, 22, "1- Añadir", "blanco");
            Pantalla.Escribir(20, 22, "S- Volver", "blanco");
            ConsoleKeyInfo tecla1 = Console.ReadKey(true);
            return tecla1.Key;
        }
        string alergia2;
        DateTime ahora = DateTime.Now;
        string fecha = ahora.ToString("d");
        string hora = ahora.ToString("t");

        Pantalla.Ventana(0, 0, 80, 25, "azul");
        Pantalla.Escribir(2, 2, "Pacientes (actual: " 
            + (indice + 1) + "/" + listaDePacientes.Count + ")", "blanco");
        Pantalla.Escribir(30, 2, fecha, "gris");
        Pantalla.Escribir(50, 2, hora, "gris");

        Pantalla.Escribir(2, 6, "Nombre completo: ", "blanco");
        Pantalla.Escribir(19, 6, listaDePacientes[indice].NombreCompleto, "gris");
        Pantalla.Escribir(2, 8, "Teléfono: ", "blanco");
        Pantalla.Escribir(19, 8, listaDePacientes[indice].Telefono, "gris");
        Pantalla.Escribir(2, 10, "Dirección: ", "blanco");
        Pantalla.Escribir(19, 10, listaDePacientes[indice].Direccion, "gris");
        Pantalla.Escribir(2, 12, "Alergia: ", "blanco");
        if (listaDePacientes[indice].Alergia == true)
        {
            alergia2 = "Con alergia";
        }
        else
        {
            alergia2 = "Sin alergia";
        }
        Pantalla.Escribir(19, 12, alergia2, "gris");

        Pantalla.Escribir(1, 20, new string('─', 78), "blanco");

        Pantalla.Escribir(2, 22, "<- Anterior", "blanco");
        Pantalla.Escribir(20, 22, "-> Siguiente", "blanco");
        Pantalla.Escribir(40, 22, "1- Añadir", "blanco");
        Pantalla.Escribir(58, 22, "2- Número", "blanco");
        Pantalla.Escribir(2, 23, "3- Buscar", "blanco");
        Pantalla.Escribir(20, 23, "4- Modificar", "blanco");
        Pantalla.Escribir(40, 23, "5- Eliminar", "blanco");
        Pantalla.Escribir(58, 23, "S- Volver", "blanco");

        ConsoleKeyInfo tecla = Console.ReadKey(true);
        return tecla.Key;
    }

    private static void Anyadir(int indice)
    {
        DateTime ahora = DateTime.Now;
        string fecha = ahora.ToString("d");
        string hora = ahora.ToString("t");
        Pantalla.Ventana(0, 0, 80, 25, "azul");
        Pantalla.Escribir(2, 2, "Pacientes (actual: "
            + (indice + 1) + "/" + listaDePacientes.Count + ")", "blanco");
        Pantalla.Escribir(30, 2, fecha, "gris");
        Pantalla.Escribir(50, 2, hora, "gris");

        string nombre = Pantalla.PedirNormal(2, 6, "Nombre completo: ");
        while (nombre == "")
        {
            nombre = Pantalla.PedirNormal(2, 6, "Nombre completo: ");
        }
        string telefono = Pantalla.PedirNormal(2, 8, "Teléfono: ");
        while (telefono == "")
        {
            telefono = Pantalla.PedirNormal(2, 8, "Teléfono: ");
        }
        string direccion = Pantalla.PedirNormal(2, 10, "Dirección: ");
        while (direccion == "")
        {
            direccion = Pantalla.PedirNormal(2, 10, "Dirección: ");
        }
        string alergia = Pantalla.PedirNormal(2, 12, "Alergia (s/n): ").ToLower();
        while (alergia != "s" && alergia != "n")
        {
            alergia = Pantalla.PedirNormal(2, 12, "Alergia (s/n): ").ToLower();
        }

        bool alergia2 = false;
        if (alergia == "s")
            alergia2 = true;

        if (alergia == "n")
            alergia2 = false;

        Pantalla.Escribir(1, 20, new string('─', 78), "blanco");

        Pantalla.Escribir(2, 22, "1- Confirmar", "blanco");
        Pantalla.Escribir(25, 22, "S- Volver", "blanco");

        string confirmacion = Console.ReadLine();
        if (confirmacion == "1")
        {
            listaDePacientes.Add(new Paciente(nombre, telefono, direccion, alergia2));
            Pantalla.Escribir(2, 16, "Paciente añadido correctamente", "verde");
            Console.ReadKey();
        }
    }

    private static int Numero()
    {
        int indice = Convert.ToInt32(Pantalla.PedirNormal
            (2, 16, "Introduzca el número de registro ")) - 1;

        return indice;
    }

    private static void Buscar()
    {
        DateTime ahora = DateTime.Now;
        string fecha = ahora.ToString("d");
        string hora = ahora.ToString("t");
        Pantalla.Ventana(0, 0, 80, 25, "azul");
        Pantalla.Escribir(2, 2, "Búsqueda de pacientes: ", "blanco");
        Pantalla.Escribir(30, 2, fecha, "gris");
        Pantalla.Escribir(50, 2, hora, "gris");

        string textoBuscar = Pantalla.PedirNormal(2, 6, "Texto a buscar: ");
        string[] contienen = EncontrarPacientesQueContienen(textoBuscar);
        if (contienen.Length > 0)
        {
            for (int i = 0; i < contienen.Length; i++)
            {
                Pantalla.Escribir(5, (8+i), contienen[i], "gris");
            }
            Console.ReadKey();
        }
        else
        {
            Pantalla.Escribir(2, 8, "No se han encontrado coincidencias.", "rojo");
        }
    }

    private static void Modificar(int indice)
    {
        string alergia, alergia2;
        bool alergia3;
        DateTime ahora = DateTime.Now;
        string fecha = ahora.ToString("d");
        string hora = ahora.ToString("t");

        Pantalla.Ventana(0, 0, 80, 25, "azul");
        Pantalla.Escribir(2, 2, "Pacientes (actual: "
            + (indice + 1) + "/" + listaDePacientes.Count + ")", "blanco");
        Pantalla.Escribir(30, 2, fecha, "gris");
        Pantalla.Escribir(50, 2, hora, "gris");

        Pantalla.Escribir(2, 6, "Nombre completo: ", "blanco");
        string nombre = Pantalla.Pedir(19, 6, 30, listaDePacientes[indice].NombreCompleto);
        Pantalla.Escribir(2, 8, "Teléfono: ", "blanco");
        string telefono = Pantalla.Pedir(19, 8, 9, listaDePacientes[indice].Telefono);
        Pantalla.Escribir(2, 10, "Dirección: ", "blanco");
        string direccion = Pantalla.Pedir(19, 10, 30, listaDePacientes[indice].Direccion);
        Pantalla.Escribir(2, 12, "Alergia (s/n): ", "blanco");

        if (listaDePacientes[indice].Alergia == true)
            alergia = "Con alergia";
        else
            alergia = "Sin alergia";
 
        alergia2 = Pantalla.Pedir(19, 12, 3, alergia).ToLower();
        if (alergia2 == "s")
            alergia3 = true;
        else
            alergia3 = false;

        Pantalla.Escribir(1, 20, new string('─', 78), "blanco");

        Pantalla.Escribir(2, 22, "1- Confirmar", "blanco");
        Pantalla.Escribir(25, 22, "S- Volver", "blanco");

        string confirmacion = Console.ReadLine();
        if (confirmacion == "1")
        {
            listaDePacientes[indice].NombreCompleto = nombre;
            listaDePacientes[indice].Telefono = telefono;
            listaDePacientes[indice].Direccion = direccion;
            listaDePacientes[indice].Alergia = alergia3;
            Pantalla.Escribir(2, 16, "Paciente modificado correctamente", "verde");
            Console.ReadKey();
        }
    }

    private static void Eliminar(int indice)
    {
        string respuesta = Pantalla.PedirNormal
            (2, 14, "¿Está seguro de borrar el paciente? (s/n)").ToLower();
        if (respuesta == "s")
        {
            listaDePacientes.RemoveAt(indice);
            Pantalla.Escribir(2, 16, "Paciente eliminado con éxito", "verde");
            Console.ReadKey();
        }      
    }

    private static string[] EncontrarPacientesQueContienen(string texto)
    {
        List<string> listaEncontrados = new List<string>();
        for (int i = 0; i < listaDePacientes.Count; i++)
        {
            if (listaDePacientes[i].Contiene(texto))
            {
                listaEncontrados.Add((i + 1) + "- " + listaDePacientes[i].ToString());
            }
        }
        return listaEncontrados.ToArray();
    }

    private static void GuardarPacientes()
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

    public static void CargarPacientes()
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