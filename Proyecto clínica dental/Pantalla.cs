using System;
using System.Drawing;

class Pantalla
{
    private static Dictionary<string, ConsoleColor> colores = new Dictionary<string, ConsoleColor>
    {
        { "negro", ConsoleColor.Black },
        { "azul", ConsoleColor.Blue },
        { "cian", ConsoleColor.Cyan },
        { "gris", ConsoleColor.Gray },
        { "verde", ConsoleColor.Green },
        { "magenta", ConsoleColor.Magenta },
        { "rojo", ConsoleColor.Red },
        { "amarillo", ConsoleColor.Yellow },
        { "blanco", ConsoleColor.White }
    };

    private const string COLOR_FONDO = "azul";

    public static void Escribir(int x, int y, string texto, string color)
    {
        ConsoleColor colorOriginal = Console.ForegroundColor;
        ConsoleColor colorOriginalFondo = Console.BackgroundColor;

        if (colores.ContainsKey(color.ToLower()))
        {
            Console.ForegroundColor = colores[color.ToLower()];
        }
        Console.BackgroundColor = colores[COLOR_FONDO];

        Console.SetCursorPosition(x, y);
        Console.Write(texto);

        Console.ForegroundColor = colorOriginal;
        Console.BackgroundColor = colorOriginalFondo; //cambio
    }

    public static string PedirNormal(int x, int y, string texto)
    {
        Console.ForegroundColor = colores["blanco"];
        Console.BackgroundColor = colores[COLOR_FONDO];
        Console.SetCursorPosition(x, y);
        Console.Write(texto);
        Console.BackgroundColor = colores["blanco"];
        Console.ForegroundColor = colores["negro"];
        return Console.ReadLine();
    }
    public static string Pedir(int x, int y, int longitudMax)
    {
        return Pedir(x, y, longitudMax, "");
    }

    public static string Pedir(int x, int y, int longitudMax, string valorAnterior)
    {
        Console.ForegroundColor = colores["gris"];
        Console.BackgroundColor = colores[COLOR_FONDO];
        Console.SetCursorPosition(x, y);
        Console.Write("[" + valorAnterior + 
            new string('.', longitudMax-valorAnterior.Length) + "]  ");
        Console.BackgroundColor = colores["blanco"];
        Console.ForegroundColor = colores["negro"];
        string input = Console.ReadLine();
        return input;
    }

    public static void Ventana(int x, int y, int ancho, int alto, string colorFondo)
    {
        ConsoleColor colorOriginalFondo = Console.BackgroundColor;
        ConsoleColor colorOriginalLetra = Console.ForegroundColor;

        string colorBorde = "magenta";

        for (int fila = 0; fila < alto; fila++)
        {
            for (int columna = 0; columna < ancho; columna++)
            {
                //borde exterior magenta
                Console.SetCursorPosition(x + columna, y + fila);
                if (colores.ContainsKey(colorFondo.ToLower()))
                {
                    Console.BackgroundColor = colores[colorFondo.ToLower()];
                }
                if (fila == 0 && columna == 0)
                {
                    Console.ForegroundColor = colores[colorBorde];
                    Console.Write("╔");
                    Console.ForegroundColor = colorOriginalLetra;
                }
                else if (fila == 0 && columna == ancho - 1)
                {
                    Console.ForegroundColor = colores[colorBorde];
                    Console.Write("╗");
                    Console.ForegroundColor = colorOriginalLetra;
                }
                else if (fila == alto - 1 && columna == 0)
                {
                    Console.ForegroundColor = colores[colorBorde];
                    Console.Write("╚");
                    Console.ForegroundColor = colorOriginalLetra;
                }
                else if (fila == alto - 1 && columna == ancho - 1)
                {
                    Console.ForegroundColor = colores[colorBorde];
                    Console.Write("╝");
                    Console.ForegroundColor = colorOriginalLetra;
                }
                else if ((fila == 0 && columna != 0 && columna < ancho-1) ||
                    (fila == alto-1 && columna != 0 && columna < ancho-1))    
                {
                    Console.ForegroundColor = colores[colorBorde];
                    Console.Write("═");
                    Console.ForegroundColor = colorOriginalLetra;
                }
                else if ((columna == 0 && fila != 0 && fila < alto - 1) ||
                    (columna == ancho - 1 && fila != 0 && fila < alto - 1))
                {
                    Console.ForegroundColor = colores[colorBorde];
                    Console.Write("║");
                    Console.ForegroundColor = colorOriginalLetra;
                }
                //rectangulo interior blanco
                else if (fila == 1 && columna == 1)
                {
                    Console.Write("┌");
                }
                else if (fila == 1 && columna == ancho - 2)
                {
                    Console.Write("┐");
                }
                else if (fila == 3 && columna == 1)
                {     
                    Console.Write("└");
                }
                else if (fila == 3 && columna == ancho - 2)
                {
                    Console.Write("┘");
                }
                else if ((fila == 1 && columna > 1 && columna < ancho - 2)
                    || (fila == 3 && columna > 1 && columna < ancho - 2)
                    || (fila == 20 && columna > 1 && columna < ancho - 2))
                {
                    Console.Write("─");
                }
                else if (fila == 3 && columna > 1 && columna < ancho - 2)
                {  
                    Console.Write("─");
                }
                else if ((fila == 2 && columna == 1) || (fila == 2 && columna == ancho - 2))
                {      
                    Console.Write("│");
                }
                else
                {
                    Console.Write(" ");
                }
            }
        }
        Console.BackgroundColor = colorOriginalFondo;
    }

    public static void VentanaAviso(int x, int y, int ancho, int alto)
    {
        Console.BackgroundColor = colores["azul"];
        Console.ForegroundColor = colores["cian"];
        Console.SetCursorPosition(x, y);
        Console.Write("╔");
        Console.SetCursorPosition(x + ancho - 1, y);
        Console.Write("╗");
        Console.SetCursorPosition(x, y + alto - 1);
        Console.Write("╚");
        Console.SetCursorPosition(x + ancho - 1, y + alto - 1);
        Console.Write("╝");
        for (int i = 1; i < ancho - 1; i++)
        {
            Console.SetCursorPosition(x + i, y);
            Console.Write("═");
            Console.SetCursorPosition(x + i, y + alto - 1);
            Console.Write("═");
        }
        for (int i = 1; i < alto - 1; i++)
        {
            Console.SetCursorPosition(x, y + i);
            Console.Write("║");
            Console.SetCursorPosition(x + ancho - 1, y + i);
            Console.Write("║");

            for (int j = 1; j < ancho - 1; j++)
            {
                Console.SetCursorPosition(x + j, y + i);
                Console.Write(" ");
            }
        }  
    }

    public static void BorrarVentanaAviso(int x, int y, int ancho, int alto)
    {
        Console.BackgroundColor = colores["azul"];
        for (int i = 0; i < alto; i++)
        {
            for (int j = 0; j < ancho; j++)
            {
                Console.SetCursorPosition(x + j, y + i);
                Console.Write(" ");
            }
        }
    }
}