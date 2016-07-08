using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LogMagic.Test
{
   [TestFixture]
   public class FormattingTest
   {
      private TestWriter _writer;
      private ILog _log;

      [SetUp]
      public void SetUp()
      {
         _writer = new TestWriter();
         L.Config.ClearWriters();
         L.Config
            .WriteTo.Trace()
            .WriteTo.Custom(_writer)
            .EnrichWith.ThreadId()
            .EnrichWith.Constant("node", "test");
         _log = L.G<FormattingTest>();
      }

      [SetUp]
      public void TearDown()
      {
         L.Shutdown();
      }

      private string Message => _writer.Message;
      private LogEvent Event => _writer.Event;

      [Test]
      public void Mixed_IntegerAndString_Formats()
      {
         _log.D("one {0} string {1}", 1, "s");
         L.Flush();

         Assert.AreEqual("one 1 string s", Message);
      }

      [Test]
      [Ignore("list formatting is unstable")]
      public void IEnumerable_SmallList_Formats()
      {
         var lst = new List<string> {"one", "two", "three" };

         _log.D("the {0} format", lst);

         L.Flush();
         Assert.AreEqual("the [3 el: {one}, {two}, {three}] format", Message);
      }

      [Test]
      [Ignore("list formatting is unstable")]
      public void IEnumerable_LargeList_Formats()
      {
         var lst = new List<string> { "one", "two", "three", "four", "five", "six", "seven" };

         _log.D("the {0} format", lst);

         L.Flush();
         Assert.AreEqual("the [7 el: {one}, {two}, {three}, {four}, {five} +2 more] format", Message);
      }


      [Test]
      public void String_NoTransform_Formats()
      {
         _log.D("the {0}", "string");

         L.Flush();
         Assert.AreEqual("the string", Message);
      }

      [Test]
      public void Structured_NamedString_Formats()
      {
         _log.D("the {Count} kettles", 5);

         L.Flush();
         Assert.AreEqual("the 5 kettles", Message);
         Assert.AreEqual(5, (int)Event.GetProperty("Count"));
      }

      [Test]
      public void Structured_Mixed_Formats()
      {
         _log.D("{0} kettles and {three:D2} lamps{2}", 5, 3, "...");

         L.Flush();
         Assert.AreEqual("5 kettles and 03 lamps...", Message);
         Assert.AreEqual(3, Event.GetProperty("three"));
      }

      
   }
}
