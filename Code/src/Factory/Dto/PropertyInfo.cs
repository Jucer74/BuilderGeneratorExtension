using BuilderGeneratorFactory.Extension;

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
            return Name.ToCamelCase();
         }
      }

      public PropertyInfo(string type, string name)
      {
         this.Type = type;
         this.Name = name;
      }
   }
}