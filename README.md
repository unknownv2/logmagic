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

## Unique Features

### Exception logging

### Visual Studio Snippets

### Dead Simple Initialisation

### Windows Azure Integration


