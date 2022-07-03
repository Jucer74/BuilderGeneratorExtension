namespace BuilderGeneratorFactory.Define
{
   public enum BuilderPropertiesType
   {
      Fields,
      Variables
   }

   public static class BuilderConstants
   {
      public static readonly string NameSpace = "namespace";
      public static readonly string PropertyRegex = "(?>public)\\s+(?!class)((static|readonly)\\s)?(?<Type>(\\S+(?:<.+?>)?)(?=\\s+\\w+\\s*\\{\\s*get))\\s+(?<Name>[^\\s]+)(?=\\s*\\{\\s*get)";
      public static readonly string ClassRegex = "\\s+(class)\\s+(?<Name>[^\\s:]+)";
      public static readonly string OpenBrace = "{";
      public static readonly string CloseBrace = "}";
   }
}