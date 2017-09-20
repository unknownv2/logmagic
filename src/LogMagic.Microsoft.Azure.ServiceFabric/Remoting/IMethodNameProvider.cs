namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   interface IMethodNameProvider
   {
      string GetMethodName(int interfaceId, int methodId);
   }
}
