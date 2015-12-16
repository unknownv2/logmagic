namespace LogMagic
{
   /// <summary>
   /// Performs formatting of a log chunk to a string
   /// </summary>
   public interface ILogChunkFormatter
   {
      /// <summary>
      /// Formats a log chunk into a string
      /// </summary>
      /// <param name="chunk"></param>
      /// <returns></returns>
      string Format(LogChunk chunk);
   }
}
