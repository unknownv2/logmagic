using System;
using System.Collections.Generic;
using System.Text;

namespace LogMagic
{
    public enum EventType
    {
        Trace,

        Dependency,

        ApplicationEvent,

        HandledRequest,

        Metric
    }
}
