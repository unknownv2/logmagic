namespace LogMagic.Tokenisation
{
   /// <summary>
   /// Token type in the parsed log string
   /// </summary>
   public enum TokenType
   {
      /// <summary>
      /// Normal string token
      /// </summary>
      String,

      /// <summary>
      /// Parameter, indexed or named
      /// </summary>
      Parameter,

      /// <summary>
      /// Event time
      /// </summary>
      Time
   }
}
