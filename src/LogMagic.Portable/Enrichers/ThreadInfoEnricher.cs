#if PORTABLE
using System.Threading.Tasks;
#else
using System.Threading;
using System.Threading.Tasks;
#endif


namespace LogMagic.Enrichers
{
   class ThreadInfoEnricher : IEnricher
   {
      public void Enrich(LogEvent e, out string propertyName, out string propertyValue)
      {
         propertyName = "ThreadId";
         propertyValue = GetThreadId();
      }

      private static string GetThreadId()
      {
#if PORTABLE
         string threadId = Task.CurrentId.ToString();
#else
         string threadId = Thread.CurrentThread.ManagedThreadId.ToString();
#endif
         return threadId;
      }
   }
}
