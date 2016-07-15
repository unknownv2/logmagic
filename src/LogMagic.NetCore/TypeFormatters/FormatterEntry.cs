using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic.TypeFormatters
{
   static class FormatterEntry
   {
      private static readonly List<ITypeFormatter> AllTransformers = new List<ITypeFormatter>
      {
         new EnumerableFormatter()
      };

      //used to cache ALL type mappings passed to logging, for increased performance
      private static readonly Dictionary<Type, ITypeFormatter> TypeToTransformer = new Dictionary<Type, ITypeFormatter>();

      public static object FormatParameter(object parameter)
      {
         if(parameter == null) return "NULL";

         /*
         Type t = parameter.GetType();
         ITypeFormatter formatter;
         if(!TypeToTransformer.TryGetValue(t, out formatter))
         {
            //try to find the EXACT type match
            formatter = AllTransformers.FirstOrDefault(tr => tr.Type == t);

            //try to find by matching by the base class
            if(formatter == null)
            {
               formatter = AllTransformers.FirstOrDefault(tr => tr.SupportsDerivedTypes && tr.Type.GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()));
            }

            if(formatter == null)
            {
               TypeToTransformer[t] = null;  //set as null so type discovery doesn't run
            }
         }

         return formatter == null ? null : formatter.Format(parameter);
         */

         return parameter;
      }
   }
}
