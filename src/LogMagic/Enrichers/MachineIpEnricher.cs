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
            ? addresses.Where(a => !IPAddress.IsLoopback(a) && a.AddressFamily == AddressFamily.InterNetworkV6).LastOrDefault()
            : addresses.Where(a => !IPAddress.IsLoopback(a) && a.AddressFamily == AddressFamily.InterNetwork).LastOrDefault();

         _address = address?.ToString();
      }

      public void Enrich(LogEvent e, out string propertyName, out object propertyValue)
      {
         propertyName = "machineIp";
         propertyValue = _address;
      }
   }
}
