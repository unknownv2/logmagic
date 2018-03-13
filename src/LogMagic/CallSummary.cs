using System;

namespace LogMagic
{
   /// <summary>
   /// Describes a summary of a call
   /// </summary>
   public class CallSummary
   {
      /// <summary>
      /// Creates a new instance of this class
      /// </summary>
      /// <param name="callName"></param>
      /// <param name="error"></param>
      /// <param name="durationTicks"></param>
      public CallSummary(string callName, Exception error, long durationTicks)
      {
         CallName = callName;
         Error = error;
         DurationTicks = durationTicks;
      }

      public string CallName { get; }

      public Exception Error { get; }

      public long DurationTicks { get; }
   }
}
