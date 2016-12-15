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
   }
}
