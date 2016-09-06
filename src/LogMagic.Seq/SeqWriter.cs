using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic.Seq
{
   //see for more: http://docs.getseq.net/docs/posting-raw-events
   class SeqWriter : ILogWriter
   {
      private readonly HttpClient _client;
      private const string PostUrl = "/api/events/raw";
      private const string ContentType = "application/json";

      public SeqWriter(Uri serverAddress, string apiKey)
      {
         _client = new HttpClient();
         if (apiKey != null) _client.DefaultRequestHeaders.Add("X-Seq-ApiKey", apiKey);
         _client.BaseAddress = serverAddress;
         _client.DefaultRequestHeaders.Accept.Clear();
         _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));
      }

      public void Write(IEnumerable<LogEvent> events)
      {
         _client.PostAsync(PostUrl, new StringContent(ToJson(events), Encoding.UTF8, ContentType)).Wait();
      }

      public async Task WriteAsync(IEnumerable<LogEvent> events)
      {
         await _client.PostAsync(PostUrl, new StringContent(ToJson(events), Encoding.UTF8, ContentType));
      }

      private static string ToJson(IEnumerable<LogEvent> events)
      {
         var data = new RawEvents
         {
            Events = events.Select(e => RawEvent.FromLogEvent(e)).ToArray()
         };

         string json = data.ToJsonString();
         return json;
      }

      public void Dispose()
      {
         _client.Dispose();
      }
   }
}
