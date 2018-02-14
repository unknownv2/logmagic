using Microsoft.ServiceFabric.Services.Remoting.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatefulSimulator.Remoting
{
   class UnsafeHeaders : IServiceRemotingRequestMessageHeader
   {
      private readonly IServiceRemotingRequestMessageHeader _inner;

      public UnsafeHeaders(IServiceRemotingRequestMessageHeader headers)
      {
         _inner = headers;
      }

      #region [ Redirection ]

      public int MethodId
      {
         get => _inner.MethodId;
         set => _inner.MethodId = value;
      }

      public int InterfaceId
      {
         get => _inner.InterfaceId;
         set => _inner.InterfaceId = value;
      }

      public string InvocationId
      {
         get => _inner.InvocationId;
         set => _inner.InvocationId = value;
      }

      public void AddHeader(string headerName, byte[] headerValue)
      {
         _inner.AddHeader(headerName, headerValue);
      }

      public bool TryGetHeaderValue(string headerName, out byte[] headerValue)
      {
         return _inner.TryGetHeaderValue(headerName, out headerValue);
      }

      #endregion

      public string[] HeaderNames
      {
         get
         {
            dynamic innerUnsafe = _inner;

            Dictionary<string, byte[]> unsafeDictionary = innerUnsafe.headers;

            return unsafeDictionary.Keys.ToArray();
         }
      }
   }
}
