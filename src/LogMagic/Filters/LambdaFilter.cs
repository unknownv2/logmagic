using System;
using System.Collections.Generic;
using System.Text;

namespace LogMagic.Filters
{
   class LambdaFilter : IFilter
   {
      private readonly Func<LogEvent, bool> _lambda;

      public LambdaFilter(Func<LogEvent, bool> lambda)
      {
         _lambda = lambda;
      }

      public bool Match(LogEvent e)
      {
         return _lambda(e);
      }
   }
}
