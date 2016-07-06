using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic
{
   public interface IEnricher
   {
      void Enrich(LogEvent e, out string propertyName, out string propertyValue);
   }
}
