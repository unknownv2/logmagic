namespace LogMagic
{
   /// <summary>
   /// Responsible for naming the log files
   /// </summary>
   interface ILogNamer
   {
      /// <summary>
      /// Called to give a name to the log file
      /// </summary>
      /// <param name="chunk">Log chunk currently logging</param>
      /// <returns>log name</returns>
      string GiveName(LogChunk chunk);
   }
}
