namespace BuilderGeneratorFactory.Extension
{
   public static class StringExtension
   {
      public static string ToCamelCase(this string word)
      {
         return string.Format("{0}{1}", word.ToLower()[0], word.Substring(1));
      }
   }
}