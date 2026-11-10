# MiniForge v1.0 — InfoForge en miniatura, en C#

Un proyecto **pequeño y para leer**, hecho para aprender la sintaxis de C#.
Es lo mismo que hace InfoForge en pequeño: información del sistema y de la
red, pero en solo 3 archivos y unas 300 líneas, con cada cosa explicada en
español dentro del propio código.

También está en GitHub: https://github.com/Am0nD4rkM4/MiniForge (repo
público, creado por Luis). Es un proyecto para analizar en casa.

## Archivos

| Archivo | Qué es | Qué enseña de C# |
|---|---|---|
| `MiniForge.csproj` | El archivo del proyecto (qué .NET usa) | `TargetFramework`, `OutputType` |
| `Program.cs` | El menú y el punto de entrada | `namespace`, `class`, `Main`, `switch`, `while`, `string?`, `??`, `int.TryParse`, `await` |
| `Sistema.cs` | Información del equipo | `static class`, `Environment`, `DriveInfo`, `foreach`, interpolación `$"..."`, formato `{valor:F2}` |
| `Red.cs` | DNS, ping, puertos y cabeceras HTTP | `new`, `using var`, `try/catch`, `async Task`, `HttpClient`, `TcpClient`, `Ping` |

## Cómo correrlo (Debian 13, en tu laptop con ForgeOS)

```bash
# 1. Instala el SDK de .NET 8 (solo la primera vez)
sudo apt update
sudo apt install -y dotnet-sdk-8.0

# 2. Descomprime el proyecto y entra
unzip miniforge-csharp.zip
cd miniforge-csharp

# 3. Compila y corre (la primera vez descarga paquetes y tarda un poco)
dotnet run
```

Verás el menú. Prueba, por ejemplo:

- Opción 1: información de tu equipo (verás tu `Tweeky` y tu usuario).
- Opción 3 con `google.com`: sus IPs.
- Opción 4 con `8.8.8.8`: el ping.
- Opción 5 con `google.com` y puerto `443`: abierto.
- Opción 6 con `https://example.com`: sus cabeceras HTTP.

## C# frente a Python, en una tabla

| Idea | Python | C# |
|---|---|---|
| Imprimir | `print("hola")` | `Console.WriteLine("hola")` |
| Variable de texto | `nombre = "Luis"` | `string nombre = "Luis";` |
| Variable que se deduce | — | `var numero = 5;` |
| Texto con variables | `f"Hola {nombre}"` | `$"Hola {nombre}"` |
| Función | `def saludar():` | `static void Saludar() { ... }` |
| Condición | `if x == 1:` | `if (x == 1) { ... }` |
| Bucle | `for x in lista:` | `foreach (var x in lista) { ... }` |
| Manejo de errores | `try: ... except Exception as e:` | `try { ... } catch (Exception ex) { ... }` |
| Clase (molde) | `class Escaner:` | `class Escaner { ... }` |
| Crear un objeto | `e = Escaner()` | `var e = new Escaner();` |
| Comentario | `# esto` | `// esto` |
| Fin de instrucción | nada (salto de línea) | `;` punto y coma |

## Ideas para seguir practicando

1. **Añade una opción 7 al menú** que pregunte un dominio y muestre solo su
   primera IP (usa `Dns.GetHostEntry`, como en `Red.ResolverDns`).
2. **Haz un escáner de puertos sencillo:** un bucle que pruebe los puertos
   20, 21, 22, 80 y 443 de un host con `Red.ProbarPuerto`.
3. **Escribe la salida en un archivo:** `File.WriteAllText("salida.txt", texto)`
   (necesita `using System.IO;`).

## Notas honestas

- Esto es un proyecto para aprender, no una herramienta de pentesting.
  No hace nada que InfoForge no haga mejor, y no toca nada fuera de tu
  propia máquina salvo las webs y hosts que tú le pidas.
- Compilado y probado con .NET 8 en Debian 13.
