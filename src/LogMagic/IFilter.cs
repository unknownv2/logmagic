using System;
using System.Collections.Generic;
using System.Text;

namespace LogMagic
{
   /// <summary>
   /// Logging filter
   /// </summary>
   public interface IFilter
   {
      /// <summary>
      /// Return TRUE to match this log event
      /// </summary>
      /// <param name="e"></param>
      /// <returns></returns>
      bool Match(LogEvent e);
   }
}
