using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogMagic.Tokenisation;
using Xunit;

namespace LogMagic.Test
{
   public class FormattedStringBuilderTest
   {
      //[Fact] not ready yet
      public void Create_default_format_with_builder()
      {
         string actual = new FormattedStringBuilder()
            .AddTime("H:mm:ss")
            .AddText("|")
            .AddLevel(-7)
            .AddText("|")
            .Build();

         Assert.Equal("{time:H:mm:ss,fff}|{level,-7}|{source}|{message}{error}", actual);
      }
   }
}
