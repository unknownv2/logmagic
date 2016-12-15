using LogMagic.Tokenisation;
using Xunit;

namespace LogMagic.Test
{
   public class FormattedStringTest
   {
      [Theory]
      [InlineData("it's just {0} egg", new object[] { "one" }, "it's just 'one' egg")]
      [InlineData("wrong position {5}", new object[] { 5 }, "wrong position ")]
      [InlineData("plain {0} and {name}", new object[] { "param", "named" }, "plain 'param' and 'named'")]
      [InlineData("{0:D2}", new object[] { 5 }, "05")]
      [InlineData(null, null, "")]
      [InlineData("null params{0}", null, "null params")]
      [InlineData("named {format:D3}", new object[] { 1 }, "named 001")]
      [InlineData("quote {0}", new object[] { "'trim" }, "quote ''trim'")]
      public void Format_Variable_Variable(string format, object[] parameters, string output)
      {
         FormattedString fs = FormattedString.Parse(format, parameters);

         Assert.Equal(output, fs.ToString());
      }

      [Fact]
      public void Format_NamedParameters_OnlyNamedCaptured()
      {
         FormattedString fs = FormattedString.Parse("named parameters are: {first} and {second}", new object[] { 1, 2 });

         Assert.Equal(2, fs.NamedParameters.Count);
         Assert.Equal(1, fs.NamedParameters["first"]);
         Assert.Equal(2, fs.NamedParameters["second"]);
      }
   }
}
