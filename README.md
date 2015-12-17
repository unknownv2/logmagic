# LogMagic

LogMagic in the easiest logging abstraction for .NET framework. It has the shortest syntax and the easiest exensibility model. It is extremely lightweight and has zero external dependencies.

Available as [NuGet Package](https://www.nuget.org/packages/LogMagic).

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

There are a few other most commonly used receivers available out of the box.

## Built-in receivers

### ConsoleLogReceiver

Outputs messages to system console and ideal for server logging. It doesn't do anything fancy unlike the next one. To add it to configuration simply write:

```csharp
L.AddReceiver(new ConsoleLogReceiver());
```

### PoshConsoleLogReceiver

Outputs messages to system console in a cheerful colorful way. You should use this if you need to look cool. Note that due to the fact it switches console colors frequently during logging, it's slower than **ConsoleLogReceiver** therefore you should use this only if your app is running in console or in debug mode:

```csharp
L.AddReceiver(new PoshConsoleLogReceiver());
```

### FileReceiver

## Unique Features

### Exception logging

### Visual Studio Snippets

### Dead Simple Initialisation

LogMagic is initialised in one line, there is no extra work to do. Configuration files and other bullcrap is not here.

### Windows Azure Integration

LogMagic supports Azure Append Blobs and Azure Tables through an additional [LogMagic.WindowsAzure](https://www.nuget.org/packages/LogMagic.WindowsAzure/) package.

#### Append Blobs

Append blobs are remote "files" in Windows Azure which can dynamically expand as new information comes in. LogMagic supports that via statement:

```csharp
L.AddReceiver(
  new AzureAppendBlobLogReceiver(
    "storageaccountname"
    "storagekey",
    "blobcontainername",
    "blobnameprefix");
```

All the parameters are self explanatory except for **blobnameprefix**. This parameter is responsible for naming blobs in the container. Azure blobs will be named as **blobnameprefix-yyyy-MM-dd.txt**.

#### Tables

Add table receiver using:

```csharp
L.AddReceiver(
  new AzureTableLogReceiver(
    "storageaccountname"
    "storagekey",
    "tablename"));
```

**tablename** specifies the destination log table name for logging. Each log statement creates a new log record where:

- **partition key** is yy-MM-dd
- **row key** is HH-mm-ss-fff
- **extra columns** such as NodeId, Severity, SourceName, ThreadName, Message, Error.