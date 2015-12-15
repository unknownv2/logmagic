# LogMagic

LogMagic in the easiest logging abstraction for .NET framework. It has the shortest syntax and the easiest exensibility model. It is extremely lightweight and has zero external dependencies.

Available as NuGet package - search "LogMagic" in nuget.

## Syntax comparison

Some examples on syntax comparison between a few popular logging frameworks.

### LogMagic

```csharp
private readonly ILog log = L.G();
log.D("my line");
```

### Log4Net

```csharp
ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
log.Debug("my line");
```

### NLog

```csharp
private static Logger log = LogManager.GetCurrentClassLogger();
log.Debug("my line");
```

## How To Use

First, reference LogMagic package from NuGet.

Second, add the logging variable into your class:

```csharp
public class LoggingDemoTest
{
  private readonly ILog _log = L.G();

  //...
}
```

You are ready to go, start logging!

```csharp
public class LoggingDemoTest
{
  private readonly ILog _log = L.G();

  public void SomeMethod()
  {
    //some code

    _log.D("my log string");

    //some code
  }
}
```

### Where does it log?

Generally you should not care. By default it does not log anywhere, however you can add log receivers on application initialisation, for example this line configures LogMagic to save everything to a text file:

```csharp
L.AddReceiver(new FileReceiver("c:\\myapp.log"));
```

## Unique Features

### Exception logging

### Visual Studio Snippets

### Dead Simple Initialisation

### Windows Azure Integration


