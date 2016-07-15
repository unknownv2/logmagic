using LogMagic.Tokenisation;
using NUnit.Framework;

namespace LogMagic.Test
{
   [TestFixture]
   public class FormattedStringTest
   {
      [TestCase("it's just {0} egg", new object[] { "one" }, "it's just one egg")]
      [TestCase("wrong position {5}", new object[] { 5 }, "wrong position ")]
      [TestCase("plain {0} and {name}", new object[] { "param", "named" }, "plain param and named")]
      [TestCase("{0:D2}", new object[] { 5 }, "05")]
      [TestCase(null, null, "")]
      [TestCase("null params{0}", null, "null params")]
      [TestCase("named {format:D3}", new object[] { 1 }, "named 001")]
      public void Format_Variable_Variable(string format, object[] parameters, string output)
      {
         FormattedString fs = FormattedString.Parse(format, parameters);

         Assert.AreEqual(output, fs.ToString());
      }

      [Test]
      public void Format_NamedParameters_OnlyNamedCaptured()
      {
         FormattedString fs = FormattedString.Parse("named parameters are: {first} and {second}", new object[] { 1, 2 });

         Assert.AreEqual(2, fs.NamedParameters.Count);
         Assert.AreEqual(1, fs.NamedParameters["first"]);
         Assert.AreEqual(2, fs.NamedParameters["second"]);
      }
   }
}
