using System;
using System.Collections.Generic;
using System.Text;

namespace LogMagic
{
   public interface IFilter
   {
      bool Match(LogEvent e);
   }
}
