using System;
using System.Collections.Generic;
using Xunit;

namespace LogMagic.Test
{
   public class FormattingTest
   {
      private TestWriter _writer;
      private ILog _log = L.G();

      public FormattingTest()
      {
         _writer = new TestWriter();
         L.Config.ClearWriters();
         L.Config
            .WriteTo.Trace()
            .WriteTo.Custom(_writer)
            .EnrichWith.ThreadId()
            .EnrichWith.Constant("node", "test");
      }

      private string Message => _writer.Message;
      private LogEvent Event => _writer.Event;

      [Fact]
      public void Mixed_IntegerAndString_Formats()
      {
         _log.D("one {0} string {1}", 1, "s");

         Assert.Equal("one 1 string 's'", Message);
      }

      [Fact]
      public void String_NoTransform_Formats()
      {
         _log.D("the {0}", "string");

         Assert.Equal("the 'string'", Message);
      }

      [Fact]
      public void SourceName_Reflected_ThisClass()
      {
         _log.D("testing source");

         Assert.Equal("LogMagic.Test.FormattingTest", Event.SourceName);
      }

      [Fact]
      public void Structured_NamedString_Formats()
      {
         _log.D("the {Count} kettles", 5);

         Assert.Equal("the 5 kettles", Message);
         Assert.Equal(5, (int)Event.GetProperty("Count"));
      }

      [Fact]
      public void Structured_Mixed_Formats()
      {
         _log.D("{0} kettles and {three:D2} lamps{2}", 5, 3, "...");

         Assert.Equal("5 kettles and 03 lamps'...'", Message);
         Assert.Equal(3, Event.GetProperty("three"));
      }

      
   }
}
