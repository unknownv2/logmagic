using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogMagic.Measures;
using Xunit;

namespace LogMagic.Test
{
   public class MeasuresTest
   {
      [Fact]
      public void Exceptions()
      {
         using (new DependencyMeasure())
         {
            throw new Exception("help me!");
         }
      }
   }
}