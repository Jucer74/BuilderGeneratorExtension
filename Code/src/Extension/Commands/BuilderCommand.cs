using BuilderGeneratorFactory;
using BuilderGeneratorFactory.Define;
using BuilderGeneratorFactory.Exceptions;
using BuilderGeneratorFactory.Options;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using System.IO;

namespace BuilderGenerator
{
   [Command(PackageIds.BuildCommand)]
   internal sealed class BuilderCommand : BaseCommand<BuilderCommand>
   {
      protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
      {
         try
         {
            await VS.MessageBox.ShowWarningAsync("Line 1", "Line 2");

            string fileClassName = "D:\\Documents\\My Repositories\\BuilderGeneratorExtension\\Entities\\Person.cs";

            await GetFileClassNameAsync();

            BuilderOptions builderOptions = new BuilderOptions()
            {
               PropertiesType = BuilderPropertiesType.Fields,
               EnableAutoFixture = true
            };

            var fullPath = Path.GetDirectoryName(fileClassName);
            var fileNameWithOutExtension = Path.GetFileNameWithoutExtension(fileClassName);

            string fullBuilderFileName = string.Format("{0}\\{1}Builder.cs", fullPath, fileNameWithOutExtension);

            string fileContent = File.ReadAllText(fileClassName);

            string builderClassContent = BuilderFactory.Generate(fileContent, builderOptions);

            File.WriteAllText(fullBuilderFileName, builderClassContent);
         }
         catch (Exception ex)
         {
            await VS.MessageBox.ShowErrorAsync("Builder Generator Extension", ex.Message);
         }
      }

      private async Task GetFileClassNameAsync()
      {
         await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

         var dte2 = await ServiceProvider.GetGlobalServiceAsync(typeof(SDTE)) as DTE2;

         var selectedItems = dte2.SelectedItems;
         if (selectedItems == null)
         {
            throw new NoSelectedFileException("No Selected File");
         }

         if (selectedItems.MultiSelect || selectedItems.Count > 1)
         {
            throw new MultipleSelectedFilesException("Only One File must be selected");
         }
      }
   }
}