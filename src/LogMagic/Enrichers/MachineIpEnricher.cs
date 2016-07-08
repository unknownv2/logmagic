using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace LogMagic.Enrichers
{
   class MachineIpEnricher : IEnricher
   {
      private readonly string _address;

      public MachineIpEnricher(bool useIpv6 = false)
      {
         string hostName = Dns.GetHostName();
         IPAddress[] addresses = Dns.GetHostAddresses(hostName);

         IPAddress address = useIpv6
            ? addresses.LastOrDefault(a => a.AddressFamily == AddressFamily.InterNetworkV6)
            : addresses.LastOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);

         _address = address?.ToString();
      }

      public void Enrich(LogEvent e, out string propertyName, out string propertyValue)
      {
         propertyName = "machineIp";
         propertyValue = _address;
      }
   }
}
