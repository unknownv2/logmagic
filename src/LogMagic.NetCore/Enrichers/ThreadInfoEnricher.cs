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
      public void Enrich(LogEvent e, out string propertyName, out object propertyValue)
      {
         propertyName = "threadId";
         propertyValue = GetThreadId();
      }

      private static string GetThreadId()
      {
#if PORTABLE || NETCORE
         string threadId = Task.CurrentId.ToString();
#else
         string threadId = Thread.CurrentThread.ManagedThreadId.ToString();
#endif
         return threadId;
      }
   }
}
