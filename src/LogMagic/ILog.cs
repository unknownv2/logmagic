using LogMagic.Trackers;

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
      /// <param name="command">Command issued to the dependency</param>
      /// <returns>Dependency tracker. Needs to be disposed to stop tracking.</returns>
      IDependencyTracker TrackDependency(string name, string command);

      /// <summary>
      /// Tracks application event
      /// </summary>
      /// <param name="name">Event name</param>
      void TrackEvent(string name);

      IRequestTracker TrackRequest(string name);
   }
}
