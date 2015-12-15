using NUnit.Framework;

namespace LogMagic.Test
{
   /// <summary>
   /// This demonstrates generic use case for logging
   /// </summary>
   [TestFixture, Ignore]
   public class LoggingDemoTest
   {
      private readonly ILog _log = L.G();

      public void Demo_GenericMethod_()
      {
         //some code

         _log.D("my log string");

         //some code
      }
   }
}
