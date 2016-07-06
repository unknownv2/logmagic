using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LogMagic.Test
{
   [TestFixture]
   public class FormattingTest
   {
      private TestWriter _receiver;
      private ILog _log;

      [SetUp]
      public void SetUp()
      {
         _receiver = new TestWriter();
         L.ClearReceivers();
         L.AddReceiver(_receiver);
         _log = L.G<FormattingTest>();
      }

      private string Message => _receiver.Message;

      [Test]
      public void Mixed_IntegerAndString_Formats()
      {
         _log.D("one {0} string {1}", 1, "s");

         Assert.AreEqual("one 1 string s", Message);
      }

      [Test]
      public void IEnumerable_SmallList_Formats()
      {
         var lst = new List<string> {"one", "two", "three" };

         _log.D("the {0} format", lst);

         Assert.AreEqual("the [3 el: {one}, {two}, {three}] format", Message);
      }

      [Test]
      public void IEnumerable_LargeList_Formats()
      {
         var lst = new List<string> { "one", "two", "three", "four", "five", "six", "seven" };

         _log.D("the {0} format", lst);

         Assert.AreEqual("the [7 el: {one}, {two}, {three}, {four}, {five} +2 more] format", Message);
      }


      [Test]
      public void String_NoTransform_Formats()
      {
         _log.D("the {0}", "string");

         Assert.AreEqual("the string", Message);
      }

      private class TestWriter : ILogWriter
      {
         public string Message { get; private set; }

         public void Dispose()
         {
         }

         public void Write(IEnumerable<LogEvent> events)
         {
            foreach (LogEvent e in events)
            {
               Message = e.Message;
            }
         }
      }
   }
}
