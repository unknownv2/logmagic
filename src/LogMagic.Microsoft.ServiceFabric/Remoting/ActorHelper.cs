using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;

namespace LogMagic.Microsoft.ServiceFabric.Remoting
{
   internal static class ActorHelper
   {
      public static string GetDefaultListenerName<TActorInterface>(out ActorRemotingProviderAttribute providerAttribute)
         where TActorInterface : IActor
      {
         providerAttribute = GetProvider(new[] { typeof(TActorInterface) });

         if (Helper.IsEitherRemotingV2(providerAttribute.RemotingClientVersion))
         {
            if (Helper.IsRemotingV2_1(providerAttribute.RemotingClientVersion))
            {
               return "V2_1Listener";
            }
            return "V2Listener";
         }

         return null;
      }

      public static ActorRemotingProviderAttribute GetProvider(IEnumerable<Type> types = null)
      {
         if (types != null)
         {
            foreach (Type type in types)
            {
               ActorRemotingProviderAttribute customAttribute = type.GetTypeInfo().Assembly.GetCustomAttribute<ActorRemotingProviderAttribute>();
               if (customAttribute != null)
                  return customAttribute;
            }
         }
         Assembly entryAssembly = Assembly.GetEntryAssembly();
         if (entryAssembly != (Assembly)null)
         {
            ActorRemotingProviderAttribute customAttribute = entryAssembly.GetCustomAttribute<ActorRemotingProviderAttribute>();
            if (customAttribute != null)
               return customAttribute;
         }
         return (ActorRemotingProviderAttribute)new FabricTransportActorRemotingProviderAttribute();
      }


   }
}