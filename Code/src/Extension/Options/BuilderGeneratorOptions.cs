using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using BuilderGeneratorFactory.Define;

namespace BuilderGeneratorExtension.Options
{
   public class BuilderGeneratorOptions : DialogPage
   {
      [Category("Generator")]
      [DisplayName("Properties Type")]
      [Description("It defines the method to implement the build properties, can be Variables or Fileds")]
      [DefaultValue(BuilderPropertiesType.Variables)]
      [TypeConverter(typeof(EnumConverter))]
      public BuilderPropertiesType PropertyType { get; set; } = BuilderPropertiesType.Variables;

      [Category("Test")]
      [DisplayName("Enable Fixture")]
      [Description("It Allows enable the Auto Fixture for the Build properties")]
      [DefaultValue(false)]
      public bool EnableFixture{ get; set; } = true;


   }
}