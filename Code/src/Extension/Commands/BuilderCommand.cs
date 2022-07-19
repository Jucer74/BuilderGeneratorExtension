using BuilderGeneratorFactory;
using BuilderGeneratorFactory.Exceptions;
using BuilderGeneratorFactory.Options;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using System.IO;
using System.Threading.Tasks;

namespace BuilderGenerator
{
   [Command(PackageIds.BuildCommand)]
   internal sealed class BuilderCommand : BaseCommand<BuilderCommand>
   {
      protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
      {
         await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

         try
         {
            var dte2 = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE2;

            string fileClassName = await GetFileClassNameAsync(dte2);

            BuilderOptions builderOptions = GetOptions();

            string fullBuilderFileName  = CreateBuilderFile(fileClassName, builderOptions);

            AddFileToProjectAsync(dte2, fullBuilderFileName);

         }
         catch (Exception ex)
         {
            await VS.MessageBox.ShowErrorAsync("Builder Generator Extension", ex.Message);
         }
      }

      private async Task AddFileToProjectAsync(DTE2 dte2, string fullBuilderFileName)
      {
         await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

         var parentDirectory = Path.GetDirectoryName(fullBuilderFileName);

         ProjectItem parentProjectItem = dte2.Solution.FindProjectItem(parentDirectory);

         parentProjectItem.ProjectItems.AddFromFile(fullBuilderFileName);
      }

      private static string CreateBuilderFile(string fileClassName, BuilderOptions builderOptions)
      {
         var fullPath = Path.GetDirectoryName(fileClassName);

         var fileNameWithOutExtension = Path.GetFileNameWithoutExtension(fileClassName);

         string fullBuilderFileName = string.Format("{0}\\{1}Builder.cs", fullPath, fileNameWithOutExtension);

         string fileContent = File.ReadAllText(fileClassName);

         string builderClassContent = BuilderFactory.Generate(fileContent, builderOptions);

         File.WriteAllText(fullBuilderFileName, builderClassContent);

         return fullBuilderFileName;
      }

      private static BuilderOptions GetOptions()
      {
         var options = BuilderGeneratorOptions.Instance;

         BuilderOptions builderOptions = new BuilderOptions()
         {
            PropertiesType = options.PropertierType,
            EnableAutoFixture = options.EnableAutoFixture
         };

         return builderOptions;
      }

      private async Task<string> GetFileClassNameAsync(DTE2 dte2)
      {
         await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

         var selectedItems = dte2.SelectedItems;

         if (selectedItems == null)
         {
            throw new NoSelectedFileException("No Selected File");
         }

         if (selectedItems.MultiSelect || selectedItems.Count > 1)
         {
            throw new MultipleSelectedFilesException("Only One File must be selected");
         }

         //Get selected item
         var selectedItem = selectedItems.Item(1);

         //and selectedItem.Project will not be null.
         var projectItem = selectedItem.ProjectItem;
         // Get the Full filename
         var fileName = projectItem.FileNames[1];

         return fileName;
      }
   }
}