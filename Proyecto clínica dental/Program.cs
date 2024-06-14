using System;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Geom;
using iText.Layout.Properties;

class GestorClinicaPrincipal
{
    public enum Semana { EstaSemana = 1, SiguienteSemana = 2}
    static void Main(string[] args)
    {
        List<Empleado> empleados = GestorDeEmpleados.lista.ListaEmpleados();
        string login = "";
        string password = "";
        Empleado empleadoVerificado = null;
        
        Login(ref login, ref password, ref empleadoVerificado, ref empleados);
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

    static void Login(ref string login, ref string password, ref Empleado empleadoVerificado,
        ref List<Empleado> empleados)
    {
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
        if (empleadoVerificado is Auxiliar)
        {
            MenuAuxiliar(login, password, empleadoVerificado, empleados);
        }
    }

    static void MenuAuxiliar(string login, string password,
        Empleado empleadoVerificado, List<Empleado> empleados)
    {
        string opcion;
        do
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            MenuAuxiliar2();
            opcion = Pantalla.PedirNormal(25, 14, "Escoge una opción: ").ToUpper();

            switch (opcion)
            {
                case "1":
                    GestorDePacientes.Lanzar();
                    break;
                case "2":
                    GestorDeCitas.Lanzar();
                    break;
                case "3":
                    //GestorDePresupuestos.Lanzar();
                    break;
                case "4":
                    GenerarHorario();
                    break;
                case "S":
                    Login(ref login, ref password, ref empleadoVerificado, ref empleados);
                    break;
                default:
                    Console.WriteLine();
                    break;
            }
        } while (opcion != "S");
    }

    static void MenuAuxiliar2()
    {
        Pantalla.Ventana(0, 0, 80, 25, "azul");
        Pantalla.Escribir(25, 2, "Menú de gestión para auxiliar", "blanco");
        Pantalla.Escribir(25, 6, "1. Gestion de pacientes", "blanco");
        Pantalla.Escribir(25, 8, "2. Gestion de citas", "blanco");
        Pantalla.Escribir(25, 10, "3. Gestion de presupuestos", "blanco");
        Pantalla.Escribir(25, 12, "4. Generar horario semanal", "blanco");
        Pantalla.Escribir(25, 14, "S. Volver", "blanco");
    }

    static void GenerarHorario()
    {
        string respuesta1;
        do
        {
            Pantalla.Ventana(0, 0, 80, 25, "azul");
            Pantalla.Escribir(20, 2, "Seleccione el método de exportación", "blanco");
            Pantalla.Escribir(25, 6, "1. Exportar a PDF", "amarillo");
            Pantalla.Escribir(25, 8, "2. Exportar a Excel", "amarillo");
            Pantalla.Escribir(25, 10, "S. Volver", "amarillo");
            respuesta1 = Console.ReadLine().ToUpper();

            switch (respuesta1)
            {
                case "1":
                    MenuSemana(respuesta1);
                    break;
                case "2":
                    //excel
                    break;
                case "S":
                    break;
            }
        } while (respuesta1 != "S");
    }

    static void GenerarPDF(int semana)
    {
        string[] nombresMeses = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
            "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"};
        List<Cita> citas = GestorDeCitas.lista.ListaCitasConfirmadas();
        PdfWriter writer = new PdfWriter("Horario-Semanal.pdf");
        PdfDocument pdf = new PdfDocument(writer);
        using var doc = new Document(pdf);
        pdf.SetDefaultPageSize(PageSize.A4.Rotate());
        doc.SetMargins(15,15, 15, 15);

        //Calcular fechas de lunes a viernes de esta semana y la próxima
        DateTime hoy = DateTime.Today;
        DateTime lunes;
        DateTime viernes;
        if (semana == 1)
        {
            lunes = hoy.AddDays(-(int)hoy.DayOfWeek + (int)DayOfWeek.Monday);
        }
        else
        {
            lunes = hoy.AddDays(-(int)hoy.DayOfWeek + (int)DayOfWeek.Monday + 7);
        }
        viernes = lunes.AddDays(4);

        Paragraph cabecera = new Paragraph("Semana del " + lunes.Day.ToString("00")
                + " al " + viernes.Day.ToString("00") + " de " + nombresMeses[lunes.Month - 1])
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(20).SetBold();
        doc.Add(cabecera);
        doc.Add(new Paragraph("\n"));
        float[] columnas = { 1, 1, 1, 1, 1 };
        Table tabla = new Table(UnitValue.CreatePercentArray(columnas));
        tabla.SetWidth(UnitValue.CreatePercentValue(100));
        tabla.AddHeaderCell(new Cell().Add(new Paragraph("Lunes")).SetBold()).SetTextAlignment(TextAlignment.CENTER); 
        tabla.AddHeaderCell(new Cell().Add(new Paragraph("Martes")).SetBold()).SetTextAlignment(TextAlignment.CENTER); 
        tabla.AddHeaderCell(new Cell().Add(new Paragraph("Miércoles")).SetBold()).SetTextAlignment(TextAlignment.CENTER);
        tabla.AddHeaderCell(new Cell().Add(new Paragraph("Jueves")).SetBold()).SetTextAlignment(TextAlignment.CENTER); 
        tabla.AddHeaderCell(new Cell().Add(new Paragraph("Viernes")).SetBold()).SetTextAlignment(TextAlignment.CENTER); 

        List<Cita>[] citasPorDia = new List<Cita>[5];
        int j = 0;
        while (j < 5)
        {
            citasPorDia[j] = new List<Cita>();
            j++;
        }
        foreach (Cita cita in citas)
        {
            int diferenciaDias = (cita.Fecha.Date - lunes.Date).Days;
            if (diferenciaDias >= 0 && diferenciaDias < 5)
                citasPorDia[diferenciaDias].Add(cita);
        }
        for (int i = 0; i < 5; i++)
        {
            if (citasPorDia[i].Count == 0)
                tabla.AddCell(new Cell().Add(new Paragraph("")));
            else
            {
                string datos = "";
                foreach (Cita cita in citasPorDia[i])
                {
                    datos = cita.Fecha.ToString("t") + " - " + cita.Paciente.NombreCompleto + "\n";
                }
                if (datos.EndsWith("\n"))
                    datos = datos.Substring(0, datos.Length - 1);

                tabla.AddCell(new Cell().Add(new Paragraph(datos)));
            }
        }   
        doc.Add(tabla);
        doc.Close();
    }

    static void GenerarExcel(int semana)
    {

    }

    static void MenuSemana(string respuesta1)
    {
        Pantalla.VentanaAviso(20, 12, 40, 10);
        Pantalla.Escribir(22, 14, "Seleccione la semana", "blanco");
        Pantalla.Escribir(22, 16, "1. Esta semana", "blanco");
        Pantalla.Escribir(22, 17, "2. Siguiente semana", "blanco");
        Pantalla.Escribir(22, 19, "S. Volver", "blanco");
        string respuesta2 = Console.ReadLine().ToUpper();
        Pantalla.BorrarVentanaAviso(20, 12, 40, 10);

        if (respuesta1 == "1")
            ExportarPDF(respuesta2);
        /*else if (respuesta1 == "2")
            ExportarExcel(respuesta2);*/
    }

    static void ExportarPDF(string opcion)
    {
        switch (opcion)
        {
            case "1":
                GenerarPDF((int)Semana.EstaSemana);
                Pantalla.Escribir(30, 15, "PDF creado correctamente", "verde");
                Console.ReadKey();
                break;
            case "2":
                GenerarPDF((int)Semana.SiguienteSemana);
                Pantalla.Escribir(30, 15, "PDF creado correctamente", "verde");
                Console.ReadKey();
                break;
        }
    }

    static void ExportarExcel(string opcion)
    {
        switch (opcion)
        {
            case "1":
                GenerarExcel((int)Semana.EstaSemana);
                Pantalla.Escribir(30, 15, "Excel creado correctamente", "verde");
                Console.ReadKey();
                break;
            case "2":
                GenerarExcel((int)Semana.SiguienteSemana);
                Pantalla.Escribir(30, 15, "Excel creado correctamente", "verde");
                Console.ReadKey();
                break;
        }
    }
}
