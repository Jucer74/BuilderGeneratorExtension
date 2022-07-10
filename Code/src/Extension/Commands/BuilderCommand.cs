namespace BuilderGeneratorExtension
{
   [Command(PackageIds.BuildCommand)]
   internal sealed class BuilderCommand : BaseCommand<BuilderCommand>
   {
      protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
      {
         //string fileClassName = "D:\\Documents\\My Repositories\\BuilderGeneratorExtension\\Entities\\Person.cs";

         //BuilderOptions builderOptions = new BuilderOptions()
         //{
         //   PropertiesType = BuilderPropertiesType.Fields,
         //   EnableAutoFixture = true
         //};

         //var fullPath = Path.GetDirectoryName(fileClassName);
         //var fileNameWithOutExtension = Path.GetFileNameWithoutExtension(fileClassName);

         //string fullBuilderFileName = string.Format("{0}\\{1}Builder.cs", fullPath, fileNameWithOutExtension);

         //string fileContent = File.ReadAllText(fileClassName);

         //string builderClassContent = BuilderFactory.Generate(fileContent, builderOptions);

         //File.WriteAllText(fullBuilderFileName, builderClassContent);

         await VS.MessageBox.ShowWarningAsync("BuilderGeneratorExtension", "Button clicked");
      }
   }
}