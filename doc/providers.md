# List of known Writers and Enrichers

## Writers

### System Console

**Package**: built-in

**Supported Frameworks**: `.NET 4.5`, `.NET Standard 1.6`

**Syntax:**
```csharp
L.Config.WriteTo.Console();
```

Most basic write producing console output. By default it looks like this:

![Writers Console Default](img/writers-console-default.png)

`.Console(string format)` overload allows custom formatting.

### Posh Console

**Package**: built-in

**Supported Frameworks**: `.NET 4.5`, `.NET Standard 1.6`

**Syntax:**
```csharp
L.Config.WriteTo.PoshConsole();
```

Colorful console writer. By default it looks like this:

![Writers Console Default](img/writers-poshconsole-default.png)

`.PoshConsole(string format)` overload allows custom formatting.

### File on disk

... in progress

core|`.FiileLog()`| file on disk |
core|`.PoshConsole()`| graphical console with coloring |
core|`.Trace()`| system trace diagnostics|
[LogMagic.WindowsAzure](https://www.nuget.org/packages/LogMagic.WindowsAzure/) | `.AzureAppendBlob()` | appends to Microsoft Azure blob storage append blob and rotates on daily basis |
| [LogMagic.WindowsAzure](https://www.nuget.org/packages/LogMagic.WindowsAzure/) | `.AzureTable()` | appends to Microsoft Azure table storage |
| [LogMagic.Seq](https://www.nuget.org/packages/LogMagic.Seq/) | `.Seq()` | writes events to [Seq](https://getseq.net/)

### Enrichers

| Package     | Writer Syntax | Meaning        |
|-------------|---------------|----------------|
|  core       |  `.Constant()` | adds a constant value to every log event |
| core | `.MachineIp()` | current machine IP address |
| core | `.MachineName()` | current machine DNS name |
| core | `.MethodName()` | caller's method name |
| core | `.ThreadId()` | managed thread id |
