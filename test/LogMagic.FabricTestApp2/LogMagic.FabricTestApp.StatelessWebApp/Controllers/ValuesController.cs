using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogMagic.FabricTestApp.Interfaces;
using LogMagic.Microsoft.Azure.ServiceFabric.Remoting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;

namespace LogMagic.FabricTestApp.StatelessWebApp.Controllers
{
   [Route("api/[controller]")]
   public class ValuesController : Controller
   {
      // GET api/values
      [HttpGet]
      public async Task<IEnumerable<string>> Get()
      {
         IRootService rootService = CorrelatingProxyFactory.CreateServiceProxy<IRootService>(
            new Uri("fabric:/LogMagic.FabricTestApp2/StatefulSimulator"),
            new ServicePartitionKey(0),
            remoteServiceName: "root");

         try
         {
            await rootService.TestCall();
         }
         catch(Exception ex)
         {
            ex = null;
         }

         return new string[] { "value1", "value2" };
      }

      // GET api/values/5
      [HttpGet("{id}")]
      public string Get(int id)
      {
         return "value";
      }

      // POST api/values
      [HttpPost]
      public void Post([FromBody]string value)
      {
      }

      // PUT api/values/5
      [HttpPut("{id}")]
      public void Put(int id, [FromBody]string value)
      {
      }

      // DELETE api/values/5
      [HttpDelete("{id}")]
      public void Delete(int id)
      {
      }
   }
}