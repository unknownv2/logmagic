### Seq

This provider is built into the core library.

**Syntax:**
```csharp
L.Config.WriteTo.Seq("server address");
```
Writes events to the remote [Seq](https://getseq.net/) server. Seq is a great log collection and analysis framework.

`.Seq(string address, string apiKey)` overload allows passing an API key.