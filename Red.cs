// ============================================================================
// Red.cs — red y DNS (como los módulos de red y DNS de InfoForge)
// ============================================================================
// Aquí se ven 4 ideas nuevas de C#:
//
//   using var x = new Algo();  -> "using" le dice a C#: "cuando termine
//                                 este bloque, libera x solo". Es como el
//                                 "with open(...) as f:" de Python.
//   try { ... } catch (Exception ex) { ... }
//                              -> como try/except de Python: si algo falla
//                                 en el try, se ejecuta el catch. ex.Message
//                                 es el texto del error.
//   await AlgoAsync(...)       -> espera a que termine una operación de red
//                                 sin bloquear el programa. El método que
//                                 usa await tiene que ser "async Task".
//   new Ping() / new TcpClient()
//                              -> "new" CREA un objeto a partir del molde
//                                 (la clase). Como en Python: ping = Ping().
// ============================================================================

using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MiniForge
{
    public static class Red
    {
        // Lista las interfaces de red: como el "ip a" de Linux.
        public static void ListarInterfaces()
        {
            Console.WriteLine("--- Interfaces de red ---");

            // NetworkInterface.GetAllNetworkInterfaces() devuelve todas.
            foreach (var tarjeta in NetworkInterface.GetAllNetworkInterfaces())
            {
                Console.WriteLine($"[{tarjeta.Name}] {tarjeta.Description}");
                Console.WriteLine($"  Estado: {tarjeta.OperationalStatus}");
                Console.WriteLine($"  Tipo:   {tarjeta.NetworkInterfaceType}");
                Console.WriteLine($"  MAC:    {tarjeta.GetPhysicalAddress()}");

                // GetIPProperties().UnicastAddresses = las IPs de la tarjeta.
                foreach (var direccion in tarjeta.GetIPProperties().UnicastAddresses)
                {
                    Console.WriteLine($"  IP:     {direccion.Address}");
                }
                Console.WriteLine();
            }
        }

        // Resuelve un dominio a sus IPs: como "nslookup google.com".
        public static void ResolverDns(string host)
        {
            try
            {
                // Dns.GetHostEntry pregunta al DNS (el de /etc/resolv.conf).
                IPHostEntry entrada = Dns.GetHostEntry(host);
                Console.WriteLine($"--- DNS de {host} ---");
                Console.WriteLine($"Nombre real: {entrada.HostName}");

                foreach (var ip in entrada.AddressList)
                {
                    Console.WriteLine($"  IP: {ip}");
                }
            }
            catch (Exception ex)
            {
                // Si el dominio no existe o no hay red, se llega aquí.
                Console.WriteLine($"No se pudo resolver {host}: {ex.Message}");
            }
        }

        // Hace ping: como "ping -c 1 8.8.8.8".
        public static void HacerPing(string host)
        {
            try
            {
                using var ping = new Ping();

                // Send(host, 3000) = 1 ping con 3 segundos de tiempo máximo.
                PingReply respuesta = ping.Send(host, 3000);

                Console.WriteLine($"--- Ping a {host} ---");
                Console.WriteLine($"Estado:  {respuesta.Status}");
                Console.WriteLine($"Tiempo:  {respuesta.RoundtripTime} ms");
                Console.WriteLine($"IP:      {respuesta.Address}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ping fallido: {ex.Message}");
            }
        }

        // Prueba si un puerto TCP está abierto: como "nc -zv host puerto".
        // Es "async Task" porque ConnectAsync necesita await.
        public static async Task ProbarPuerto(string host, int puerto)
        {
            try
            {
                // CancellationTokenSource = un temporizador de 3 segundos:
                // si no conecta en ese tiempo, se cancela solo.
                using var tiempo = new CancellationTokenSource(TimeSpan.FromSeconds(3));
                using var cliente = new TcpClient();

                await cliente.ConnectAsync(host, puerto, tiempo.Token);

                Console.WriteLine($"Puerto {puerto} en {host}: ABIERTO");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Puerto {puerto} en {host}: cerrado o sin respuesta ({ex.Message})");
            }
        }

        // Pide una web y muestra su código y cabeceras: como "curl -I url".
        public static async Task VerCabeceras(string url)
        {
            try
            {
                using var http = new HttpClient();
                http.Timeout = TimeSpan.FromSeconds(10);

                // GetAsync hace la petición GET y espera la respuesta.
                using HttpResponseMessage respuesta = await http.GetAsync(url);

                Console.WriteLine($"--- Cabeceras de {url} ---");
                Console.WriteLine($"Código: {(int)respuesta.StatusCode} {respuesta.StatusCode}");

                Console.WriteLine("Cabeceras de respuesta:");
                foreach (var cabecera in respuesta.Headers)
                {
                    // string.Join(", ", ...) une una lista con comas,
                    // como ", ".join(lista) en Python.
                    Console.WriteLine($"  {cabecera.Key}: {string.Join(", ", cabecera.Value)}");
                }

                Console.WriteLine("Cabeceras del contenido:");
                foreach (var cabecera in respuesta.Content.Headers)
                {
                    Console.WriteLine($"  {cabecera.Key}: {string.Join(", ", cabecera.Value)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo pedir {url}: {ex.Message}");
            }
        }
    }
}
