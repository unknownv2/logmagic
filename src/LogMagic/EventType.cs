using System;
using System.Collections.Generic;
using System.Text;

namespace LogMagic
{
   /// <summary>
   /// Type of logging event
   /// </summary>
   public enum EventType
   {
      /// <summary>
      /// The trace event, usual development logging
      /// </summary>
      Trace,

      /// <summary>
      /// The dependency execution event
      /// </summary>
      Dependency,

      /// <summary>
      /// The application event
      /// </summary>
      ApplicationEvent,

      /// <summary>
      /// The incoming request event
      /// </summary>
      HandledRequest,

      /// <summary>
      /// The metric event
      /// </summary>
      Metric
   }
}