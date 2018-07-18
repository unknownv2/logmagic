using System;
using System.Collections.Generic;
using System.Text;
using LogMagic.Enrichers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace LogMagic.Microsoft.Azure.ApplicationInsights.Writers
{
   class OperationTelemetryInitialiser : ITelemetryInitializer
   {
      public void Initialize(ITelemetry telemetry)
      {
#if !NET45
         string operationId = L.GetContextValue(KnownProperty.OperationId);
         telemetry.Context.Operation.Id = operationId;
#endif
      }
   }
}
