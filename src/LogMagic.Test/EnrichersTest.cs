using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LogMagic.Test
{
   [TestFixture]
   public class EnrichersTest
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
            .WriteTo.Custom(_writer);
         _log = L.G<FormattingTest>();
      }

      [SetUp]
      public void TearDown()
      {
         L.Shutdown();
      }

      [Test]
      public void MethodName_ThisMethod_Matches()
      {
         L.Config.EnrichWith.MethodName();
         _log.D("method call");

         L.Flush();
         Assert.AreEqual("LogMagic.Test.EnrichersTest.MethodName_ThisMethod_Matches()", _writer.Event.GetProperty("method"));
      }

      [Test]
      public void MethodName_MethodWithParameters_ParametersFormatted()
      {
         L.Config.EnrichWith.MethodName();
         MethodWithParameters(null, 1, Guid.NewGuid());

         L.Flush();
         Assert.AreEqual("LogMagic.Test.EnrichersTest.MethodWithParameters(String s, Int32 i, Guid g)",
            _writer.Event.GetProperty("method"));
      }

      private string MethodWithParameters(string s, int i, Guid g)
      {
         _log.D("parameters method");
         return s;
      }
   }
}
