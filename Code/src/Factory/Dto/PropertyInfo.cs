namespace BuilderGeneratorFactory.Dto
{
   public class PropertyInfo
   {
      public string Type { get; }

      public string Name { get; }

      public string CamelCaseName
      {
         get
         {
            return string.Format("{0}{1}", Name.ToLower()[0], Name.Substring(1));
         }
      }

      public PropertyInfo(string type, string name)
      {
         this.Type = type;
         this.Name = name;
      }
   }
}