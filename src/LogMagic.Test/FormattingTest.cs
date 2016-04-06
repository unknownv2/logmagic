using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LogMagic.Test
{
   [TestFixture]
   public class FormattingTest
   {
      private TestReceiver _receiver;
      private ILog _log;

      [SetUp]
      public void SetUp()
      {
         _receiver = new TestReceiver();
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
      public void IEnumerable_CanGetCount_Formats()
      {
         var lst = new List<string> {"one", "two", "three" };

         _log.D("the {0} format", lst);

         Assert.AreEqual("the (3 elements) format", Message);
      }

      private class TestReceiver : ILogReceiver
      {
         public string Message { get; private set; }

         public void Dispose()
         {
         }

         public void Send(LogChunk chunk)
         {
            Message = chunk.Message;
         }
      }
   }
}
