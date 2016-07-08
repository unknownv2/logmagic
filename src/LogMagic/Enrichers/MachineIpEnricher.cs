using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace LogMagic.Enrichers
{
   class MachineIpEnricher : IEnricher
   {
      private readonly string _address;

      public MachineIpEnricher(bool includeIpV6)
      {
         string hostName = Dns.GetHostName();
         IPAddress[] addresses = Dns.GetHostAddresses(hostName);

         _address = string.Join(", ", addresses.Where(a => 
            !IPAddress.IsLoopback(a) &&
            (a.AddressFamily == AddressFamily.InterNetwork || (a.AddressFamily == AddressFamily.InterNetworkV6 && includeIpV6))));
      }

      public void Enrich(LogEvent e, out string propertyName, out object propertyValue)
      {
         propertyName = "machineIp";
         propertyValue = _address;
      }
   }
}
