# File on disk

This provider is built into the core library.

**Syntax:**
```csharp
L.Config.WriteTo.File("c:\\tmp\\my.log");
```

Writes logs to a file on disk specified in the first parameter. File is rolled every day and is always named as `name-YYYY-mm-dd.extension`. For example for the line above on December 19, 2016 the first file created immediately in `c:\tmp` folder will be `my-2016-12-19.log`, next day `my-2016-12-20.log` and so on. Time calculation is always performed in UTC and never local time.

Following overload allows custom formatting:
```csharp
L.Config.WriterTo.File(string name, string format)
``` 


Note that this writer doesn't support rollovers, size limitations etc. provided by older frameworks. Mostly because file logging is not relevant nowadays and I didn't want to spend time implementing this.