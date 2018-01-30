### System trace

This provider is built into the core library.

**Syntax:**
```csharp
L.Config.WriteTo.Trace();
```
Writes events to `System.Diagnostics.Trace`. Note that it's only supported in `.NET 4.5` because at the moment `.NET Standard` doesn't have trace logging support.

`.Trace(string format)` overload allows custom formatting.