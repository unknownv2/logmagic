namespace LogMagic.Enrichers
{
   /// <summary>
   /// Contains a curated list of known property names
   /// </summary>
   public static class KnownProperty
   {
      /// <summary>
      /// Standard property name to store error information
      /// </summary>
      public const string Error = "error";

      /// <summary>
      /// Application name
      /// </summary>
      public const string ApplicationName = "appName";

      /// <summary>
      /// Application version
      /// </summary>
      public const string Version = "version";

      /// <summary>
      /// Node name
      /// </summary>
      public const string NodeName = "nodeName";

      /// <summary>
      /// Node IP address
      /// </summary>
      public const string NodeIp = "nodeIp";

      /// <summary>
      /// In multiinstance scenarios this property identifies the instance ID
      /// </summary>
      public const string NodeInstanceId = "nodeInstanceId";

      /// <summary>
      /// Method name the code is executing in
      /// </summary>
      public const string MethodName = "methodName";

      /// <summary>
      /// ID of a thread the code is executing from
      /// </summary>
      public const string ThreadId = "threadId";

   }
}
