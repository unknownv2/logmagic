using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ServiceFabric.Services.Remoting;

namespace LogMagic.Microsoft.ServiceFabric.Remoting
{
   internal class Helper
   {
      public static bool IsEitherRemotingV2(RemotingClientVersion remotingClient)
      {
         if (!Helper.IsRemotingV2(remotingClient))
            return Helper.IsRemotingV2_1(remotingClient);
         return true;
      }

      public static bool IsEitherRemotingV2(RemotingListenerVersion remotingListener)
      {
         if (!Helper.IsRemotingV2(remotingListener))
            return Helper.IsRemotingV2_1(remotingListener);
         return true;
      }

      public static bool IsRemotingV2(RemotingClientVersion remotingClient)
      {
         return remotingClient.HasFlag((Enum)RemotingClientVersion.V2);
      }

      public static bool IsRemotingV2(RemotingListenerVersion remotingListener)
      {
         return remotingListener.HasFlag((Enum)RemotingListenerVersion.V2);
      }

      public static bool IsRemotingV2_1(RemotingListenerVersion remotingListener)
      {
         return remotingListener.HasFlag((Enum)RemotingListenerVersion.V2_1);
      }

      public static bool IsRemotingV2_1(RemotingClientVersion remotingClient)
      {
         return remotingClient.HasFlag((Enum)RemotingClientVersion.V2_1);
      }
   }
}
