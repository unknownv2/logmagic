namespace LogMagic
{
   public interface IWriterConfiguration
   {
      ILogConfiguration Custom(ILogWriter writer);
   }
}
