# LogMagic

![logmagic icon](https://alonestore.blob.core.windows.net/nuget/logmagic.png)

![](https://aloneguid.visualstudio.com/DefaultCollection/_apis/public/build/definitions/323c5f4c-c814-452d-9eaf-1006c83fd44c/3/badge)

## Why LogMagic

Like many other libraries for .NET, LogMagic provides diagnostic logging into files, the console, and *elsewhere*. It's probably the easiest framework to setup, has a clean API, extremely extensible.

LogMagic also supports a relatively new paradigm of *structured logging*.

## Installation

### Installing from NuGet

The core logging package is [LogMagic](https://www.nuget.org/packages/LogMagic). Supported frameworks are:

* **.NET 4.5**
* **PCL** (Portable Library)
* **.NET Core** support is coming soon

```
PM> Install-Package LogMagic
```

### Setup

Types are in `LogMagic` namespace

```csharp
using LogMagic;
```

An `ILog` instance is the one used to log events and can be created in one of three ways by calling to global static `L` class.

Create using current class type name

```csharp
ILog _log = L.G();
```

Or by specifying type explicitly

```csharp
ILog _log = L.G<T>;			//using generics
ILog _log = L.G(typeof(T));	//passing type
```

Or by specifying name explicitly

```csharp
ILog _log = L.G("instance name");
```

By default LogMagic doesn't write events anywhere and you need to configure it:

```csharp
L.Config.WriteTo.Console();
```

This is typically done once at application startup.

### Example application

The complete example below shows logging in a simple console application, with events sent to the console as well as to file on disk.

1. **Create a new Console Application project**
2. **Install the core LogMagic package**

In Visual Studio, open Package Manager Console and type:

```powershell
Install-Package LogMagic
```

3. **Add the following code to `Program.cs`**

```csharp
using System;
using LogMagic;

namespace LogMagicExample
{
   public class Program
   {
      private readonly ILog _log = L.G();

      public static void Main(string[] args)
      {
         L.Config
            .WriteTo.Console()
            .EnrichWith.ThreadId();

         new Program().Run();

         Console.ReadLine();

         L.Shutdown();
      }

      private void Run()
      {
         _log.I("hello, LogMagic!");

         int a = 10, b = 0;

         try
         {
            _log.D("dividing {a} by {b}", a, b);
            Console.WriteLine(a / b);
         }
         catch(Exception ex)
         {
            _log.E("unexpected error", ex);
         }

         _log.D("attempting to divide by zero");
      }

   }
}
```

4. **Run the program**

## Visual Studio Snippets

Visual Studio snippets are installed along with the NuGet package so you can use them straight away with any project:

`ldef` defines a logging variable inside the class:
```csharp
private readonly ILog _log = L.G();
```

`ld` writes a logging statement:
```csharp
_log.D("...");
```
there are identical shortcuts for other logging levels `le`, `li`, `lw`.

## Configuration basics

todo
