using BuilderGeneratorFactory.Define;

namespace BuilderGeneratorFactory.Options
{
   public class BuilderOptions
   {
      public BuilderPropertiesType PropertiesType { get; set; } = BuilderPropertiesType.Fields;
      public bool EnableAutoFixture { get; set; } = false;
   }
}