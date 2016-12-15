using System;
using System.Collections;
using System.Text;

namespace LogMagic.TypeFormatters
{
   /// <summary>
   /// Attempts to format IEnumerable
   /// </summary>
   class EnumerableFormatter : ITypeFormatter
   {
      private const int MaxElements = 5;

      public Type Type => typeof(IEnumerable);

      public bool SupportsDerivedTypes => true;

      public string Format(object value)
      {
         bool isCollection = value is ICollection;
         if(!isCollection) return value.ToString();

         //attempt to get the element count
         long count = ((ICollection) value).Count;

         if (count == 0) return "[empty]";

         var sb = new StringBuilder();
         sb.Append("[");
         sb.Append(count);
         sb.Append(" el: ");

         int added = 0;
         foreach (object el in (IEnumerable) value)
         {
            if (added != 0) sb.Append(", ");
            sb.Append("{");
            string elv = FormatterEntry.FormatParameter(el).ToString();
            sb.Append(elv);
            sb.Append("}");
            if(++added >= MaxElements) break;
         }

         if (added < count)
         {
            sb.Append(" +");
            sb.Append(count - added);
            sb.Append(" more");
         }

         sb.Append("]");

         return sb.ToString();
      }
   }
}
