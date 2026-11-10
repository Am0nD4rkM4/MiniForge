// ============================================================================
// Sistema.cs — información del equipo (como el módulo "sistema" de InfoForge)
// ============================================================================
// Esta clase es el MOLDE del que hablábamos: agrupa todo lo relacionado con
// el sistema. No hay que crear un objeto de ella: sus métodos son "static",
// así que se llaman como Sistema.Mostrar() directamente.
//
// Environment es una clase que ya trae .NET con datos del sistema operativo.
// DriveInfo es otra clase de .NET que describe un disco.
// GC es el recolector de basura de .NET; también sabe cuánta RAM hay.
// ============================================================================

using System;
using System.IO;

namespace MiniForge
{
    // "public" = cualquiera puede usar esta clase desde otro archivo.
    // "class Sistema" = el molde llamado Sistema.
    public static class Sistema
    {
        // "public static void Mostrar()" = método público, de la clase,
        // que no devuelve nada (void = vacío, como una función de Python
        // que no tiene return).
        public static void Mostrar()
        {
            Console.WriteLine("--- Sistema ---");

            // Environment.MachineName = nombre del equipo (tu "Tweeky").
            // Environment.UserName    = tu usuario (am0nd4rk).
            // Environment.OSVersion   = sistema operativo y versión.
            Console.WriteLine($"Equipo:        {Environment.MachineName}");
            Console.WriteLine($"Usuario:       {Environment.UserName}");
            Console.WriteLine($"Sistema:       {Environment.OSVersion}");
            Console.WriteLine($"64 bits:       {Environment.Is64BitOperatingSystem}");

            // Environment.ProcessorCount = cuántos núcleos (lógicos) tiene la CPU.
            Console.WriteLine($"Procesadores:  {Environment.ProcessorCount}");

            // GC.GetGCMemoryInfo().TotalAvailableMemoryBytes = RAM total que
            // el sistema le deja ver a .NET, en bytes. Lo dividimos entre
            // 1024*1024*1024 para pasarlo a GB. El ":F2" dentro de { } le
            // dice que muestre solo 2 decimales, como round(x, 2) en Python.
            double ramGB = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes
                           / (1024.0 * 1024.0 * 1024.0);
            Console.WriteLine($"RAM total:     {ramGB:F2} GB");

            // Environment.CurrentDirectory = la carpeta desde donde se corre.
            Console.WriteLine($"Carpeta:       {Environment.CurrentDirectory}");

            Console.WriteLine("\n--- Discos ---");

            // DriveInfo.GetDrives() devuelve una LISTA de discos.
            // "foreach (var disco in ...)" = el "for disco in ..." de Python.
            foreach (var disco in DriveInfo.GetDrives())
            {
                // IsReady = el disco está listo (un USB sin meter da false).
                // TotalSize == 0 = no es un disco real (son carpetas del
                // sistema como /sys o /dev, que no tienen tamaño).
                if (!disco.IsReady || disco.TotalSize == 0)
                {
                    continue; // saltar a la siguiente vuelta del bucle.
                }

                double totalGB = disco.TotalSize / (1024.0 * 1024.0 * 1024.0);
                double libreGB = disco.AvailableFreeSpace / (1024.0 * 1024.0 * 1024.0);

                // disco.Name = "C:\" o "/". disco.DriveFormat = "ext4", "NTFS"...
                Console.WriteLine($"{disco.Name} ({disco.DriveFormat}) - " +
                                  $"total {totalGB:F1} GB, libre {libreGB:F1} GB");
            }
        }
    }
}
