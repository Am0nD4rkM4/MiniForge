// ============================================================================
// MiniForge v1.0 — mini herramienta de obtención de información, en C#
// ============================================================================
// Es un InfoForge en miniatura: solo 3 archivos, ~300 líneas, para que
// puedas leerlo entero y entender la sintaxis de C#.
//
// CÓMO LEER ESTE ARCHIVO (C# comparado con Python):
//   namespace MiniForge { ... }  -> como un "paquete" de Python: agrupa clases.
//   class Program { ... }        -> una CLASE es un molde: define datos y
//                                  acciones. Aquí el molde se llama Program.
//   static async Task Main()     -> Main es donde EMPIEZA el programa,
//                                  como el "if __name__ == '__main__'" de
//                                  Python. "static" = pertenece a la clase,
//                                  no hace falta crear un objeto para usarlo.
//   string texto = "...";        -> las variables llevan su TIPO delante.
//                                  En Python escribirías: texto = "..."
//   var numero = 5;              -> "var" deja que C# deduzca el tipo solo.
//   Console.WriteLine(...)       -> como print() en Python.
//   $"Hola {nombre}"            -> interpolación: mete variables dentro del
//                                  texto. En Python sería f"Hola {nombre}".
//   // comentario                -> como el # de Python.
//
// PARA CORRERLO (en Debian 13):
//   sudo apt install -y dotnet-sdk-8.0
//   dotnet run
// ============================================================================

using System;
using System.Threading.Tasks;

namespace MiniForge
{
    class Program
    {
        // async Task Main: el programa puede esperar cosas de red (await)
        // sin quedarse congelado. Si no hubiera red, sería "static void Main()".
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== MiniForge v1.0 — InfoForge en miniatura (C#) ===");
            Console.WriteLine("Escribe el número de una opción y pulsa Enter.\n");

            // while (true) = bucle infinito: el menú se repite hasta elegir 0.
            while (true)
            {
                Console.WriteLine("1. Información del sistema");
                Console.WriteLine("2. Interfaces de red");
                Console.WriteLine("3. Resolver DNS de un dominio");
                Console.WriteLine("4. Hacer ping a un host");
                Console.WriteLine("5. Probar si un puerto está abierto");
                Console.WriteLine("6. Ver cabeceras HTTP de una web");
                Console.WriteLine("0. Salir");
                Console.Write("\nOpción: ");

                // Console.ReadLine() lee lo que escribes. "?" en string?
                // significa "puede ser nulo": si pulsas Enter sin escribir
                // nada, ReadLine devuelve null.
                string? opcion = Console.ReadLine();
                Console.WriteLine();

                // switch = como muchos if/else encadenados, pero más limpio.
                switch (opcion)
                {
                    case "1":
                        Sistema.Mostrar();
                        break;
                    case "2":
                        Red.ListarInterfaces();
                        break;
                    case "3":
                        Console.Write("Dominio (ej. google.com): ");
                        Red.ResolverDns(PedirTexto());
                        break;
                    case "4":
                        Console.Write("Host (ej. 8.8.8.8): ");
                        Red.HacerPing(PedirTexto());
                        break;
                    case "5":
                        Console.Write("Host (ej. google.com): ");
                        string host = PedirTexto();
                        Console.Write("Puerto (ej. 443): ");
                        // int.TryParse: intenta convertir texto a número.
                        // Si falla, devuelve false y no rompe el programa.
                        if (int.TryParse(PedirTexto(), out int puerto))
                        {
                            // await = "espera a que termine la red y sigue".
                            await Red.ProbarPuerto(host, puerto);
                        }
                        else
                        {
                            Console.WriteLine("Eso no es un número de puerto.");
                        }
                        break;
                    case "6":
                        Console.Write("URL (ej. https://example.com): ");
                        await Red.VerCabeceras(PedirTexto());
                        break;
                    case "0":
                        Console.WriteLine("Hasta luego.");
                        return; // salir de Main = terminar el programa.
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

                Console.WriteLine("\nPulsa Enter para continuar...");
                Console.ReadLine();
                Console.WriteLine();
            }
        }

        // Método auxiliar: pide texto y si viene vacío devuelve "".
        // "static string" = método que devuelve un string.
        // El "??" significa: "si lo de la izquierda es nulo, usa lo de
        // la derecha". En Python sería: texto if texto is not None else "".
        static string PedirTexto()
        {
            return Console.ReadLine() ?? "";
        }
    }
}
