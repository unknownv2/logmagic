using System.Threading;

namespace LogMagic.Enrichers
{
   class ThreadInfoEnricher : IEnricher
   {
      public void Enrich(LogEvent e, out string propertyName, out object propertyValue)
      {
         propertyName = "threadId";
         propertyValue = GetThreadId();
      }

      private static string GetThreadId()
      {
         return Thread.CurrentThread.ManagedThreadId.ToString();
      }
   }
}
