using BuilderGeneratorFactory;
using BuilderGeneratorFactory.Exceptions;
using BuilderGeneratorFactory.Options;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using System.IO;
using System.Linq;

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

            string fileClassName = GetFileClassName(dte2);

            BuilderOptions builderOptions = GetOptions();

            string builderFileName = CreateBuilderFile(fileClassName, builderOptions);

            AddFileToProject(dte2, builderFileName);
         }
         catch (Exception ex)
         {
            await VS.MessageBox.ShowErrorAsync("Builder Generator Extension", ex.Message);
         }
      }

      private static void AddFileToProject(DTE2 dte2, string builderFileName)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         FileInfo file = new FileInfo(builderFileName);

         EnvDTE.Project project = dte2.SelectedItems.Item(1).ProjectItem.ContainingProject;

         ProjectItems parentProjectItems = GetProjectItems(project, file);

         parentProjectItems.AddFromFile(builderFileName);
      }

      private static ProjectItems GetProjectItems(EnvDTE.Project project, FileInfo file)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         var directoryFullPath = file.DirectoryName;

         var projectDirectoryFullPath = Path.GetDirectoryName(project.FullName);

         if (projectDirectoryFullPath.Equals(directoryFullPath))
         {
            return project.ProjectItems;
         }

         var parentDirectoryName = file.Directory.Name;

         return project.ProjectItems.Cast<ProjectItem>()
                                    .FirstOrDefault(i =>
                                    {
                                       ThreadHelper.ThrowIfNotOnUIThread();
                                       return i.Name.Equals(parentDirectoryName, StringComparison.OrdinalIgnoreCase);
                                    })
                                    .ProjectItems;
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

      private static string GetFileClassName(DTE2 dte2)
      {
         ThreadHelper.ThrowIfNotOnUIThread();

         var selectedItems = dte2.SelectedItems;

         if (selectedItems == null)
         {
            throw new NoSelectedFileException("No Selected File");
         }

         if (selectedItems.MultiSelect || selectedItems.Count > 1)
         {
            throw new MultipleSelectedFilesException("Only One File must be selected");
         }

         var selectedItem = selectedItems.Item(1);
         var projectItem = selectedItem.ProjectItem;
         var fileName = projectItem.FileNames[1];

         return fileName;
      }
   }
}