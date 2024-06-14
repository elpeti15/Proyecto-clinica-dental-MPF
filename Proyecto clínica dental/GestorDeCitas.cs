using System;
using System.Numerics;

class GestorDeCitas
{
    public static ListaDeCitas lista = new ListaDeCitas();
    //creamos la lista secundaria para manejar la pantalla y solo muestre las citas confirmadas
    public static void Lanzar()
    {
        List<Cita> citasConfirmadas = lista.ListaCitasConfirmadas(); //lista secundaria de las citas (no canceladas)
        int indice = 0, contador = 0;
        bool salir = false, ocultar = true;
        do
        {
            ConsoleKey opcion = PantallaCitas(indice, citasConfirmadas, ocultar);
            switch (opcion)
            {
                case ConsoleKey.LeftArrow:
                    if (ocultar == false)
                        indice = (indice - 1 + lista.CantidadDeCitas()) % lista.CantidadDeCitas();
                    else
                        indice = (indice - 1 + citasConfirmadas.Count) % citasConfirmadas.Count;
                    break;
                case ConsoleKey.RightArrow:
                    if (ocultar == false)
                        indice = (indice + 1) % lista.CantidadDeCitas();
                    else
                        indice = (indice + 1) % citasConfirmadas.Count;
                    break;
                case ConsoleKey.D1:
                    Anyadir(lista.CantidadDeCitas(), ocultar, citasConfirmadas); break;
                case ConsoleKey.D2:
                    if (ocultar == false)
                        indice = Numero(lista.CantidadDeCitas());
                    else
                        indice = Numero(citasConfirmadas.Count);
                    PantallaCitas(indice, citasConfirmadas, ocultar); break;
                case ConsoleKey.D3:
                    ConsoleKey opcion2;
                    do
                    {
                        opcion2 = VerHorariosOcupados(contador, citasConfirmadas);
                        if (opcion2 == ConsoleKey.LeftArrow && contador >= 13)
                            contador -= 13;

                        else if (opcion2 == ConsoleKey.RightArrow && contador + 13 < citasConfirmadas.Count)
                            contador += 13;

                        else if (opcion2 == ConsoleKey.LeftArrow && contador < 13)
                            contador = 0;

                        else if (opcion2 == ConsoleKey.RightArrow && contador + 13 >= citasConfirmadas.Count)
                            contador = citasConfirmadas.Count - (citasConfirmadas.Count % 13);

                    } while (opcion2 != ConsoleKey.S); break;
                case ConsoleKey.D4:
                    Buscar(ocultar); break;
                case ConsoleKey.D5:
                    Modificar(indice, ocultar, citasConfirmadas); break;
                case ConsoleKey.D6:
                    Cancelar(indice); break;
                case ConsoleKey.D7:
                    Configuracion(ref ocultar); break;
                case ConsoleKey.S: salir = true; break;
                default:
                    Pantalla.Escribir(25, 15, "Opción no válida", "rojo");
                    break;
            }
        } while (!salir);
    }

    private static ConsoleKey PantallaCitas(int indice, List<Cita> citasConfirmadas,
        bool ocultar)
    {
        if (lista.CantidadDeCitas() == 0 || citasConfirmadas.Count == 0)
        {
            Pantalla.Ventana(0, 0, 80, 25, "azul");
            Pantalla.Escribir(1, 20, new string('─', 78), "blanco");
            Pantalla.Escribir(2, 2, "No hay citas registradas", "blanco");
            Pantalla.Escribir(2, 22, "1- Añadir", "blanco");
            Pantalla.Escribir(20, 22, "S- Volver", "blanco");
            ConsoleKeyInfo tecla1 = Console.ReadKey(true);
            return tecla1.Key;
        }
        RellenoMenu(indice, ocultar, citasConfirmadas);
        if (ocultar == false)
            PantallaCitasTotal(indice);
        else
            PantallaCitasConfirmadas(indice, citasConfirmadas);
        ConsoleKeyInfo tecla = Console.ReadKey(true);
        return tecla.Key;
    }

    private static void PantallaCitasTotal(int indice)
    {
        if(lista.Obtener(indice).Cancelada == true)
        {
            Pantalla.Escribir(2, 6, "ID cita: ", "rojo");
            Pantalla.Escribir(2, 8, "Paciente: ", "rojo");
            Pantalla.Escribir(2, 10, "Doctor: ", "rojo");
            Pantalla.Escribir(2, 12, "Fecha: ", "rojo");
            Pantalla.Escribir(2, 14, "Estado: ", "rojo");
        }
        else
        {
            Pantalla.Escribir(2, 6, "ID cita: ", "blanco");
            Pantalla.Escribir(2, 8, "Paciente: ", "blanco");
            Pantalla.Escribir(2, 10, "Doctor: ", "blanco");
            Pantalla.Escribir(2, 12, "Fecha: ", "blanco");
            Pantalla.Escribir(2, 14, "Estado: ", "blanco");
        }
        Pantalla.Escribir(20, 6, lista.Obtener(indice).IdCita, "gris");   
        Pantalla.Escribir(20, 8, lista.Obtener(indice).Paciente.NombreCompleto, "gris");      
        Pantalla.Escribir(20, 10, lista.Obtener(indice).Doctor.Nombre, "gris");   
        Pantalla.Escribir(20, 12, lista.Obtener(indice).Fecha.ToString("g"), "gris");      
        Pantalla.Escribir(20, 14, lista.Obtener(indice).Cancelada ? "Cancelada" : "Confirmada", "gris");

        if (lista.Obtener(indice) is CitaPrimera)
        {
            if (lista.Obtener(indice).Cancelada == true)
                Pantalla.Escribir(2, 16, "Valoración bucal: ", "rojo");
            else
                Pantalla.Escribir(2, 16, "Valoración bucal: ", "blanco");
            if (lista.Obtener(indice) is CitaPrimera citaPrimera)
                Pantalla.Escribir(20, 16, citaPrimera.ValoracionBucal, "gris");
        }
        else if (lista.Obtener(indice) is CitaNormal)
        {
            if (lista.Obtener(indice).Cancelada == true)
                Pantalla.Escribir(2, 16, "Problemática: ", "rojo");
            else
                Pantalla.Escribir(2, 16, "Problemática: ", "blanco");
            if (lista.Obtener(indice) is CitaNormal citaNormal)
                Pantalla.Escribir(20, 16, citaNormal.DescripcionProblema, "gris");
            if (lista.Obtener(indice).Cancelada == true)
                Pantalla.Escribir(2, 18, "Tratamiento: ", "rojo");
            else
                Pantalla.Escribir(2, 18, "Tratamiento: ", "blanco");
            if (lista.Obtener(indice) is CitaNormal citaNormal2)
                Pantalla.Escribir(20, 18, citaNormal2.Tratamiento, "gris");
        }
    }

    private static void PantallaCitasConfirmadas(int indice, List<Cita> citasConfirmadas)
    {
        Pantalla.Escribir(2, 6, "ID cita: ", "blanco");
        Pantalla.Escribir(20, 6, citasConfirmadas[indice].IdCita, "gris");
        Pantalla.Escribir(2, 8, "Paciente: ", "blanco");
        Pantalla.Escribir(20, 8, citasConfirmadas[indice].Paciente.NombreCompleto, "gris");
        Pantalla.Escribir(2, 10, "Doctor: ", "blanco");
        Pantalla.Escribir(20, 10, citasConfirmadas[indice].Doctor.Nombre, "gris");
        Pantalla.Escribir(2, 12, "Fecha: ", "blanco");
        Pantalla.Escribir(20, 12, citasConfirmadas[indice].Fecha.ToString("g"), "gris");
        Pantalla.Escribir(2, 14, "Estado: ", "blanco");
        Pantalla.Escribir(20, 14, citasConfirmadas[indice].Cancelada ? "Cancelada" : "Confirmada", "gris");

        if (citasConfirmadas[indice] is CitaPrimera)
        {
            Pantalla.Escribir(2, 16, "Valoración bucal: ", "blanco");
            if (citasConfirmadas[indice] is CitaPrimera citaPrimera)
                Pantalla.Escribir(20, 16, citaPrimera.ValoracionBucal, "gris");
        }
        else if (citasConfirmadas[indice] is CitaNormal)
        {
            Pantalla.Escribir(2, 16, "Problemática: ", "blanco");
            if (citasConfirmadas[indice] is CitaNormal citaNormal)
                Pantalla.Escribir(20, 16, citaNormal.DescripcionProblema, "gris");

            Pantalla.Escribir(2, 18, "Tratamiento: ", "blanco");
            if (citasConfirmadas[indice] is CitaNormal citaNormal2)
                Pantalla.Escribir(20, 18, citaNormal2.Tratamiento, "gris");
        }
    }

    private static void Anyadir(int indice, bool ocultar, List<Cita> citasConfirmadas)
    {
        string idCita = "", nombreP = "", nombreD = "", valoracion = "",
            dProblema = "", tratamiento = "";
        DateTime fecha = new DateTime();
        Doctor doctor = null;
        Paciente paciente = null;

        RellenoCabecera(indice, ocultar, citasConfirmadas);
        string respuesta = Pantalla.PedirNormal(2, 6, "Es la primera cita del paciente? (s/n) ").ToLower();
        while (respuesta != "s" && respuesta != "n")
        {
            respuesta = Pantalla.PedirNormal(2, 6, "Es la primera cita del paciente? (s/n)").ToLower();
        }
        if (respuesta == "s")
        {
            PedirDatosGeneral(ref idCita, ref nombreP, ref nombreD, ref fecha);
            PedirDatos1aCita(ref valoracion);
            lista.ConvertirPacientesYDoctor(ref nombreP, ref nombreD, ref doctor, ref paciente);

            Pantalla.Escribir(2, 22, "1- Confirmar", "blanco");
            Pantalla.Escribir(25, 22, "S- Volver", "blanco");

            string confirmacion = Console.ReadLine();
            if (confirmacion == "1")
            {
                lista.Anyadir(new CitaPrimera(idCita, paciente, doctor,
                    fecha, false, valoracion));
                Pantalla.Escribir(2, 16, "Primera cita añadida correctamente", "verde");
                Console.ReadKey();
            }
        }
        else if (respuesta == "n")
        {
            PedirDatosGeneral(ref idCita, ref nombreP, ref nombreD, ref fecha);
            PedirDatosCitaNormal(ref dProblema, ref tratamiento);
            lista.ConvertirPacientesYDoctor(ref nombreP, ref nombreD, ref doctor, ref paciente);

            Pantalla.Escribir(2, 22, "1- Confirmar", "blanco");
            Pantalla.Escribir(25, 22, "S- Volver", "blanco");
            string confirmacion = Console.ReadLine();
            if (confirmacion == "1")
            {
                lista.Anyadir(new CitaNormal(idCita, paciente, doctor,
                    fecha, false, dProblema, tratamiento));
                Pantalla.Escribir(2, 18, "Cita añadida correctamente", "verde");
                Console.ReadKey();
            }
        }
    }

    private static int Numero(int totalCitas)
    {
        int indice = 0;
        do
        {
            indice = Convert.ToInt32(Pantalla.PedirNormal
            (2, 18, "Introduzca el número de registro ")) - 1;

        } while (indice > totalCitas || indice < 0);

        return indice;
    }

    private static ConsoleKey VerHorariosOcupados(int contador, List<Cita> citasConfirmadas)
    {
        DateTime ahora = DateTime.Now;
        string fecha = ahora.ToString("d");
        string hora = ahora.ToString("t");
        Pantalla.Ventana(0, 0, 80, 25, "azul");
        Pantalla.Escribir(2, 2, "Horario laboral (08:00 - 16:00) / Duración cita (30') ", "blanco");
        Pantalla.Escribir(57, 2, fecha, "gris");
        Pantalla.Escribir(71, 2, hora, "gris");
        Pantalla.Escribir(1, 20, new string('─', 78), "blanco");
        string texto = "Fechas de las citas confirmadas: ";
        Pantalla.Escribir((78 - texto.Length) / 2, 4, texto, "blanco");

        int maxItems = Math.Min(13, citasConfirmadas.Count - contador);
        for (int i = 0; i < maxItems; i++)
        {
            Pantalla.Escribir(30, i + 6, "- " + citasConfirmadas[i + contador].Fecha.ToString("g"), "amarillo");
        }
        Pantalla.Escribir(5, 22, "<- Anteriores fechas", "blanco");
        Pantalla.Escribir(30, 22, "-> Siguientes fechas", "blanco");
        Pantalla.Escribir(55, 22, "S- Volver", "blanco");
        ConsoleKeyInfo tecla = Console.ReadKey(true);
        return tecla.Key;
    }

    private static void Buscar(bool ocultar)
    {
        DateTime ahora = DateTime.Now;
        string fecha = ahora.ToString("d");
        string hora = ahora.ToString("t");
        Pantalla.Ventana(0, 0, 80, 25, "azul");
        Pantalla.Escribir(2, 2, "Búsqueda de citas: ", "blanco");
        Pantalla.Escribir(57, 2, fecha, "gris");
        Pantalla.Escribir(71, 2, hora, "gris");

        string textoBuscar = Pantalla.PedirNormal(2, 6, "Texto a buscar: ");
        if (ocultar == true)
        {
            string[] contienen = lista.ObtenerCitasConfirmadas(textoBuscar);
            if (contienen.Length > 0)
            {
                for (int i = 0; i < contienen.Length; i++)
                {
                    Pantalla.Escribir(5, (8 + i), contienen[i], "gris");
                }
                Console.ReadKey();
            }
            else
            {
                Pantalla.Escribir(2, 8, "No se han encontrado coincidencias.", "rojo");
                Console.ReadKey();
            }
        }
        else
        {
            string[] contienen = lista.ObtenerCitasTotales(textoBuscar);
            if (contienen.Length > 0)
            {
                for (int i = 0; i < contienen.Length; i++)
                {
                    if (contienen[i].Contains("true"))
                        Pantalla.Escribir(5, (8 + i), contienen[i], "rojo");
                    else
                        Pantalla.Escribir(5, (8 + i), contienen[i], "gris");
                }
                Console.ReadKey();
            }
            else
            {
                Pantalla.Escribir(2, 8, "No se han encontrado coincidencias.", "rojo");
            }
        }
    }

    private static void Modificar(int indice, bool ocultar, List<Cita> citasConfirmadas)
    {
        string valoracion = "", problematica = "", tratamiento = "";
        RellenoCabecera(indice, ocultar, citasConfirmadas);

        Pantalla.Escribir(2, 6, "ID cita: ", "blanco"); //no se debe modificar (PK)
        Pantalla.Escribir(19, 6, lista.Obtener(indice).IdCita, "gris");
        Pantalla.Escribir(2, 6, "Paciente: ", "blanco");
        string paciente = Pantalla.Pedir(19, 6, 25, lista.Obtener(indice).Paciente.NombreCompleto);
        Pantalla.Escribir(2, 8, "Doctor: ", "blanco");
        string doctor = Pantalla.Pedir(19, 8, 25, lista.Obtener(indice).Doctor.Nombre);
        Pantalla.Escribir(2, 10, "Fecha: ", "blanco");
        string fecha = Pantalla.Pedir(19, 10, 16, lista.Obtener(indice).Fecha.ToString("g"));


        if (lista.Obtener(indice) is CitaPrimera citaPrimera)
        {
            Pantalla.Escribir(2, 12, "Valoración: ", "blanco");
            valoracion = Pantalla.Pedir(19, 12, 25, citaPrimera.ValoracionBucal);
        }
        else if (lista.Obtener(indice) is CitaNormal citaNormal)
        {
            Pantalla.Escribir(2, 12, "Problemática: ", "blanco");
            problematica = Pantalla.Pedir(19, 12, 25, citaNormal.DescripcionProblema);
            Pantalla.Escribir(2, 14, "Tratamiento: ", "blanco");
            tratamiento = Pantalla.Pedir(19, 14, 25, citaNormal.Tratamiento);
        }

        Pantalla.Escribir(1, 20, new string('─', 78), "blanco");
        Pantalla.Escribir(2, 22, "1- Confirmar", "blanco");
        Pantalla.Escribir(25, 22, "S- Volver", "blanco");

        string confirmacion = Console.ReadLine();
        if (confirmacion == "1" && (lista.Obtener(indice) is CitaPrimera))
        {
            lista.Modificar1aCita(indice, paciente, doctor, fecha, valoracion); //revisar
            Pantalla.Escribir(2, 16, "Paciente modificado correctamente", "verde");
            Console.ReadKey();
        }
        else if (confirmacion == "1" && (lista.Obtener(indice) is CitaNormal))
        {
            lista.ModificarCitaNormal(indice, paciente, doctor, fecha, problematica, tratamiento);
            Pantalla.Escribir(2, 16, "Paciente modificado correctamente", "verde");
            Console.ReadKey();
        }
    }

    private static void Cancelar(int indice)
    {
        MostrarAviso(indice);
    }

    private static void Configuracion(ref bool ocultar)
    {
        DateTime ahora = DateTime.Now;
        string fecha = ahora.ToString("d");
        string hora = ahora.ToString("t");
        Pantalla.Ventana(0, 0, 80, 25, "azul");
        Pantalla.Escribir(2, 2, "Ajustes de configuración: ", "blanco");
        Pantalla.Escribir(57, 2, fecha, "gris");
        Pantalla.Escribir(71, 2, hora, "gris");

        Pantalla.Escribir(2, 6, "Apariencia: ", "blanco");
        Pantalla.Escribir(2, 8, "1- Ocultar citas canceladas (por defecto)", "gris");
        Pantalla.Escribir(2, 10, "2- Mostrar citas canceladas", "gris");
        Pantalla.Escribir(2, 12, "S- Volver", "gris");

        string confirmacion = Console.ReadLine();
        if (confirmacion == "1")
        {
            ocultar = true;
            Pantalla.Escribir(2, 14, "Las citas canceladas serán ocultadas", "verde");
            Console.ReadKey();
        }
        else if (confirmacion == "2")
        {
            ocultar = false;
            Pantalla.Escribir(2, 14, "Las citas canceladas se podrán observar", "verde");
            Console.ReadKey();
        }
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
            || (hora < 8 || hora >= 16) || (minutos < 0 || minutos > 59))
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

    private static void RellenoMenu(int indice, bool ocultar, List<Cita> citasConfirmadas)
    {
        DateTime ahora = DateTime.Now;
        string fecha = ahora.ToString("d");
        string hora = ahora.ToString("t");

        Pantalla.Ventana(0, 0, 80, 25, "azul");
        if (ocultar == false)
        {
            Pantalla.Escribir(2, 2, "Citas (actual: "
                + (indice + 1) + "/" + lista.CantidadDeCitas() + ")", "blanco");
        }
        else
        {
            Pantalla.Escribir(2, 2, "Citas (actual: "
                + (indice + 1) + "/" + citasConfirmadas.Count + ")", "blanco");
        }

        Pantalla.Escribir(57, 2, fecha, "gris");
        Pantalla.Escribir(71, 2, hora, "gris");

        Pantalla.Escribir(1, 20, new string('─', 78), "blanco");

        Pantalla.Escribir(2, 22, "<- Anterior", "blanco");
        Pantalla.Escribir(15, 22, "-> Siguiente", "blanco");
        Pantalla.Escribir(29, 22, "1- Añadir", "blanco");
        Pantalla.Escribir(40, 22, "2- Número", "blanco");
        Pantalla.Escribir(51, 22, "3- Horarios", "blanco");
        Pantalla.Escribir(2, 23, "4- Buscar", "blanco");
        Pantalla.Escribir(13, 23, "5- Modificar", "blanco");
        Pantalla.Escribir(27, 23, "6- Cancelar", "blanco");
        Pantalla.Escribir(40, 23, "7- Config", "blanco");
        Pantalla.Escribir(51, 23, "S- Volver", "blanco");
    }

    private static void RellenoCabecera(int indice, bool ocultar, List<Cita> citasConfirmadas)
    {
        DateTime ahora = DateTime.Now;
        string fecha = ahora.ToString("d");
        string hora = ahora.ToString("t");

        Pantalla.Ventana(0, 0, 80, 25, "azul");
        if (ocultar == false)
        {
            Pantalla.Escribir(2, 2, "Citas (actual: "
                + (indice + 1) + "/" + lista.CantidadDeCitas() + ")", "blanco");
        }
        else
        {
            Pantalla.Escribir(2, 2, "Citas (actual: "
                + (indice + 1) + "/" + citasConfirmadas.Count + ")", "blanco");
        }

        Pantalla.Escribir(57, 2, fecha, "gris");
        Pantalla.Escribir(71, 2, hora, "gris");

        Pantalla.Escribir(1, 20, new string('─', 78), "blanco");
    }

    private static void MostrarAviso(int indice)
    {
        Pantalla.VentanaAviso(20, 8, 40, 10);
        Pantalla.Escribir((78 - "cancelar cita".Length) / 2 +3, 10, "Cancelar cita", "amarillo");
        Pantalla.VentanaAviso(21, 14, 19, 3);
        Pantalla.Escribir(26, 15, "1- Aceptar", "amarillo");
        Pantalla.VentanaAviso(40, 14, 19, 3);
        Pantalla.Escribir(45, 15, "S- Volver", "amarillo");

        string confirmacion = Console.ReadLine();
        if (confirmacion == "1")
        {
            lista.Cancelar(lista.Obtener(indice));
            Pantalla.Escribir((78-"Cita cancelada correctamente".Length)/2,
                13, "Cita cancelada correctamente", "cian");
            Console.ReadKey();
        }
        Pantalla.BorrarVentanaAviso(20, 8, 40, 10);
        Pantalla.Escribir(2, 8, "Paciente: ", "blanco");
        Pantalla.Escribir(20, 8, lista.Obtener(indice).Paciente.NombreCompleto, "gris");
        Pantalla.Escribir(2, 10, "Doctor: ", "blanco");
        Pantalla.Escribir(20, 10, lista.Obtener(indice).Doctor.Nombre, "gris");
        Pantalla.Escribir(2, 12, "Fecha: ", "blanco");
        Pantalla.Escribir(20, 12, lista.Obtener(indice).Fecha.ToString("g"), "gris");
        Pantalla.Escribir(2, 14, "Estado: ", "blanco");
        Pantalla.Escribir(20, 14, lista.Obtener(indice).Cancelada ? "Cancelada" : "Confirmada", "gris");

        if (lista.Obtener(indice) is CitaPrimera)
        {
            Pantalla.Escribir(2, 16, "Valoración bucal: ", "blanco");
            if (lista.Obtener(indice) is CitaPrimera citaPrimera)
                Pantalla.Escribir(20, 16, citaPrimera.ValoracionBucal, "gris");
        }
        else if (lista.Obtener(indice) is CitaNormal)
        {
            Pantalla.Escribir(2, 16, "Problemática: ", "blanco");
            if (lista.Obtener(indice) is CitaNormal citaNormal)
                Pantalla.Escribir(19, 16, citaNormal.DescripcionProblema, "gris");

            Pantalla.Escribir(2, 18, "Tratamiento: ", "blanco");
            if (lista.Obtener(indice) is CitaNormal citaNormal2)
                Pantalla.Escribir(19, 18, citaNormal2.Tratamiento, "gris");
        }
    }
}