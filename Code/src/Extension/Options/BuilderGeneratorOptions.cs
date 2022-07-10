using BuilderGeneratorFactory.Define;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace BuilderGeneratorExtension
{
   internal partial class OptionsProvider
   {
      // Register the options with this attribute on your package class:
      // [ProvideOptionPage(typeof(OptionsProvider.BuilderGeneratorOptionsOptions), "BuilderGeneratorExtension", "BuilderGeneratorOptions", 0, 0, true, SupportsProfiles = true)]
      [ComVisible(true)]
      public class BuilderGeneratorOptionsOptions : BaseOptionPage<BuilderGeneratorOptions> { }
   }

   public class BuilderGeneratorOptions : BaseOptionModel<BuilderGeneratorOptions>
   {
      [Category("Generator")]
      [DisplayName("Properties Type")]
      [Description("Specifies if the generator will create the WithMethods using Fields or Variables. The Fields refers to object.FieldName and variables referes to _variableName.")]
      [DefaultValue(BuilderPropertiesType.Fields)]
      [TypeConverter(typeof(EnumConverter))]
      public BuilderPropertiesType PropertierType { get; set; } = BuilderPropertiesType.Fields;

      [Category("Test")]
      [DisplayName("Enable Auto Fixture")]
      [Description("Specifies if the generator will create the SetFixtureData and GetFixtureDataList to allow testings methods.")]
      [DefaultValue(true)]
      public bool EnableAutoFixture { get; set; } = true;

   }
}
