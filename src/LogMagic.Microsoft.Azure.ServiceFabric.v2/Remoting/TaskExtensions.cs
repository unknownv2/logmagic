using System.Threading.Tasks;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   static class TaskExtensions
   {
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "task")]
      public static void Forget(this Task task)
      {
      }
   }
}
