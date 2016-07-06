using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace LogMagic.Seq
{
   //see for more: http://docs.getseq.net/docs/posting-raw-events
   class SeqWriter : ILogWriter
   {
      private readonly HttpClient _client;

      public SeqWriter(Uri serverAddress)
      {
         _client = new HttpClient();
         _client.BaseAddress = serverAddress;
         _client.DefaultRequestHeaders.Accept.Clear();
         _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      }

      public void Write(IEnumerable<LogEvent> events)
      {
         var data = new RawEvents
         {
            Events = events.Select(e => RawEvent.FromLogEvent(e)).ToArray()
         };

         string json = data.ToJsonString();
         _client.PostAsync("/api/events/raw", new StringContent(json, Encoding.UTF8, "application/json"));
      }

      public void Dispose()
      {
         _client.Dispose();
      }
   }
}
