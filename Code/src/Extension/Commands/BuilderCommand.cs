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
            //var dte2 = await ServiceProvider.GetGlobalServiceAsync(typeof(SDTE)) as DTE2;
            var dte2 = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE2;

            var options = BuilderGeneratorOptions.Instance;

            BuilderOptions builderOptions = new BuilderOptions()
            {
               PropertiesType = options.PropertierType,
               EnableAutoFixture = options.EnableAutoFixture
            };

            string fileClassName = await GetFileClassNameAsync(dte2);

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
         var projectName = projectItem.Name;
         // Get the Ful filename
         var fileName = projectItem.FileNames[1];

         var project = selectedItem.Project;

         ProjectItem parentProjectItem = dte2.Solution.FindProjectItem(fileName);

         VS.MessageBox.Show(parentProjectItem.Name, parentProjectItem.Kind);

         //var project = projectItem.ContainingProject;

         //if (project != null)
         //{
         //   var projectItems = project.ProjectItems;
         //   int projectItemsCount = projectItems.Count;
         //   ProjectItem item;
         //   for (int i = 1; i <= projectItemsCount; i++ )
         //   {
         //      item = projectItems.Item(i);
         //      VS.MessageBox.Show(item.Name, item.Kind);
         //   }
         //}


         //project.ProjectItems.AddFromFile(fileBuildName).Properties.Item((object)"ItemType").Value = (object)"Compile";



         return fileName;
      }
   }
}