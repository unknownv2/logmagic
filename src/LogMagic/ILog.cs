using System;
using System.Collections.Generic;

namespace LogMagic
{
   /// <summary>
   /// Logging interface used by the client code, the most high level
   /// </summary>
   public interface ILog
   {
      /// <summary>
      /// Send debug statement
      /// </summary>
      void D(string format, params object[] parameters);

      /// <summary>
      /// Send error statement
      /// </summary>
      void E(string format, params object[] parameters);

      /// <summary>
      /// Send general information statement
      /// </summary>
      void I(string format, params object[] parameters);

      /// <summary>
      /// Send warning statement
      /// </summary>
      void W(string format, params object[] parameters);

      /// <summary>
      /// Start tracking dependency.
      /// </summary>
      /// <param name="name">Dependency name</param>
      /// <param name="type"></param>
      /// <param name="command">Command issued to the dependency</param>
      /// <param name="duration"></param>
      /// <param name="error"></param>
      /// <returns>Dependency tracker. Needs to be disposed to stop tracking.</returns>
      void Dependency(string type, string name, string command, long duration, Exception error = null);

      /// <summary>
      /// Tracks application event
      /// </summary>
      /// <param name="name">Event name</param>
      /// <param name="properties">Extra properties for this event</param>
      void Event(string name, Dictionary<string, object> properties = null);

      /// <summary>
      /// Track incoming application request
      /// </summary>
      /// <param name="name"></param>
      /// <param name="duration"></param>
      /// <param name="error"></param>
      void Request(string name, long duration, Exception error = null);

      /// <summary>
      /// Measures metric of a specified name
      /// </summary>
      /// <param name="name">Metric name</param>
      /// <param name="value">Value of this metric</param>
      void Metric(string name, double value);
   }
}
