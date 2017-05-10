using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LogMagic.Test
{
   public class EnrichersTest : IDisposable
   {
      private TestWriter _writer;
      private ILog _log;

      public EnrichersTest()
      {
         _writer = new TestWriter();
         L.Config.ClearWriters();
         L.Config
            .WriteTo.Trace()
            .WriteTo.Custom(_writer);
         _log = L.G<FormattingTest>();
      }

      public void Dispose()
      {
         L.Shutdown();
      }

      [Fact]
      public void MethodName_ThisMethod_Matches()
      {
         L.Config.EnrichWith.MethodName();
         _log.D("method call");

         Assert.Equal("LogMagic.Test.EnrichersTest.MethodName_ThisMethod_Matches()", 
            (string)_writer.Event.GetProperty("method"));
      }

      [Fact]
      public void MethodName_MethodWithParameters_ParametersFormatted()
      {
         L.Config.EnrichWith.MethodName();
         MethodWithParameters(null, 1, Guid.NewGuid());

         Assert.Equal("LogMagic.Test.EnrichersTest.MethodWithParameters(String s, Int32 i, Guid g)",
            (string)_writer.Event.GetProperty("method"));
      }

      private string MethodWithParameters(string s, int i, Guid g)
      {
         _log.D("parameters method");
         return s;
      }

      [Fact]
      public void MachineIp_Current_ReturnsSomething()
      {
         L.Config.EnrichWith.MachineIpAddress();
         _log.D("what's this machine IP?");

         string address = (string)_writer.Event.GetProperty("machineIp");
         _log.D("address: {ipAddress}", address);
         Assert.NotNull(address);
      }
   }
}
