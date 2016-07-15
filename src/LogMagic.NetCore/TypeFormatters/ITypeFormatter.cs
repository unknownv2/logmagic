using System;

namespace LogMagic.TypeFormatters
{
   interface ITypeFormatter
   {
      Type Type { get; }

      bool SupportsDerivedTypes { get; }

      string Format(object value);
   }
}
