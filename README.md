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

Outputs messages to a file on local filesystem. Accepts full file path to the target file. This receiver opens or creates the file in append mode and sets sharing mode to read & write. It also creates the directory on file system if it doesn't exist.

```csharp
L.AddReceiver(new FileReceiver("c:\\logs\\myapp.log")));
```

## Unique Features

### Exception logging

There is no special overload method to log exceptions. Passing any exception object is enough. Note that you shouldn't reserve any format parameter in the log string, i.e. this just works:

```csharp
_log.E("something bad happened", ex);
_log.E("failed on {0}", 1, ex);
```

### Visual Studio Snippets

There are visual studio snippets available in the git repository for log variable declaration and all the logging methods. You can import them into VS with built-in Import Snippet dialog.

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

## Extending

Extending LogMagic (adding your own implementation of a **receiver**) is extremely easy. All you need to do is implement the ILogReceiver interface:

```csharp
/// <summary>
/// Common interface for a log receiver
/// </summary>
public interface ILogReceiver : IDisposable
{
   /// <summary>
   /// Sends a chunk of log data to the receiver
   /// </summary>
   void Send(LogChunk chunk);
}
```

There is just **one** method to implement, unlike the overcomplicated slow corporate frameworks!

### Blocking Operations

Note that all the calls to ILog are **synchronous** and blocking. This is by design. Heavy and slow receivers, such as Azure Blob Storage or even FileReceiver aren't blocking because they derive from an abstract `AsyncReceiver` which sends messages in the background.

In order to implement an **asynchronous** receiver you can just derive from `AsyncReceiver` and implement one method:

```csharp
      /// <summary>
      /// Sends accumulated chunks to the destination
      /// </summary>
      /// <param name="chunks">Chunks accumulated</param>
      protected abstract void SendChunks(IEnumerable<LogChunk> chunks);
```

`AsyncReceiver` groups log chunks sent too fast so you can utilise batching approach.