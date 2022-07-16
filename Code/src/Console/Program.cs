// Libraries
using BuilderGeneratorConsole;
using BuilderGeneratorFactory;
using BuilderGeneratorFactory.Options;
using System;
using System.IO;

namespace BuilderGenerator
{
   public class Program
   {
      public static void Main(string[] args)
      {
         Execute(args);
      }

      public static void Execute(string[] args)
      {
         if (args.Length != 1)
         {
            Console.WriteLine("Error: Invalid parameters.");
            Usage();
            return;
         }

         string fileClassName = args[0];

         try
         {
            ProcessFile(fileClassName);
         }
         catch (Exception ex)
         {
            Console.WriteLine("Error: Processing File Class Name [{0}]. {1}.", fileClassName, ex.Message);
            Usage();
         }
      }

      public static void Usage()
      {
         Console.WriteLine("Usage:");
         Console.WriteLine("> BuilderGenerator ClassFileName\n");
         Console.WriteLine("Example:");
         Console.WriteLine("> BuilderGenerator People.cs");
      }

      public static BuilderOptions GetBuilderOptions()
      {
         BuilderOptions builderOptions = new BuilderOptions();
         var startup = new Startup();
         if (startup.BuilderOptions != null)
         {
            builderOptions = startup.BuilderOptions;
         }

         return builderOptions;
      }

      public static void ProcessFile(string fileClassName)
      {
         if (File.Exists(fileClassName))
         {
            BuilderOptions builderOptions = GetBuilderOptions();

            var fullPath = Path.GetDirectoryName(fileClassName);
            var fileNameWithOutExtension = Path.GetFileNameWithoutExtension(fileClassName);

            string fullBuilderFileName = string.Format("{0}\\{1}Builder.cs", fullPath, fileNameWithOutExtension);

            string fileContent = File.ReadAllText(fileClassName);

            string builderClassContent = BuilderFactory.Generate(fileContent, builderOptions);

            File.WriteAllText(fullBuilderFileName, builderClassContent);

            Console.WriteLine("Builder File [{0}] was created.", fullBuilderFileName);
         }
         else
         {
            Console.WriteLine("Error: File Class Name [{0}] Not Exists.", fileClassName);
            Usage();
         }
      }
   }
}