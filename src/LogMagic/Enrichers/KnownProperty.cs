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

      /// <summary>
      /// Used to track an operation across multiple layers
      /// </summary>
      public const string OperationId = "operationId";

      /// <summary>
      /// Used in conjunction with <see cref="OperationId"/> to track the parent operation ID
      /// </summary>
      public const string OperationParentId = "operationParentId";

      /// <summary>
      /// Operation duration
      /// </summary>
      public const string Duration = "duration";

      /// <summary>
      /// Event name
      /// </summary>
      public const string EventName = "eventName";

      /// <summary>
      /// Request name
      /// </summary>
      public const string RequestName = "requestName";

      /// <summary>
      /// Metric name
      /// </summary>
      public const string MetricName = "metricName";

      /// <summary>
      /// Dependency name
      /// </summary>
      public const string DependencyName = "dependencyName";

      /// <summary>
      /// Dependency type
      /// </summary>
      public const string DependencyType = "dependencyType";

      /// <summary>
      /// Dependency command
      /// </summary>
      public const string DependencyCommand = "dependencyCommand";

      /// <summary>
      /// Metric value
      /// </summary>
      public const string MetricValue = "metricValue";
   }
}
