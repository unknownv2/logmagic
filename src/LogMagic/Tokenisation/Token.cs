namespace LogMagic.Tokenisation
{
   /// <summary>
   /// Token in a log string
   /// </summary>
   public struct Token
   {
      /// <summary>
      /// Name, only for named parameters
      /// </summary>
      public string Name;

      /// <summary>
      /// Relative position of the value in the arguments array
      /// </summary>
      public int Position;

      /// <summary>
      /// If a native formatting is used, contains the format string
      /// </summary>
      public string NativeFormat;

      /// <summary>
      /// If a native formatting is used, contains the format string without position parameter
      /// </summary>
      public string Format;

      /// <summary>
      /// Type of the token
      /// </summary>
      public TokenType Type;

      /// <summary>
      /// Parameter value
      /// </summary>
      public string Value;

      /// <summary>
      /// Create a new instance of the token
      /// </summary>
      public Token(TokenType type, string value, string name, int position, string nativeFormat, string format)
      {
         Type = type;
         Value = value;
         Name = name;
         Position = position;
         NativeFormat = nativeFormat;
         Format = format;
      }

      /// <summary>
      /// Returns token name
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return Name;
      }
   }
}
