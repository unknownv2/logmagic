namespace LogMagic
{
   /// <summary>
   /// Configuration of logging filters
   /// </summary>
   public interface IFilterConfiguration
   {
      /// <summary>
      /// Adds a custom filter
      /// </summary>
      /// <param name="filter">Filter reference</param>
      /// <returns>Log configuration</returns>
      ILogConfiguration Custom(IFilter filter);
   }
}
